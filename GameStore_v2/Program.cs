using BLL.AutoMapper;
using BLL.Services;
using DAL.Repositories;
using FluentAssertions.Common;
using FluentValidation;
using FluentValidation.AspNetCore;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Games;
using GameStore.BLL.Services;
using GameStore.BLL.Validators;
using GameStore.DAL.Data;
using GameStore.DAL.Models.AuthModels;
using GameStore.DAL.MongoRepositories;
using GameStore.DAL.Repositories;
using GameStore.DAL.Repositories.AuthRepositories;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore.WEB.AuthUtilities;
using GameStore.WEB.Middleware.Exstensions;
using GameStore.WEB.ServiceCollections;
using GameStore_DAL.Data;
using GameStore_DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using System.Configuration;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString2 = builder.Configuration.GetConnectionString("DefaultConnection2");
// Add services to the container.
//builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GameStoreDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddDbContext<AuthDbContext>(options => {
    options.UseMySql(connectionString2, ServerVersion.AutoDetect(connectionString2));
});

var b = builder.Configuration["JWT:Secret"];
builder.Services.AddIdentity<User, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options => {
    
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    
                .AddJwtBearer(options => {
                    options.Events = new JwtBearerEvents {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Headers["Authorization"];
                            context.Token = token;

                            return Task.CompletedTask;
                        },
                    };
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters() {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"])),
                        ValidateIssuer=true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidateAudience=true,
                        ValidAudience = builder.Configuration["JWT:Audience"]
                    };
                });


builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider,PolicyHandler>();
                

builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
                                          .AddNewtonsoftJson(options =>
                                                             options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddScoped<PermissionRepository>();
builder.Services.AddScoped<PermissionRoleRepository>();
builder.Services.AddMemoryCache();

builder.Services.AddDataAccessLayerDependencies();
builder.Services.AddBusinessLogicLayerDependencies();
builder.Services.AddFluentValidationDependencies();

builder.Services.AddScoped<IMongoDatabase>(_ =>
new
MongoClient("mongodb://localhost:27017").GetDatabase("Northwind"));
builder.Services.AddScoped<ShippersService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddLazyCache();
builder.Services.AddCors(p => p.AddPolicy("corspolicy", (build) =>
{
    build.WithOrigins("http://127.0.0.1:8080").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    
}));


var app = builder.Build();
RoleIntializer.AddNewRoles(app).GetAwaiter().GetResult();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("corspolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});


bool.TryParse(builder.Configuration["Serilog:EnableCustomLogger"], out bool enableCustomLogger);
app.UseIpLogger(builder.Configuration.GetValue<string>("Logging:IpPath"),enableCustomLogger);
app.UseGlobalExceptionMiddleware();
app.UsePerformanceLogging(builder.Configuration.GetValue<string>("Logging:PerformancePath"),enableCustomLogger);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


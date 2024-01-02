using BLL.AutoMapper;
using BLL.Services;
using DAL.Repositories;
using FluentAssertions.Common;
using GameStore.BLL.Services;
using GameStore.DAL.Repositories;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Repositories;
using GameStore_v2.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
//builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GameStoreDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IGamesRepository,GamesRepository>();
builder.Services.AddScoped<IGenreRepository,GenreRepository>();
builder.Services.AddScoped<IGameGenreRepository,GameGenreRepository>();
builder.Services.AddScoped<IPlatformRepository,PlatformRepository>();
builder.Services.AddScoped<IGamePlatformRepository,GamePlatformRepository>();
builder.Services.AddScoped<IPublisherRepository,PublisherRepository>();
builder.Services.AddScoped<IOrderCartRepository,OrderCartRepository>();
builder.Services.AddScoped<ICommentRepository,CommentRepository>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<PlatformService>();
builder.Services.AddScoped<PublisherService>();
builder.Services.AddScoped<OrderCartService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddLazyCache();
builder.Services.AddCors(p => p.AddPolicy("corspolicy", (build) =>
{
    build.WithOrigins("http://127.0.0.1:8080").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();

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


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

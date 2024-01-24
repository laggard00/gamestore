using FluentValidation;
using FluentValidation.AspNetCore;
using GameStore.BLL.Validators;

namespace GameStore.WEB.ServiceCollections {
    public static class FluentValidationExtensions {
        public static IServiceCollection AddFluentValidationDependencies(this IServiceCollection services) {
            // services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

           // services.AddValidatorsFromAssemblyContaining<AddGameRequestValidator>();
           // services.AddValidatorsFromAssemblyContaining<UpdateGameRequestValidator>();
           // services.AddValidatorsFromAssemblyContaining<AddCommentRequestValidator>();
           // services.AddValidatorsFromAssemblyContaining<AddGenreRequestValidator>();
           // services.AddValidatorsFromAssemblyContaining<AddPlatfromRequestValidator>();
           // services.AddValidatorsFromAssemblyContaining<AddPublisherRequestValidator>();
           // services.AddValidatorsFromAssemblyContaining<UpdateGenreRequestValidator>();
           // services.AddValidatorsFromAssemblyContaining<UpdatePublisherRequestValidator>();
           // services.AddValidatorsFromAssemblyContaining<UpdatePlatformRequestValidator>();
           //
            return services;
        }
    }
}

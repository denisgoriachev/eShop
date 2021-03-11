using eShop.Api.Filters;
using eShop.Api.Services;
using eShop.Application.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://eshop-auth:80";
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = "eShop"
                    };
                });

            services.AddHttpContextAccessor();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IIdentityService, IdentityService>();

            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
            });

            services.AddOpenApiDocument(document =>
            {
                document.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "eShop API";
                    document.Info.Description = "DDD + CQRS eShop API";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Denis Goriachev",
                    };
                };

                document.AddSecurity("OAuth2", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Description = "My Authentication",
                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = "https://localhost:5101/connect/authorize",
                            TokenUrl = "https://localhost:5101/connect/token",
                            Scopes = new Dictionary<string, string>
                            {
                                {"eShop.admin", "eShop API Admin Access"}
                            }
                        }
                    }
                });

                document.OperationProcessors.Add(
                    new OperationSecurityScopeProcessor("OAuth2"));
            });

            return services;
        }
    }
}

using eShop.Api.Filters;
using eShop.Api.Services;
using eShop.Application.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
                    options.Authority = "https://localhost:5101";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultScheme = "Cookies";
            //        options.DefaultChallengeScheme = "oidc";
            //    })
            //    .AddCookie("Cookies")
            //    .AddOpenIdConnect("oidc", options =>
            //    {
            //        options.Authority = "http://localhost:5101";

            //        // This will allow the container to reach the discovery endpoint
            //        options.MetadataAddress = "http://eshop-auth/.well-known/openid-configuration";
            //        options.RequireHttpsMetadata = false;

            //        options.Events.OnRedirectToIdentityProvider = context =>
            //        {
            //            // Intercept the redirection so the browser navigates to the right URL in your host
            //            context.ProtocolMessage.IssuerAddress = "http://localhost:5001/connect/authorize";
            //            return Task.CompletedTask;
            //        };

            //        options.ClientId = "eshop-api";
            //        options.ClientSecret = "secret";
            //        options.ResponseType = "code";

            //        options.SaveTokens = true;

            //        options.Scope.Add("profile");
            //        options.GetClaimsFromUserInfoEndpoint = true;
            //    });

            services.AddHttpContextAccessor();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IIdentityService, IdentityService>();

            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eShop.Api", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }
    }
}


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SpredMedia.CommonLibrary
{
    public static class SwaggerDoc 
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services, string Service, string assemble)
        {
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(Service, new OpenApiInfo()
                {
                    Title = $"{Service} API",
                    Version = "v1",
                    Description = $"A Web API for the {Service} services "
                });

                setupAction.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                if (Service != "GateWay")
                {
                    //Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{assemble}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    setupAction.IncludeXmlComments(xmlPath);
                }
                


            // To Enable authorization using Swagger (JWT) 
            setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                setupAction.AddSecurityDefinition("Username", new OpenApiSecurityScheme
                {
                    Description = "Username",
                    Name = "Username",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                setupAction.AddSecurityDefinition("Password", new OpenApiSecurityScheme
                {
                    Description = "Password",
                    Name = "Password",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Username"
                            }
                        },
                        new string[] {}
                    },

                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Password"
                            }
                        },
                        new string[] {}
                    },

                });
                // please come back to add the value for the iamge input for swagger documentations
                setupAction.OperationFilter<FileUploadOperation>();
            });
        }

        public class FileUploadOperation : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (operation.OperationId == "MyOperation")
                {
                    operation.Parameters.Clear();
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "uploadedFile",
                        In = ParameterLocation.Header,
                        Description = "Upload File",
                        Required = true,
                        Schema = new OpenApiSchema
                        {
                            Type = "file",
                            Format = "binary"
                        }
                    });
                    var uploadFileMediaType = new OpenApiMediaType()
                    {
                        Schema = new OpenApiSchema()
                        {
                            Type = "object",
                            Properties =
                    {
                        ["uploadedFile"] = new OpenApiSchema()
                        {
                            Description = "Upload File",
                            Type = "file",
                            Format = "binary"
                        }
                    },
                            Required = new HashSet<string>()
                        {
                            "uploadedFile"
                        }
                        }
                    };
                    operation.RequestBody = new OpenApiRequestBody
                    {
                        Content =
                    {
                        ["multipart/form-data"] = uploadFileMediaType
                    }
                    };
                }
            }
        }
    }
}

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace SocialMedia.API.OpenApi;

internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Name         = "Authorization",
            Type         = SecuritySchemeType.Http,
            Scheme       = "bearer",
            BearerFormat = "JWT",
            In           = ParameterLocation.Header,
            Description  = "Insira o token JWT. Exemplo: Bearer eyJhbGci..."
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer")] = []
        };

        if (document.Paths is null) return Task.CompletedTask;

        foreach (var path in document.Paths.Values)
            foreach (var operation in path.Operations.Values)
            {
                operation.Security ??= [];
                operation.Security.Add(securityRequirement);
            }

        return Task.CompletedTask;
    }
}
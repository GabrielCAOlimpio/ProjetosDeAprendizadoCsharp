using Microsoft.EntityFrameworkCore;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Domain.Interfaces.Users;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Domain.Services;
using SocialMedia.Domain.Interfaces.Posts;
using SocialMedia.Domain.Interfaces.Comments;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Infrastructure.Security;
using SocialMedia.Domain.Interfaces.Auth;
using SocialMedia.Infrastructure.Security.Auth;
using System.IdentityModel.Tokens.Jwt;
using Scalar.AspNetCore;
using Asp.Versioning;
using SocialMedia.API.Middleware;
using SocialMedia.API.OpenApi;

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// --- BANCO DE DADOS ---
builder.Services.AddDbContext<SocialMediaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- SEGURANÇA (JWT) ---
var secretKey = builder.Configuration["JwtSettings:Secret"];
if (string.IsNullOrEmpty(secretKey))
    throw new InvalidOperationException("Chave JWT não encontrada nas configurações.");

var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey         = new SymmetricSecurityKey(key),
        ValidateIssuer           = true,
        ValidIssuer              = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience         = true,
        ValidAudience            = builder.Configuration["JwtSettings:Audience"],
        ClockSkew                = TimeSpan.Zero
    };

#if DEBUG
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine("[JWT] Falha: " + ctx.Exception?.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = ctx =>
        {
            Console.WriteLine("[JWT] Válido: " + ctx.Principal?.Identity?.Name);
            return Task.CompletedTask;
        },
        OnChallenge = ctx =>
        {
            Console.WriteLine("[JWT] Challenge: " + ctx.Error + " | " + ctx.ErrorDescription);
            return Task.CompletedTask;
        }
    };
#endif
});

builder.Services.AddAuthorization();

// --- INJEÇÃO DE DEPENDÊNCIA ---
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IPasswordHasher, BCryptHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// --- VERSIONAMENTO ---
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion                   = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions                   = true;
})
.AddApiExplorer(opt =>
{
    opt.GroupNameFormat           = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();

// --- OPENAPI / SCALAR ---
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// -------------------------------------------------------

var app = builder.Build();


app.UseMiddleware<ExceptionHandler>();

if (!app.Environment.IsProduction())
{
    app.MapOpenApi("/openapi/{documentName}.json");

    app.MapScalarApiReference(options =>
    {
        options.Title               = "Social Media API";
        options.OpenApiRoutePattern = "/openapi/{documentName}.json";
        options.Authentication      = new ScalarAuthenticationOptions
        {
            PreferredSecuritySchemes = ["Bearer"]
        };
    });

    app.MapGet("/", (HttpContext context) =>
    {
        context.Response.Redirect("/scalar/v1");
        return Task.CompletedTask;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
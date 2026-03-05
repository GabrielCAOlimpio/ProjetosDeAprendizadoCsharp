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

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// --- BANCO DE DADOS ---
builder.Services.AddDbContext<SocialMediaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- SEGURANÇA (JWT) ---
var secretKey = builder.Configuration["JwtSettings:Secret"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new Exception("ERRO: Chave JWT não encontrada no appsettings.json!");
}

var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };

    // Logs de debug — pode remover depois que estiver funcionando
    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine("❌ JWT FALHOU: " + ctx.Exception?.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = ctx =>
        {
            Console.WriteLine("✅ JWT VÁLIDO: " + ctx.Principal?.Identity?.Name);
            return Task.CompletedTask;
        },
        OnChallenge = ctx =>
        {
            Console.WriteLine("🔒 CHALLENGE: " + ctx.Error + " | " + ctx.ErrorDescription);
            return Task.CompletedTask;
        }
    };
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
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
})
.AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();

// --- SCALAR (substitui o Swagger) ---
if (!builder.Environment.IsProduction())
{
    builder.Services.AddOpenApi();
}

// -------------------------------------------------------

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Social Media API";
        options.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecurityScheme = "Bearer"
        };
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
using System.Transactions;
using FinTrack.API.Interfaces.Balances;
using FinTrack.API.Interfaces.Transaction;
using FinTrack.API.Interfaces.User;
using FinTrack.API.Middlewares;
using FinTrack.API.Services;
using FinTrack.API.Services.Balances;
using FinTrack.API.Services.Transactions;
using FinTrack.Domain.Entities;
using FinTrack.Domain.Interfaces.Balances;
using FinTrack.Domain.Interfaces.Security;
using FinTrack.Domain.Interfaces.Transactions;
using FinTrack.Domain.Interfaces.User;
using FinTrack.Infrastructure.Data;
using FinTrack.Infrastructure.Repositories.Balances;
using FinTrack.Infrastructure.Repositories.Transaction;
using FinTrack.Infrastructure.Repositories.User;
using FinTrack.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- BANCO DE DADOS ---
builder.Services.AddDbContext<FinTrackDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");


// --- INJEÇÃO DE DEPENDÊNCIAS ---
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, BCryptHasher>();


// --- CONTROLLERS ---
builder.Services.AddControllers();

// --- SWAGGER ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------------------------------------------

var app = builder.Build();

app.UseMiddleware<ExceptionHandler>();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
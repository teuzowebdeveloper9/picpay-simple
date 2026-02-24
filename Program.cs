using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SimplePicPay.Infra;
using SimplePicPay.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Services
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<ITransferService, TransferService>();

// Controllers
builder.Services.AddControllers();

// OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PicPay Simplificado API",
        Version = "v1",
        Description = "API de pagamentos simplificada - transferências entre usuários e lojistas"
    });
});

var app = builder.Build();

// Swagger UI disponível em qualquer ambiente
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PicPay Simplificado API v1");
    c.RoutePrefix = string.Empty; // Swagger na raiz
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

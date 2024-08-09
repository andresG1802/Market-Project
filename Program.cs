using Microsoft.EntityFrameworkCore;
using MiApi.Data;
using MiApi.Middlewares;
var builder = WebApplication.CreateBuilder(args);

// Configurar servicios necesarios
builder.Services.AddControllers();

// Configurar DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext >(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configurar el pipeline de la aplicación
app.UseHttpsRedirection();
app.UseAuthorization();
// app.UseNameLengthMiddleware();//Agregando Middleware
app.MapControllers();
app.Run();


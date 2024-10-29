using Models;
using DotNetEnv;
using Services;


var builder = WebApplication.CreateBuilder(args);
string myAllowSpecificOrigins = Environment.GetEnvironmentVariable("MY_ALLOW_SPECIFIC_ORIGINS");

Env.Load();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(myAllowSpecificOrigins)
              .WithMethods("GET", "POST", "DELETE", "PUT")
              .AllowAnyHeader();
    });
});
// Add services to the container.

var services = builder.Services;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Services
services.AddScoped<CartuchoService>();
services.AddScoped<ImpresoraService>();
services.AddScoped<OficinaService>();
services.AddScoped<Asignar_ImpresoraService>();


//BDD
builder.Services.AddDbContext<Gestion_Cartuchos_Context>();

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

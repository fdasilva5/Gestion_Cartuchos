using Models;
using DotNetEnv;
using Services;


var builder = WebApplication.CreateBuilder(args);

Env.Load();
string myAllowSpecificOrigins = Environment.GetEnvironmentVariable("MY_ALLOW_SPECIFIC_ORIGINS");


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

var services = builder.Services;
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Services
services.AddScoped<CartuchoService>();
services.AddScoped<ImpresoraService>();
services.AddScoped<OficinaService>();
services.AddScoped<Asignar_ImpresoraService>();
services.AddScoped<RecargaService>();
services.AddScoped<ModeloService>();
services.AddScoped<EstadoService>();


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

app.UseCors("MyAllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Models;
using DotNetEnv;
using Services;


var builder = WebApplication.CreateBuilder(args);

Env.Load();
string myAllowSpecificOrigins = Environment.GetEnvironmentVariable("MY_ALLOW_SPECIFIC_ORIGINS");


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

services.AddControllers();
services.AddMemoryCache(); //falta inyectar en los servicios
builder.Logging.AddJsonConsole();   
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(); 

//Services
services.AddScoped<ICartuchoService,CartuchoService>();
services.AddScoped<IImpresoraService,ImpresoraService>();
services.AddScoped<IOficinaService,OficinaService>();
services.AddScoped<IAsignar_Impresora_Service,Asignar_ImpresoraService>();
services.AddScoped<IRecargaService,RecargaService>();
services.AddScoped<IModeloService,ModeloService>();
services.AddScoped<IEstadoService,EstadoService>();


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

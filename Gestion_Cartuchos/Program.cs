using Models;
using DotNetEnv;
using Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<Gestion_Cartuchos_Context>()
    .AddDefaultTokenProviders();

Env.Load();
string myAllowSpecificOrigins = Environment.GetEnvironmentVariable("MY_ALLOW_SPECIFIC_ORIGINS");


builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins", policy =>
    {
       // Asegúrate de que myAllowSpecificOrigins no sea null ni vacío
        if (myAllowSpecificOrigins != null && myAllowSpecificOrigins.Length > 0)
        {
            policy.WithOrigins("http://localhost", "http://localhost:80", "http://localhost:3000", "http://localhost:8080")
                  .WithMethods("GET", "POST", "DELETE", "PUT")
                  .AllowAnyHeader();
        }
        else
        {
            // Si no se configura correctamente, puedes aplicar una política más permisiva o lanzar un error
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

// Configurar autenticación con cookies
// services.ConfigureApplicationCookie(options =>
// {
//     options.LoginPath = "/localhost/auth/login";  // URL para el login
//     options.LogoutPath = "/localhost/auth/logout";  // URL para el logout
//     options.SlidingExpiration = true;
//     options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Tiempo de expiración de la cookie
// });


var services = builder.Services;

services.AddAuthorization();
services.AddAuthentication();

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

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();

//BDD
builder.Services.AddDbContext<Gestion_Cartuchos_Context>();

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyAllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllers();

app.Run();

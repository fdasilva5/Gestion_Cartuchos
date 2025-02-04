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
        policy.WithOrigins(myAllowSpecificOrigins)
              .WithMethods("GET", "POST", "DELETE", "PUT")
              .AllowAnyHeader();
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

// Configure the HTTP request pipeline.
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

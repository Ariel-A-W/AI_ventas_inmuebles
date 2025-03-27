using ventas_inmuebles.Server.Application.UsesCases.Casas;
using ventas_inmuebles.Server.Application.UsesCases.Localidades;
using ventas_inmuebles.Server.Application.UsesCases.TiposCasas;
using ventas_inmuebles.Server.Domain.Casas;
using ventas_inmuebles.Server.Domain.Localidades;
using ventas_inmuebles.Server.Domain.TiposCases;
using ventas_inmuebles.Server.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

// Agregar política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaCors",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILocalidad, LocalidadRepository>();
builder.Services.AddScoped<LocalidadesUseCase>();

builder.Services.AddScoped<ITipoCasa, TiposCasasRepository>();
builder.Services.AddScoped<TiposCasasUseCase>();

builder.Services.AddScoped<ICasa, CasasRepository>();
builder.Services.AddScoped<CasasUseCase>();

var app = builder.Build();

// Usar la política de CORS antes de los endpoints
app.UseCors("PoliticaCors");

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

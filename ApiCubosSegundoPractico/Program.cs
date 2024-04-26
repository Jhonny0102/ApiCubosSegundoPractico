using ApiCubosSegundoPractico.Data;
using ApiCubosSegundoPractico.Helpers;
using ApiCubosSegundoPractico.Repositories;
using Microsoft.EntityFrameworkCore;
using NSwag.Generation.Processors.Security;
using NSwag;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Añadimos Helper.
HelperActionServicesOauth helper = new HelperActionServicesOauth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOauth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());

//Configuracion KeyVault
builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret secret = await secretClient.GetSecretAsync("SqlAzure");
KeyVaultSecret issuer = await secretClient.GetSecretAsync("Issuer");
KeyVaultSecret audience = await secretClient.GetSecretAsync("Audience");
KeyVaultSecret secrectkey = await secretClient.GetSecretAsync("SecretKey");
string connectionString = secret.Value;

//Añadimos repos y conexion
//string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryCubo>();
builder.Services.AddDbContext<CuboContext>(option => option.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//Modificamos para que aparezca el autorizathion en la web.
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Api Segunda practica Cubos";
    document.Description = "Api segunda practica azure Cubos";
    // CONFIGURAMOS LA SEGURIDAD JWT PARA SWAGGER,
    // PERMITE AÑADIR EL TOKEN JWT A LA CABECERA.
    document.AddSecurity("JWT", Enumerable.Empty<string>(),
        new NSwag.OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Copia y pega el Token en el campo 'Value:' así: Bearer {Token JWT}."
        }
    );
    document.OperationProcessors.Add(
    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

var app = builder.Build();

//Modificamos para que aparezca el authorization en la web.
app.UseOpenApi();

//Modifcamos para subir a la WEB.
//app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api crud coches v1");
    options.RoutePrefix = "";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

//Indicamos que usara authentication.
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

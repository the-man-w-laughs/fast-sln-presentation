using FastSlnPresentation.Server.Extensions;
using FastSlnPresentation.BLL.Extensions;
using FastSlnPresentation.Server.Middlewares;
using FastSlnPresentation.Server.Security;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddJwtBearerAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(config);
builder.Services.AddKebabControllers();
builder.Services.AddDefaultCors();
builder.Services.RegisterServices();
builder.Services.RegisterDALDependencies(config);
builder.Services.RegisterAutomapperProfiles();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRootRedirect("swagger");

app.Run();

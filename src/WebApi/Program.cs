using Application;
using AspNetCoreRateLimit;
using Infrastructure;
using WebApi;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
    o.AddPolicy("Mutulink Admin", builder =>
    {
        builder
        .AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }
    ));

builder.Services.AddControllers();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentitySettings();
builder.Services.AddApplicationServices();

builder.Services
    .AddJwtAuthentication(builder.Services
    .GetApplicationSettings(builder.Configuration));

builder.Services.AddIdentityServices();
builder.Services.AddLinkService();
builder.Services.AddInfrastructureDependencies();

builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterSwagger();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();  


var app = builder.Build();

app.SeedDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Mutulink Admin");
app.UseIpRateLimiting();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.Run();

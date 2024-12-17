using Application;
using Infrastructure;
using WebApi;

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
builder.Services.AddEmployeeService();
builder.Services.AddInfrastructureDependencies();

builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterSwagger();


var app = builder.Build();

app.SeedDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Mutulink Admin");
app.UseAuthorization();

app.MapControllers();

app.Run();

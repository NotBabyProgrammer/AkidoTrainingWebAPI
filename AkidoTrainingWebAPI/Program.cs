using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AkidoTrainingWebAPI.DataAccess.Data;
using AkidoTrainingWebAPI.BusinessLogic.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AkidoTrainingWebAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AkidoTrainingWebAPIContext") ?? throw new InvalidOperationException("Connection string 'AkidoTrainingWebAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin",
    build =>
    {
        build.WithOrigins("*")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddScoped<AccountRepository>();

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

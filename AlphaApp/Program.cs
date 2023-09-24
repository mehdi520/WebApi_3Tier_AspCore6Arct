using AlphaApp.ApplicationServices.Contracts;
using AlphaApp.ApplicationServices.Services;
using AlphaApp.Repositories.Contracts;
using AlphaApp.Repositories.DatabaseContexts;
using AlphaApp.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AlphaAppContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AlphaAppDB"));
    options.UseLazyLoadingProxies(true);
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.DataAcessObjecImpl;
using DataAccess.Demo.Dbcontext;
using DataAccess.Demo.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddDbContext<MyShopDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("ConnStrName"), b => b.MigrationsAssembly("BE_ASPNET_2911")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IMyShopUnitOfWork, MyShopUnitOfWork>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

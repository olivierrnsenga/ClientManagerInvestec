using ClientManager.Core.DataInterface.DeskBooker.Core.DataInterface;
using ClientManager.Core.Processor;
using ClientManager.DataAccess;
using ClientManager.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ClientContext>(options =>
    options.UseInMemoryDatabase("ClientDatabase")); 

builder.Services.AddScoped<IClientRequestProcessor, ClientRequestProcessor>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
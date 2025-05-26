using EventSourcingDemo.Application.Commands;
using EventSourcingDemo.Application.Projector;
using EventSourcingDemo.Application.Query;
using EventSourcingDemo.MongoDb;

namespace EventSourcingDemo.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateReservationCommand>());

        builder.Services.AddScoped<TablesStore, MongoDbTablesStore>();
        builder.Services.AddScoped<TablesCollection, MongoDbTablesRepository>();
        builder.Services.AddScoped<OrderCollection, MongoDbOrderRepository>();
        builder.Services.AddScoped<MongoDbTablesRepository>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventSourcingDemo API V1");
                c.RoutePrefix = string.Empty; // Launches Swagger at root
            });
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}
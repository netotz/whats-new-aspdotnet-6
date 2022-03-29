using Microsoft.EntityFrameworkCore;

using System.Net;

using WiredBrainCoffee.MinApi;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Orders") ?? "Data Source=Orders.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlite<OrderDbContext>(connectionString);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", builder =>
    {
        builder.AllowAnyOrigin();
    });
});
builder.Services.AddHttpClient();

var app = builder.Build();

await CreateDb(app.Services, app.Logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.MapGet("/orderStatus", async (IHttpClientFactory httpFactory) =>
{
    var httpClient = httpFactory.CreateClient();
    var url = "http://wiredbraincoffee.azurewebsites.net/api/ordersystemstatus";
    try
    {
        var response = await httpClient.GetFromJsonAsync<OrderSystemStatus>(url);
        return Results.Ok(response);
    }
    catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    {
        return Results.NotFound();
    }
})
    .WithName("get order status");

app.MapGet("/orders", async (OrderDbContext orderDb) =>
    Results.Ok(await orderDb.Orders.ToListAsync()))
    .WithName("get orders");

app.MapGet("/orders/{id}", async (OrderDbContext orderDb, int id) =>
{
    var order = await orderDb.Orders.FirstOrDefaultAsync(o => o.Id == id);

    return order is null ? Results.NotFound() : Results.Ok(order);
})
    .WithName("get order");

app.MapPost("/orders", async (OrderDbContext orderDb, Order newOrder) =>
{
    orderDb.Orders.Add(newOrder);
    await orderDb.SaveChangesAsync();

    return Results.Created($"/orders/{newOrder.Id}", newOrder);
})
    .WithName("post order");

app.MapPut("/orders/{id}", async (OrderDbContext orderDb, int id, Order updatedOrder) =>
{
    var order = await orderDb.Orders
        .FirstOrDefaultAsync(o => o.Id == id);

    if (order == null)
    {
        return;
    }

    order.Created = updatedOrder.Created;
    order.Description = updatedOrder.Description;
    order.OrderNumber = updatedOrder.OrderNumber;
    order.PromoCode = updatedOrder.PromoCode;
    order.Total = updatedOrder.Total;

    await orderDb.SaveChangesAsync();
})
    .WithName("update order");

app.MapDelete("/orders/{id}", async (OrderDbContext orderDb, int id) =>
{
    var order = await orderDb.Orders.FirstOrDefaultAsync(o => o.Id == id);
    if (order == null)
    {
        return;
    }

    orderDb.Orders.Remove(order);
    await orderDb.SaveChangesAsync();
})
    .WithName("delete order");

app.Run();

async Task CreateDb(IServiceProvider services, ILogger logger)
{
    using var db = services.CreateScope().ServiceProvider.GetRequiredService<OrderDbContext>();
    await db.Database.MigrateAsync();
}

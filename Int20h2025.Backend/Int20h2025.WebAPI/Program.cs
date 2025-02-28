using Int20h2025.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInt20hServices(builder.Configuration);
builder.Services.AddInt20h2025AzureBlobStorage(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.SeedDatabase();

app.Run();

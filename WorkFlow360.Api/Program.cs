using WorkFlow360.Api.ExceptionHandling;
using WorkFlow360.Application;
using WorkFlow360.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(
    builder.Configuration);

builder.Services.AddExceptionHandler<
    GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "WorkFlow360 API is running");
//app.MapGet(
//    "/test-error",
//    () =>
//    {
//        throw new InvalidOperationException(
//            "This is our test exception.");
//    });

app.Run();
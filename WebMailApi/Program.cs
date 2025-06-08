using Azure.Communication.Email;
using WebMailApi.Dtos;
using WebMailApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();


// 1. Configuration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));


// 2. HTTP Client setup
builder.Services.AddHttpClient("BookingService", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["BookingService:BaseUrl"]);
});
builder.Services.AddHttpClient("EventService", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["EventService:BaseUrl"]);
});


// 3. Registerar your confirmation service
builder.Services.AddTransient<IBookingConfirmationService, BookingConfirmationService>();
builder.Services.AddSwaggerGen();


// Konfigurera HttpClients med bas-URLs
builder.Services.AddHttpClient("BookingService", client =>
{
    client.BaseAddress = new Uri("https://bookingservices-add0avaub6dbbybj.swedencentral-01.azurewebsites.net");
});

builder.Services.AddHttpClient("EventService", client =>
{
    client.BaseAddress = new Uri("https://ventixe-eventservice-app-dba4gpfwd4eebeff.swedencentral-01.azurewebsites.net");
});




var app = builder.Build();
app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());



app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mail Sevice API");
    c.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

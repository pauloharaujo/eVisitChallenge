var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITrackIpAddressService, TrackIpAddressService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/request_handled", (string ip_address, ITrackIpAddressService trackIpAddressService) =>
{
    trackIpAddressService.TrackNewIpAddress(ip_address);
});

app.MapGet("/top100", () =>
{
    return InMemoryCache.Top100IpAddresses;
});

app.MapGet("/clear", (ITrackIpAddressService trackIpAddressService) =>
{
    trackIpAddressService.ClearIpAddressTracking();
});

app.Run("http://localhost:3000");

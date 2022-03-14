var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITrackIpAddressService, TrackIpAddressService>();

var app = builder.Build();

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

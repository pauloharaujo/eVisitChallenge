using Xunit;

namespace eVisit.Challenge.Tests;

public class TrackIpAddressServiceTests
{
    TrackIpAddressService trackIpAddressService = new TrackIpAddressService();

    const int MaxNumberOfTopAddressesToTrack = 100;

    public TrackIpAddressServiceTests()
    {
        // make sure the in memory cache data is empty for each unit test run
        trackIpAddressService.ClearIpAddressTracking();
    }

    [Fact]
    public void ClearIpAddressTracking_ClearIpAddressesAndTop100IpAddresses()
    {
        var numberOfIpAddresses = 50;

        for(var i = 0; i < numberOfIpAddresses; i++)
        {
            trackIpAddressService.TrackNewIpAddress($"127.0.0.{i}");
        }

        Assert.Equal(numberOfIpAddresses, InMemoryCache.IpAddresses.Count);
        Assert.Equal(numberOfIpAddresses, InMemoryCache.Top100IpAddresses.Count);

        trackIpAddressService.ClearIpAddressTracking();
        Assert.Empty(InMemoryCache.IpAddresses);
        Assert.Empty(InMemoryCache.Top100IpAddresses);
    }

    [Fact]
    public void TrackNewIpAddress_TrackCorrectNumberOfRequests()
    {
        var numberOfIpAddresses = 500;

        for(var i = 0; i < numberOfIpAddresses; i++)
        {
            trackIpAddressService.TrackNewIpAddress($"127.0.0.{i}");
        }

        Assert.Equal(numberOfIpAddresses, InMemoryCache.IpAddresses.Count);
        Assert.Equal(MaxNumberOfTopAddressesToTrack, InMemoryCache.Top100IpAddresses.Count);
    }

   [Fact]
    public void TrackNewIpAddress_TrackCorrectTop100IpAddresses()
    {
        var numberOfIpAddresses = 500;

        for(var i = 0; i < numberOfIpAddresses; i++)
        {
            trackIpAddressService.TrackNewIpAddress($"127.0.0.{i}");
        }

        // add extra 5 requests so it should be the first index
        trackIpAddressService.TrackNewIpAddress("127.0.0.1");
        trackIpAddressService.TrackNewIpAddress("127.0.0.1");
        trackIpAddressService.TrackNewIpAddress("127.0.0.1");
        trackIpAddressService.TrackNewIpAddress("127.0.0.1");
        trackIpAddressService.TrackNewIpAddress("127.0.0.1");

        // add extra 3 requests so it should be the second index
        trackIpAddressService.TrackNewIpAddress("127.0.0.15");
        trackIpAddressService.TrackNewIpAddress("127.0.0.15");
        trackIpAddressService.TrackNewIpAddress("127.0.0.15");

        // add extra 2 requests so it should be the third index
        trackIpAddressService.TrackNewIpAddress("127.0.0.12");
        trackIpAddressService.TrackNewIpAddress("127.0.0.12");

        Assert.Equal(MaxNumberOfTopAddressesToTrack, InMemoryCache.Top100IpAddresses.Count);
        Assert.Equal("127.0.0.1", InMemoryCache.Top100IpAddresses[0].IpAddress);
        Assert.Equal(6, InMemoryCache.Top100IpAddresses[0].NumberOfRequests);

        Assert.Equal("127.0.0.15", InMemoryCache.Top100IpAddresses[1].IpAddress);
        Assert.Equal(4, InMemoryCache.Top100IpAddresses[1].NumberOfRequests);

        Assert.Equal("127.0.0.12", InMemoryCache.Top100IpAddresses[2].IpAddress);
        Assert.Equal(3, InMemoryCache.Top100IpAddresses[2].NumberOfRequests);
    }


}
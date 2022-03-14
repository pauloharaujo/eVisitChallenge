public class TrackIpAddressService : ITrackIpAddressService
{
    const int MaxNumberOfTopAddressesToTrack = 100;

    public void TrackNewIpAddress(string ipAddress)
    {
        // get existing entry on top 100 ip address
        if (InMemoryCache.IpAddresses.ContainsKey(ipAddress))
        {
            InMemoryCache.IpAddresses[ipAddress] = InMemoryCache.IpAddresses[ipAddress] + 1;
        }
        else
        {
            InMemoryCache.IpAddresses.Add(ipAddress, 1);
        }
        UpdateTopIpAddresses(ipAddress);
    }
    public void ClearIpAddressTracking()
    {
        InMemoryCache.IpAddresses = new Dictionary<string, int>();
        InMemoryCache.Top100IpAddresses = new List<IpAddressTracker>();
    }


    private void UpdateTopIpAddresses(string ipAddress)
    {
        // check if the ip address is already in the top100 ip addresses
        var ipAddressTracker = new IpAddressTracker(ipAddress, InMemoryCache.IpAddresses[ipAddress]);
        var indexOf = InMemoryCache.Top100IpAddresses.IndexOf(ipAddressTracker);
        if (indexOf >= 0)
        {
            InMemoryCache.Top100IpAddresses[indexOf].NumberOfRequests += 1;
        }
        else
        {
            InMemoryCache.Top100IpAddresses.Add(ipAddressTracker);
        }
        // sort the list by descending order so it is ready to retrieve when needed
        SortTopIpAddressDescending(InMemoryCache.Top100IpAddresses);

        // make sure the list only keep the top ip addresses
        if(InMemoryCache.Top100IpAddresses.Count > MaxNumberOfTopAddressesToTrack)
        {
            InMemoryCache.Top100IpAddresses.RemoveAt(MaxNumberOfTopAddressesToTrack);
        }
    }

    private void SortTopIpAddressDescending(List<IpAddressTracker> ipAddressTrackerList)
    {
        int count = ipAddressTrackerList.Count;
        for (int i = 0; i < count - 1; i++)
        {
            for (int j = 0; j < count - i - 1; j++)
            {
                if (ipAddressTrackerList[j].NumberOfRequests < ipAddressTrackerList[j + 1].NumberOfRequests)
                {
                    IpAddressTracker temp = ipAddressTrackerList[j];
                    ipAddressTrackerList[j] = ipAddressTrackerList[j + 1];
                    ipAddressTrackerList[j + 1] = temp;
                }
            }
        }
    }
}
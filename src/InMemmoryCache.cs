// Using a static class to save the data in memory
public static class InMemoryCache
{
    // Using a Dictionary, which is very similar to a hashtable but I can define the 
    // types of the key and the value, so it is faster because it avoids the boxing and unboxing
    public static Dictionary<string, int> IpAddresses = new Dictionary<string, int>();
    //public static Dictionary<string, int> Top100IpAddresses = new Dictionary<string, int>();
    
    // Using a list to save the top 100 ip addresses so I can maintain it descending ordered by
    // the number of requests per ip address and the top100 endpoint is requested I can just 
    // return right it way
    public static List<IpAddressTracker> Top100IpAddresses = new List<IpAddressTracker>();
}


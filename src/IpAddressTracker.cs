// class to store the top 100 ip address
public class IpAddressTracker : IEquatable<IpAddressTracker>
{
    public string IpAddress { get; set; }
    public int NumberOfRequests { get; set; }

    public IpAddressTracker(string ipAddress, int numberOfRequests)
    {
        this.IpAddress = ipAddress;
        this.NumberOfRequests = numberOfRequests;
    }

    public bool Equals(IpAddressTracker? other) => other != null && IpAddress == other.IpAddress;

    // override the Equals function so I can use the List.IndexOf to find the index of the element in the list
    public override bool Equals(object obj) => Equals(obj as IpAddressTracker);

    public override int GetHashCode()
    {
        return HashCode.Combine(IpAddress, NumberOfRequests);
    }
}



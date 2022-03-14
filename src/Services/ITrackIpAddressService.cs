public interface ITrackIpAddressService
{
    void TrackNewIpAddress(string ipAddress);
    void ClearIpAddressTracking();
}

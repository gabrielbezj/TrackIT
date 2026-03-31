namespace TrackIT.Models;

public enum AssetType
{
    Laptop,
    Desktop,
    Server,
    NetworkDevice,
    MobileDevice,
    Peripheral
}

public enum AssetStatus
{
    InStock,
    Assigned,
    InMaintenance,
    Retired
}

public enum LicenseType
{
    Subscription,
    Perpetual,
    Trial
}
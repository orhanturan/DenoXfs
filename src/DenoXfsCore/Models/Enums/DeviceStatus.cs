namespace DenoXfs.Core.Models.Enums;

/// <summary>
/// XFS cihaz durumu değerleri.
/// WFS_STAT_DEV* sabitlerinin enum karşılığı.
/// </summary>
public enum DeviceStatus : ushort
{
    /// <summary>
    /// Cihaz çevrimiçi ve kullanıma hazır.
    /// </summary>
    Online = 0,

    /// <summary>
    /// Cihaz çevrimdışı.
    /// </summary>
    Offline = 1,

    /// <summary>
    /// Cihaz kapalı.
    /// </summary>
    PowerOff = 2,

    /// <summary>
    /// Cihaz bulunamadı.
    /// </summary>
    NoDevice = 3,

    /// <summary>
    /// Donanım hatası.
    /// </summary>
    HardwareError = 4,

    /// <summary>
    /// Kullanıcı hatası.
    /// </summary>
    UserError = 5,

    /// <summary>
    /// Cihaz meşgul.
    /// </summary>
    Busy = 6,

    /// <summary>
    /// Dolandırıcılık girişimi tespit edildi.
    /// </summary>
    FraudAttempt = 7,

    /// <summary>
    /// Potansiyel dolandırıcılık.
    /// </summary>
    PotentialFraud = 8
}








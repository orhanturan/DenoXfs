namespace DenoXfs.Idc.Models.Enums;

/// <summary>
/// IDC cihaz tipi değerleri.
/// WFS_IDC_TYPE* sabitlerinin enum karşılığı.
/// </summary>
[Flags]
public enum IdcDeviceType : ushort
{
    /// <summary>
    /// Motorlu kart okuyucu.
    /// </summary>
    Motor = 0x0001,

    /// <summary>
    /// Swipe (kaydırmalı) kart okuyucu.
    /// </summary>
    Swipe = 0x0002,

    /// <summary>
    /// DIP (takmalı) kart okuyucu.
    /// </summary>
    Dip = 0x0003,

    /// <summary>
    /// Temassız kart okuyucu.
    /// </summary>
    Contactless = 0x0004,

    /// <summary>
    /// Kilitli DIP kart okuyucu.
    /// </summary>
    LatchedDip = 0x0005,

    /// <summary>
    /// Kalıcı kart okuyucu.
    /// </summary>
    Permanent = 0x0006,

    /// <summary>
    /// Akıllı temassız kart okuyucu.
    /// </summary>
    IntelligentContactless = 0x0007
}








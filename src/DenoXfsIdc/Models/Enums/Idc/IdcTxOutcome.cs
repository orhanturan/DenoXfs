namespace DenoXfs.Core.Models.Enums.Idc;

public enum IdcTxOutcome : ushort
{
    Multiplecards = 0,
    Endapplication = 7,
    AppRove = 1,
    Decline = 2,
    Tryagain = 5,
    Tryanotherinterface = 6,
    OnlineRequest = 3,
    OnlineRequestcompletionrequired = 4,
    Confirmationrequired = 8
}

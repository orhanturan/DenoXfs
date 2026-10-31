namespace DenoXfs.Idc.Shared;

public enum IdcTxOutcomeEmvclessTxDataOutput : ushort
{
    MultipleCards = 0,
    Approve = 1,
    Decline = 2,
    OnlineRequest = 3,
    OnlineRequestCompletionRequired = 4,
    TryAgain = 5,
    TryAnotherInterface = 6,
    EndApplication = 7,
    ConfirmationRequired = 8
}

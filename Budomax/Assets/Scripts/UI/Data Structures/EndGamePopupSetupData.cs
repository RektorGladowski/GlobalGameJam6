using System;

public struct EndGamePopupSetupData
{
    public EndGamePopupData data;
    public Action<EndGamePopupResult> callback;

    public EndGamePopupSetupData(EndGamePopupData data, Action<EndGamePopupResult> callback)
    {
        this.data = data;
        this.callback = callback;
    }
}
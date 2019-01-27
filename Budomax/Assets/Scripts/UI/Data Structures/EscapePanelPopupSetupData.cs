using System;

public struct EscapePanelPopupSetupData
{
    public Action<EscapePanelSelection> callback;

    public EscapePanelPopupSetupData(Action<EscapePanelSelection> callback)
    {
        this.callback = callback;
    }
}

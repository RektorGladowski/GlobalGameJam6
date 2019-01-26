using System;

public struct RoomSelectionPopupSetupData
{
    public Action<RoomTypeSelection> callback;
    
    public RoomSelectionPopupSetupData(Action<RoomTypeSelection> callback)
    {
        this.callback = callback;
    }
}

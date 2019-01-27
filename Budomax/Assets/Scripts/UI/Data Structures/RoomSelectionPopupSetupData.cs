using System;

public struct RoomSelectionPopupSetupData
{
    public Action<RoomTypeSelection> callback;
    public RoomSelectionButtonLockPreset buttonLocks;
    
    public RoomSelectionPopupSetupData(Action<RoomTypeSelection> callback)
    {
        this.callback = callback;
        buttonLocks = RoomSelectionButtonLockPreset.AllUnlocked;
    }

    public RoomSelectionPopupSetupData(Action<RoomTypeSelection> callback, RoomSelectionButtonLockPreset preset)
    {
        this.callback = callback;
        buttonLocks = preset;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    public Action<RoomTypeSelection> OnRoomTypeSelected;
    public Action<EndGamePopupResult> OnEndGameOptionSelected;


    void Awake () => instance = this;

    #region Room Creation Popup
    public void ShowRoomCreationPopup ()
    {
        GetComponentInChildren<IPopup<RoomSelectionPopupSetupData>>()?.OpenPopup(new RoomSelectionPopupSetupData(RoomCreationPopupExit));
    }

    void RoomCreationPopupExit (RoomTypeSelection rts)
    {
        OnRoomTypeSelected?.Invoke(rts);
    }
    #endregion

    #region End Game Popup
    public void ShowEndGamePopup (EndGamePopupData data)
    {
        GetComponentInChildren<IPopup<EndGamePopupSetupData>>()?.OpenPopup(new EndGamePopupSetupData(data, EndGamePopupExit));
    }

    void EndGamePopupExit (EndGamePopupResult egpr)
    {
        OnEndGameOptionSelected?.Invoke(egpr);
    }
    #endregion
}

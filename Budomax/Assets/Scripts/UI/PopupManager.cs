using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    public Action<RoomTypeSelection> OnRoomTypeSelected;
    public Action<EndGamePopupResult> OnEndGameOptionSelected;


    void Awake() => instance = this;

    #region Room Creation Popup
    public void ShowRoomCreationPopup()
    {
        GetComponentInChildren<IPopup<RoomSelectionPopupSetupData>>()?.OpenPopup(new RoomSelectionPopupSetupData(RoomCreationPopupExit));
    }

    public void ShowRoomCreationPopup(Action<RoomTypeSelection> callback)
    {
        GetComponentInChildren<IPopup<RoomSelectionPopupSetupData>>()?.OpenPopup(new RoomSelectionPopupSetupData(callback));
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

    #region Tutorial popups
    public void ShowTutorialMessage (string msg)
    {
        GetComponentInChildren<IPopup<QueueableMessage>>()?.OpenPopup(new QueueableMessage(msg));
    }

    public void HidePreviousTutorialMessage ()
    {
        GetComponentInChildren<IPopup<QueueableMessage>>()?.ClosePopupManually();
    }
    #endregion

    #region Escape Panel Popup

    #endregion
}

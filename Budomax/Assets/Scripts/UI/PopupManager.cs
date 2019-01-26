using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    public Action<RoomTypeSelection> OnRoomTypeSelected;
    public Action<EndGamePopupResult> OnEndGameOptionSelected;

    TutorialPopup tpManager;

    void Awake()
    {
        instance = this;
        tpManager = GetComponentInChildren<TutorialPopup>();
        tpManager?.SetupTutorialPopups();
    }

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
        tpManager?.ShowMessage(msg);
    }

    public void HidePreviousTutorialMessage ()
    {
        tpManager?.HidePreviousMessage();
    }
    #endregion
}

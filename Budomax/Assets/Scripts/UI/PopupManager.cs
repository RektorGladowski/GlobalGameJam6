using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    public Action<RoomTypeSelection> OnRoomTypeSelected;
    public Action<EndGamePopupResult> OnEndGameOptionSelected;
    public Action<EscapePanelSelection> OnEscapePanelOptionSelected;

    public bool EscapePanelActive { get; private set; } = false;
    public bool EndGamePopupActive { get; private set; } = false;
    public bool RoomCreationPopupActive { get; private set; } = false;


    void Awake() => instance = this;

    #region Room Creation Popup
    public void ShowRoomCreationPopup()
    {
        RoomCreationPopupActive = true;
        GetComponentInChildren<IPopup<RoomSelectionPopupSetupData>>()?.OpenPopup(new RoomSelectionPopupSetupData(RoomCreationPopupExit));
    }

    public void ShowRoomCreationPopup(RoomSelectionButtonLockPreset preset)
    {
        RoomCreationPopupActive = true;
        GetComponentInChildren<IPopup<RoomSelectionPopupSetupData>>()?.OpenPopup(new RoomSelectionPopupSetupData(RoomCreationPopupExit, preset));
    }

    public void ShowRoomCreationPopup(Action<RoomTypeSelection> callback)
    {
        RoomCreationPopupActive = true;
        GetComponentInChildren<IPopup<RoomSelectionPopupSetupData>>()?.OpenPopup(new RoomSelectionPopupSetupData(callback));
    }

    public void ShowRoomCreationPopup(Action<RoomTypeSelection> callback, RoomSelectionButtonLockPreset preset)
    {
        RoomCreationPopupActive = true;
        GetComponentInChildren<IPopup<RoomSelectionPopupSetupData>>()?.OpenPopup(new RoomSelectionPopupSetupData(callback, preset));
    }

    void RoomCreationPopupExit (RoomTypeSelection rts)
    {
        RoomCreationPopupActive = false;
        OnRoomTypeSelected?.Invoke(rts);
    }
    #endregion

    #region End Game Popup
    public void ShowEndGamePopup (EndGamePopupData data)
    {
        EndGamePopupActive = true;
        GetComponentInChildren<IPopup<EndGamePopupSetupData>>()?.OpenPopup(new EndGamePopupSetupData(data, EndGamePopupExit));
    }

    void EndGamePopupExit (EndGamePopupResult egpr)
    {
        EndGamePopupActive = false;
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
    public void ShowEscapePanel ()
    {
        EscapePanelActive = true;
        GetComponentInChildren<IPopup<EscapePanelPopupSetupData>>()?.OpenPopup(new EscapePanelPopupSetupData(EscapePanelButtonExit));
    }

    public void CloseEscapePanel ()
    {
        EscapePanelActive = false;
        GetComponentInChildren<IPopup<EscapePanelPopupSetupData>>()?.ClosePopupManually();
    }

    void EscapePanelButtonExit (EscapePanelSelection eps)
    {
        EscapePanelActive = false;
        OnEscapePanelOptionSelected?.Invoke(eps);
    }
    #endregion
}

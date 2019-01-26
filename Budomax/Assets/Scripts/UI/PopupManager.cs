using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    public Action<RoomTypeSelection> OnRoomTypeSelected;


    void Awake() => instance = this;


    public void ShowRoomCreationPopup()
    {
        GetComponentInChildren<IPopup<RoomTypeSelection>>().OpenPopup(RoomCreationPopupExit);
    }

    public void ShowRoomCreationPopup(Action<RoomTypeSelection> callback)
    {
        GetComponentInChildren<IPopup<RoomTypeSelection>>().OpenPopup(callback);
    }

    void RoomCreationPopupExit(RoomTypeSelection rts)
    {
        OnRoomTypeSelected?.Invoke(rts);
    }
}

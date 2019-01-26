using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomCreationPopup : MonoBehaviour, IPopup<RoomTypeSelection>
{
    public GameObject PopupPanel;
    public Button kitchenButton, scavButton, barracksButton;

    Action<RoomTypeSelection> managerCallback;

    public void OpenPopup(Action<RoomTypeSelection> callback)
    {
        managerCallback = callback;
        SetPanelInteractability(true);
    }

    public void OnKitchenButtonPressed ()
    {
        SetPanelInteractability(false);
        managerCallback(RoomTypeSelection.Kitchen);
    }

    public void OnBarracksButtonPressed ()
    {
        SetPanelInteractability(false);
        managerCallback(RoomTypeSelection.Barracks);
    }

    public void OnScavengerButtonPressed ()
    {
        SetPanelInteractability(false);
        managerCallback(RoomTypeSelection.ScavengerRoom);
    }
   
    void SetPanelInteractability(bool interactable)
    {
        kitchenButton.interactable = scavButton.interactable = barracksButton.interactable = interactable;
        PopupPanel.SetActive(interactable);
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectionPopup : MonoBehaviour, IPopup<RoomSelectionPopupSetupData>
{
    public GameObject PopupPanel;
    public Button kitchenButton, scavButton, barracksButton;

    Action<RoomTypeSelection> managerCallback;

    public void OpenPopup(RoomSelectionPopupSetupData data)
    {
        managerCallback = data.callback;
        SetPanelInteractability(true);

        switch(data.buttonLocks)
        {
            case RoomSelectionButtonLockPreset.KitchenAsOnlyOption:
                barracksButton.interactable = scavButton.interactable = false;
                break;
            case RoomSelectionButtonLockPreset.BarracksAsOnlyOption:
                kitchenButton.interactable = scavButton.interactable = false;
                break;
            case RoomSelectionButtonLockPreset.ScavengerRoomAsOnlyOption:
                kitchenButton.interactable = barracksButton.interactable = false;
                break;
        }
    }

    public void ClosePopupManually()
    {
        Debug.LogWarning("Closing room selection popup manually is not implemented");
    }



    public void OnKitchenButtonPressed ()
    {
        SetPanelInteractability(false);
        managerCallback?.Invoke(RoomTypeSelection.Kitchen);
    }

    public void OnBarracksButtonPressed ()
    {
        SetPanelInteractability(false);
        managerCallback?.Invoke(RoomTypeSelection.Barracks);
    }

    public void OnScavengerButtonPressed ()
    {
        SetPanelInteractability(false);
        managerCallback?.Invoke(RoomTypeSelection.ScavengerRoom);
    }
   
    void SetPanelInteractability(bool interactable)
    {
        kitchenButton.interactable = scavButton.interactable = barracksButton.interactable = interactable;
        PopupPanel.SetActive(interactable);
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class EscapePanelPopup : MonoBehaviour, IPopup<EscapePanelPopupSetupData>
{
    public GameObject PopupPanel;
    public Button continueButton, restartButton, quitGameButton;

    Action<EscapePanelSelection> managerCallback;

    public void OpenPopup(EscapePanelPopupSetupData setupData)
    {
        managerCallback = setupData.callback;
        SetPanelInteractability(true);
    }

    public void ClosePopupManually()
    {
        managerCallback = null;
        SetPanelInteractability(false);
    }



    public void OnContinueButtonPressed()
    {
        SetPanelInteractability(false);
        managerCallback?.Invoke(EscapePanelSelection.Continue);
    }

    public void OnRestartButtonPressed()
    {
        SetPanelInteractability(false);
        managerCallback?.Invoke(EscapePanelSelection.Restart);
    }

    public void OnQuitGameButtonPressed()
    {
        SetPanelInteractability(false);
        managerCallback?.Invoke(EscapePanelSelection.QuitGame);
    }

    void SetPanelInteractability(bool interactable)
    {
        continueButton.interactable = restartButton.interactable = quitGameButton.interactable = interactable;
        PopupPanel.SetActive(interactable);
    }
}

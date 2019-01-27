using System;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePopup : MonoBehaviour, IPopup<EndGamePopupSetupData>
{
    public GameObject PopupPanel;
    public GameObject WinInfo, FailInfo;
    public Button restartButton;
    public Text scoreText;

    Action<EndGamePopupResult> managerCallback;


    public void OpenPopup(EndGamePopupSetupData setupData)
    {
        managerCallback = setupData.callback;
        SetPanelInteractability(true);

        if (setupData.data.hasPlayerWon) FailInfo.SetActive(false);
        else WinInfo.SetActive(false);

        scoreText.text = "Total Score: " + setupData.data.totalScore.ToString();
    }

    public void ClosePopupManually()
    {
        Debug.LogWarning("Closing end game popup manually is not implemented");
    }



    public void OnRestartButtonPressed ()
    {
        SetPanelInteractability(false);
        managerCallback(EndGamePopupResult.RestartGame);
    }

    void SetPanelInteractability(bool interactable)
    {
        restartButton.interactable = interactable;
        PopupPanel.SetActive(interactable);
        WinInfo.SetActive(interactable);
        FailInfo.SetActive(interactable);
    }
}

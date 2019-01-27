using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    delegate void TimerUpdater();
    TimerUpdater UpdateTimer = null;

    public static TutorialManager instance;
    public static Action EnemySpawnerReadyToGo;

    public List<TutorialStageData> tutorialStages = new List<TutorialStageData>();
    public TutorialPopup popupScript;

    TutorialStage currentTutorialStage;
    float timer = 0f;

    TutorialCheckState state = TutorialCheckState.Checking;


    #region Setting up
    public bool IsRunning() => state == TutorialCheckState.Running;

    void Awake()
    {
        instance = this;
        CheckIfTutorialIsNecessary();
    }

    void CheckIfTutorialIsNecessary()
    {
        if (PlayerPrefs.HasKey("tutorialInfo"))
        {
            if (PlayerPrefs.GetInt("tutorialInfo") == 0)
            {
                state = TutorialCheckState.Running;
            }
            else
            {
                state = TutorialCheckState.NotRunning;
            }
        }
        else
        {
            PlayerPrefs.SetInt("tutorialInfo", 0);
            PlayerPrefs.Save();

            state = TutorialCheckState.Running;
        }
    }

    void Start ()
    {
        if (state == TutorialCheckState.Running)
        {
            SetupPopupScript();
            StartTutorial();
        }       
    }

    void SetupPopupScript ()
    {
        List<string> messages = new List<string>();

        for (int id = 0; id < tutorialStages.Count; id++)
            messages.Add(tutorialStages[id].stageMessage);

        popupScript?.SetupPopups(messages);
    }

    void StartTutorial ()
    {
        currentTutorialStage = TutorialStage.DragWallToBuild;
        popupScript.ShowMessage((int)currentTutorialStage);

        timer = 0f;
        if (tutorialStages[(int)currentTutorialStage].stageType == TutorialStageType.Timed) UpdateTimer = Timer;
        else UpdateTimer = null;
    }
    #endregion


    #region Triggers
    public void OnWallPlaced ()
    {
        if (currentTutorialStage <= TutorialStage.DragWallToBuild)
            UpdateTutorialStage(TutorialStage.DragWallToBuild);
    }
    
    public void OnRoomCreated ()
    {
        if (currentTutorialStage <= TutorialStage.CloseTheRoomToFinishIt)
            UpdateTutorialStage(TutorialStage.CloseTheRoomToFinishIt);
    }

    public void OnScavengerRoomCreated ()
    {
        if (currentTutorialStage <= TutorialStage.BuildScavengerRoom)
            UpdateTutorialStage(TutorialStage.BuildScavengerRoom);
    }

    public void OnKitchenRoomCreated ()
    {
        if (currentTutorialStage <= TutorialStage.BuildKitchen)
            UpdateTutorialStage(TutorialStage.BuildKitchen);
    }
    
    public void OnFeederPlaced ()
    {
        if (currentTutorialStage <= TutorialStage.BuildFeeder)
            UpdateTutorialStage(TutorialStage.BuildFeeder);
    }

    public void OnBarracksCreated ()
    {
        if (currentTutorialStage <= TutorialStage.BuildBarracks)
            UpdateTutorialStage(TutorialStage.BuildBarracks);
    }
    #endregion



    #region Updating tutorial
    void UpdateTutorialStage(TutorialStage endedStage)
    {
        if (state == TutorialCheckState.Running)
        {
            if (endedStage == TutorialStage.UnitsInfo) EnemySpawnerReadyToGo?.Invoke();

            if (endedStage != TutorialStage.DefendHome)
            {
                currentTutorialStage = (TutorialStage)((int)endedStage + 1);
                popupScript?.ShowMessage((int)currentTutorialStage);

                timer = 0f;
                if (tutorialStages[(int)currentTutorialStage].stageType == TutorialStageType.Timed) UpdateTimer = Timer;
                else UpdateTimer = null;
            }
            else
            {
                PlayerPrefs.SetInt("tutorialInfo", 1);
                PlayerPrefs.Save();
                popupScript.ClosePopupManually();
            }
        }       
    }

    void Timer ()
    {
        timer += Time.deltaTime;
        if (timer >= tutorialStages[(int)currentTutorialStage].timeToShow)
        {
            UpdateTimer = null;
            UpdateTutorialStage(currentTutorialStage);
        }
    }

    void LateUpdate()
    {
        UpdateTimer?.Invoke();
    }
    #endregion
}

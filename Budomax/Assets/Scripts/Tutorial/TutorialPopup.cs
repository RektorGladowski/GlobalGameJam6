using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    // Constants
    const float Popup_Max_Teleport_Gap = 0.35f;
    const float Popup_Max_Movement_Speed = 36000f;
    const float Popup_Transition_Time = 0.1f;

    delegate void PopupMover();
    PopupMover MovePopup = null;

    // Popup customization
    public Sprite[] availablePopupSprites;
    public Color popupTextColor;

    // Animation related components
    public RectTransform popupRT;
    public Image popupImage;
    public Text popupText;

    // Showing messages
    List<string> popupMessages = new List<string>();
    int currentMessageID = 0;
    bool waitingToShowNextMessage = false; 

    // Animation variables
    TutorialPopupState currentPopupState = TutorialPopupState.Hidden;
    Vector2 relShowV, relHideV;
    Vector2 hideTarget;
    float maxGap;
    float maxSpeed;
    float transTime;

    

    #region Setting up
    public void SetupPopups (List<string> messages)
    {
        // Assign messages
        popupMessages = messages;

        // Change position of the popup and deactivate its object
        popupRT.anchoredPosition = new Vector2(-Screen.width, 0f);
        popupRT.gameObject.SetActive(false);

        // Set animation variables
        maxGap = Popup_Max_Teleport_Gap;
        maxSpeed = Popup_Max_Movement_Speed;
        transTime = Popup_Transition_Time;
     
        relHideV = relShowV = Vector2.zero;
        hideTarget = new Vector2(-Screen.width, 0f);
    }
    #endregion

    #region Animation delegates  
    void ShowPopup ()
    {
        if (Mathf.Abs(popupRT.anchoredPosition.x) > maxGap)
        {
            popupRT.anchoredPosition = Vector2.SmoothDamp(popupRT.anchoredPosition, Vector2.zero, ref relShowV, transTime, maxSpeed, Time.deltaTime);
        }
        else
        {
            popupRT.anchoredPosition = Vector2.zero;
            FinishedShowing();
        }
    }

    void HidePopup ()
    {
        if (Mathf.Abs(popupRT.anchoredPosition.x - hideTarget.x) > maxGap)
        {
            popupRT.anchoredPosition = Vector2.SmoothDamp(popupRT.anchoredPosition, hideTarget, ref relHideV, transTime, maxSpeed, Time.deltaTime);
        }
        else
        {
            popupRT.anchoredPosition = hideTarget;
            FinishedHiding();
        }
    }
    #endregion

    #region Animation events
    void StartShowing ()
    {
        waitingToShowNextMessage = false;
        currentPopupState = TutorialPopupState.Showing;

        popupText.text = popupMessages[currentMessageID];
        popupImage.sprite = availablePopupSprites[Random.Range(0, availablePopupSprites.Length)];
        popupRT.gameObject.SetActive(true);
        MovePopup = ShowPopup;
    }

    void StartHiding()
    {
        currentPopupState = TutorialPopupState.Hiding;
        MovePopup = HidePopup;
    }

    void FinishedShowing()
    {
        currentPopupState = TutorialPopupState.Shown;
        MovePopup = null;
    }

    void FinishedHiding()
    {
        currentPopupState = TutorialPopupState.Hidden;
        MovePopup = null;
        popupRT.gameObject.SetActive(false);

        if (waitingToShowNextMessage) StartShowing();  
    }
    #endregion

    #region Managing Messages
    public void ShowMessage (int messageID)
    {
        currentMessageID = messageID;
       
        if (currentPopupState != TutorialPopupState.Hidden)
        {
            waitingToShowNextMessage = true;
            StartHiding();
        }
        else
        {
            StartShowing();
        }
    }

    public void ClosePopupManually ()
    {
        waitingToShowNextMessage = false;
        StartHiding();
    }
    #endregion

    void Update() => MovePopup?.Invoke();
}

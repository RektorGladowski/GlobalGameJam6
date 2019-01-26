using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    // Constants
    const float Popup_Max_Teleport_Gap = 0.35f;
    const float Popup_Max_Movement_Speed = 12000f;
    const float Popup_Transition_Time = 0.3f;

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
    Queue<QueueableMessage> messagesToDisplay = new Queue<QueueableMessage>();
    QueueableMessage currentMessage;

    // Animation variables
    Vector2 relShowV, relHideV;
    Vector2 hideTarget;
    float maxGap;
    float maxSpeed;
    float transTime;


    #region Setting up
    public void SetupTutorialPopups ()
    {
        // Change position of the popup and deactivate its object
        popupRT.anchoredPosition = new Vector2(-Screen.width, 0f);
        popupRT.gameObject.SetActive(false);

        // Set animation variables
        maxGap = Popup_Max_Teleport_Gap;
        maxSpeed = Popup_Max_Movement_Speed;
        transTime = Popup_Transition_Time;
     
        relHideV = relShowV = Vector2.zero;
        hideTarget = new Vector2(-Screen.width, 0f);
        currentMessage = null;
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
        popupRT.gameObject.SetActive(true);
        MovePopup = ShowPopup;
    }

    void StartHiding()
    {
        MovePopup = HidePopup;
    }

    void FinishedShowing()
    {
        MovePopup = null;
    }

    void FinishedHiding()
    {
        MovePopup = null;
        popupRT.gameObject.SetActive(false);

        DequeuePreviousMessage();
        if (!IsTheQueueEmpty()) StartShowingQueueableMessage();      
    }
    #endregion

    #region Managing Queue
    public void ShowMessage (string message)
    {
        QueueableMessage qMessage = new QueueableMessage(message);

        if (IsTheQueueEmpty())
        {
            messagesToDisplay.Enqueue(qMessage);
            StartShowingQueueableMessage();
        }
        else
        {
            messagesToDisplay.Enqueue(qMessage);
        }
    }

    public void HidePreviousMessage ()
    {
        // Hide message
        StartHiding();
    }

    void DequeuePreviousMessage()
    {
        // Just dequeue the message
        messagesToDisplay.Dequeue();
        currentMessage = null;
    }

    void StartShowingQueueableMessage()
    {
        // Get message from queue
        currentMessage = messagesToDisplay.Peek();
        popupText.text = currentMessage.messageText;
        popupImage.sprite = availablePopupSprites[Random.Range(0, availablePopupSprites.Length)];

        StartShowing();
    }
    #endregion

    void Update() => MovePopup?.Invoke();
    bool IsTheQueueEmpty() => (messagesToDisplay.Count == 0);
}

public class QueueableMessage
{
    public string messageText;

    public QueueableMessage(string text)
    { 
        messageText = text;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StoryTeller : MonoBehaviour
{
    public List<TimedPopup> popupSprites = new List<TimedPopup>();
    public GameObject popup;
    public Image popupImage;

    MainMenu menu;
    bool showingStory = false;
    int currentStoryId = 0;
    float timer = 0f;


    // Called by Main menu
    public void SetupPopups ()
    {
        menu = GetComponent<MainMenu>();
        popupImage.gameObject.SetActive(false);
        popup.SetActive(false);
    }

    public void ShowStory ()
    {
        // Set correct image and display it
        currentStoryId = 0;
        timer = 0f;
        popupImage.sprite = popupSprites[currentStoryId].sprite;
        popup.SetActive(true);
        popupImage.gameObject.SetActive(true);

        showingStory = true;
    }

    void Advance()
    {
        showingStory = false;
        currentStoryId++;

        if (currentStoryId < popupSprites.Count) // Advance normally
        {
            popupImage.sprite = popupSprites[currentStoryId].sprite;

            timer = 0f;
            showingStory = true;
        }
        else // Finish the story
        {
            showingStory = false;
            timer = 0f;

            popupImage.gameObject.SetActive(false);
            popup.SetActive(false);
            menu.FinishedShowingStory();
        }
    }

    void Update()
    {
        if (showingStory)
        {
            timer += Time.deltaTime;
            if (timer >= popupSprites[currentStoryId].time) Advance();            
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    StoryTeller storyTeller;
    bool panelActive = false;


    void Awake()
    {
        storyTeller = GetComponent<StoryTeller>();
        storyTeller.SetupPopups();

        panelActive = true;
    }

    public void OnPlayButtonPress()
    {
        if (panelActive)
        {           
            panelActive = false;
            StartGame();
        }
    }

    public void OnStoryTimeButtonPress()
    {
        if (panelActive)
            StartShowingStory();
    }

    public void OnQuitGameButtonPress()
    {
        if (panelActive)
            Application.Quit();
    }


    public void FinishedShowingStory()
    {
        panelActive = true;
    }

    void StartShowingStory()
    {
        panelActive = false;
        storyTeller.ShowStory();
    }


    #region Scene loading
    void StartGame ()
    {
        StartCoroutine(WaitForLoadAndLoadLevel("Main"));
    }

    IEnumerator WaitForLoadAndLoadLevel(string sceneName)
    {
        yield return new WaitForSeconds(0f);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    #endregion
}
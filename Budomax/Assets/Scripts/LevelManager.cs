using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Escape panel Event listener
    void EscapePanelOptionSelected(EscapePanelSelection result)
    {
        switch (result)
        {
            case EscapePanelSelection.Restart:
                ReloadLevel();
                break;

            case EscapePanelSelection.QuitGame:
                Application.Quit();
                break;
        }
    }
    #endregion

    #region End game Event listener and listener actions
    void EndGameOptionSelected(EndGamePopupResult result)
    {
        switch (result)
        {
            case EndGamePopupResult.RestartGame:
                ReloadLevel();
                break;
        }
    }

    void ReloadLevel()
    {
        StartCoroutine(WaitForLoadAndLoadLevel("Reset"));
    }
    #endregion
    
    #region Scene loading
    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetComponentInChildren<PopupManager>().OnEndGameOptionSelected = EndGameOptionSelected;
        GetComponentInChildren<PopupManager>().OnEscapePanelOptionSelected = EscapePanelOptionSelected;
    }

    IEnumerator WaitForLoadAndLoadLevel(string sceneName)
    {
        yield return new WaitForSeconds(0f);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    #endregion
}

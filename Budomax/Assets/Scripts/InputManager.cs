using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    bool escapePanelOpened = false;

    void LateUpdate()
    {
        CheckForEscapePanelInput();
    }

    void CheckForEscapePanelInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PopupManager.instance.EscapePanelActive)
            {
                PopupManager.instance.CloseEscapePanel();
                escapePanelOpened = false;
            }
            else
            {
                PopupManager.instance.ShowEscapePanel();
                escapePanelOpened = true;
            }
        }
    }

    public static bool GetRotateLeftAction() => Input.GetKey(KeyCode.Q);
    public static bool GetRotateRightAction() => Input.GetKey(KeyCode.E);
}

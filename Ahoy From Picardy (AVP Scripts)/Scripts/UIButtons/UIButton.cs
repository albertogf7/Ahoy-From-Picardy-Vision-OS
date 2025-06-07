using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class UIButton : MonoBehaviour
{
    [SerializeField]
    private int _buttonID;
    [SerializeField]
    private UIHandler _uiScript;

    public void UIButtonPress()
    {
        switch (_buttonID)
        {
            case 0:
                //Play
                _uiScript.PressPlay();
                break;
            case 1:
                //Pause
                _uiScript.PressPause();
                break;
            case 2:
                //Exit Tray
                _uiScript.PressExit();
                break;
            case 3:
                //Cancel Exit
                _uiScript.PressCancel();
                break;
            case 4:
                //Confirm Exit
                _uiScript.PressConfirm();
                break;
            case 5:
                // Quit Game
                _uiScript.PressQuit();
                break;
                //5 added by Dan
        }
    }

}

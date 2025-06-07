using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //[SerializeField]
    //private PlayableDirector _exitButton;

    ////public void RunExitAnim()
    ////{
    ////    _exitButton.Play();
    ////}
    public void QuitGameOnSignal()
    {
        Debug.Log("App should quit now");
        Application.Quit(); 
    }
}

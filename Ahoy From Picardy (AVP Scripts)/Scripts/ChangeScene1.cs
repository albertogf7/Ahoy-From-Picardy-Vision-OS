using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Buffers;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;
    [SerializeField]
    private PlayableDirector _startButton;

    public void LoadMainScene()
    {
        _startButton.Play();
    }

    public void ChangeNow()
    {
        StartCoroutine(ExitToSceneOne());
    }
    private IEnumerator ExitToSceneOne()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        yield return new WaitForSeconds(1); // Wait for a moment to ensure cleanup
        SceneManager.LoadScene(_sceneName);
    }
}

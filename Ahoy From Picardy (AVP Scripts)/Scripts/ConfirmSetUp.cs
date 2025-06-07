using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ConfirmSetUp : MonoBehaviour
{
    //Initial Set Up
    [SerializeField]
    private GameObject _pauseButton;
    [SerializeField]
    private bool _firstSetUp = false;
    [SerializeField]
    private Collider _circleAdjuster;

    [SerializeField]
    private AudioSource _setUpSound;

    [SerializeField]
    private PlayableDirector _mainTimeline;

    // Start is called before the first frame update
    void Start()
    {
        _pauseButton.gameObject.SetActive(false);
        
    }

    public void FirstSetUpDone()
    {
        if (!_firstSetUp)
        {
            
            _pauseButton.gameObject.SetActive(true);
            _circleAdjuster.enabled = false;
            _mainTimeline.Play();
            _firstSetUp = true;
            if (_firstSetUp)
            {
                StartCoroutine(FadeOutVolume(2f));
            }
        }
    }

    IEnumerator FadeOutVolume(float duration)
    {
        float startVolume = _setUpSound.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            _setUpSound.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }
        _setUpSound.volume = 0f;
        _setUpSound.Stop();
        this.gameObject.SetActive(false);
    }
}

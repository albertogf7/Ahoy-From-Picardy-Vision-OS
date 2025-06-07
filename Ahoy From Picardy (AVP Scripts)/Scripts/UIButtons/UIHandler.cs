using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    // Bools
    [SerializeField]
    private bool _isPaused = false;
    [SerializeField]
    private bool _isExiting = false;
    [SerializeField]
    private bool _hasConfirmed = false;

    // Playable Timelines
    [SerializeField]
    private PlayableDirector _playButtonTimeline;
    [SerializeField]
    private PlayableDirector _homeTrayTimeline;
    [SerializeField]
    private int _currentTimelineIndex = 0; // Index of the currently active timeline
    [SerializeField]
    private PlayableDirector[] _timelineDirectors; // Array to hold all PlayableDirectors

    // Colliders
    [SerializeField]
    private Collider _playCollider;
    [SerializeField]
    private Collider _pauseCollider;

    [SerializeField]
    private string _sceneOne;
    [SerializeField]
    private GameObject[] _buttonsToEnable;

    //private MemoryManager _memoryManager; // Reference to MemoryManager for handling memory

    private void Start()
    {
        _isPaused = false;
        _isExiting = false;
        _hasConfirmed = false;
        _buttonsToEnable[0].SetActive(false);
        _buttonsToEnable[1].SetActive(false);
    }

    #region UI Buttons

    public void PressPlay()
    {
        _playButtonTimeline.Play();
        _playCollider.enabled = false;
        _pauseCollider.enabled = true;

        if (!_hasConfirmed)
        {
            _hasConfirmed = true;
            if (_hasConfirmed)
            {
                // Play the current timeline
                _timelineDirectors[_currentTimelineIndex].Play();
                _buttonsToEnable[0].SetActive(true);
                _buttonsToEnable[1].SetActive(true);
            }
        }

        if (_isPaused)
        {
            _isPaused = false;
            _timelineDirectors[_currentTimelineIndex].Resume();
        }
    }

    public void PressPause()
    {
        if (!_isPaused)
        {
            // Pause the current timeline
            _timelineDirectors[_currentTimelineIndex].Pause();
            _isPaused = true;
            _pauseCollider.enabled = false;
            _playCollider.enabled = true;
            _playButtonTimeline.Resume();
        }
    }

    public void PressExit()
    {
        _homeTrayTimeline.Play();
        _isExiting = true;
    }

    public void PressCancel()
    {
        _homeTrayTimeline.Resume();
        _isExiting = false;
    }

    public void PressConfirm()
    {
        if (_isExiting)
        {
            StartCoroutine(ExitToSceneOne());
        }
    }

    public void EndExit()
    {
        StartCoroutine(ExitToSceneOne());
    }

    private IEnumerator ExitToSceneOne()
    {
        MemoryManager.Instance.CleanupMemoryBeforeSceneChange();
        yield return new WaitForSeconds(1); // Wait for a moment to ensure cleanup

        SceneManager.LoadScene(_sceneOne);
    }

    public void PlayButtonPause()
    {
        _playButtonTimeline.Pause();
    }

    public void TrayHold()
    {
        _homeTrayTimeline.Pause();
    }

    #endregion

    //added by Dan below
    public void PressQuit()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            //gameManager.RunExitAnim(); // Play the exit animation
            Invoke(nameof(QuitGame), 1f); // Delay quitting to allow animation to play
        }
        else
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

    private void QuitGame()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.QuitGameOnSignal(); // Trigger quitting
        }
    }
    //added by Dan above


    // Method to switch to the next timeline
    public void UpdateCurrentTimeline()
    {
        // Check if there are more timelines available
        if (_currentTimelineIndex < _timelineDirectors.Length - 1)
        {
            // Increment index to switch to the next timeline
            _currentTimelineIndex++;
            // Play the new timeline
            _timelineDirectors[_currentTimelineIndex].Play();
            int _previousTimeline = _currentTimelineIndex - 1;
            // Optimize memory usage by unloading unused assets
            MemoryManager.Instance.OptimizeMemoryAfterTimelineChange(_previousTimeline);
        }
        else
        {
            Debug.Log("All timelines played.");
        }
    }
}

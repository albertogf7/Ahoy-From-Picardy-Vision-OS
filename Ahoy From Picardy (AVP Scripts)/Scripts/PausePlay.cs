using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class PausePlay : MonoBehaviour
{
    [SerializeField]
    private Collider _adjusterCircle;

    [SerializeField]
    private bool _isPaused = false;
    [SerializeField]
    private Sprite playSprite;
    [SerializeField]
    private Sprite pauseSprite;
    private Image _ppImage;
    [SerializeField]
    private PlayableDirector _mainTimeline;

    private void Start()
    {
        _ppImage = GetComponent<Image>();
        _isPaused = false;
    }

    public void TogglePausePlay()
    {
        if (!_isPaused)
        {
            // Pause the game
            _mainTimeline.Pause();
            _ppImage.sprite = playSprite; // Change to the sprite for play
            _adjusterCircle.enabled = true;
            _isPaused = true;
        }
        else
        {
            // Game is resumed
            _mainTimeline.Resume();
            _ppImage.sprite = pauseSprite; // Change to the sprite for pause
            _adjusterCircle.enabled = false;
            _isPaused = false;
        }
    }
}

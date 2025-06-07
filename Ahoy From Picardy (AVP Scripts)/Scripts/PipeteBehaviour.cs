using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PipeteBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool _pipeteTapped = false;
    private bool _colliderEnabled = false;
    [SerializeField]
    private PlayableDirector _pipeteDirector;
    [SerializeField]
    private float _loopAtTime;
    [SerializeField]
    private BoxCollider _pipeteCollider;
    [SerializeField]
    private GameObject _confirmationSound;


    private void Start()
    {
        _pipeteCollider.enabled = false;
        _confirmationSound.gameObject.SetActive(false);
    }
    public void CheckLoop()
    {
        if (!_pipeteTapped)
        {
            _pipeteDirector.time = _loopAtTime;
        }
        else
        {
            return;
        }
    }
    public void Drop()
    {
        if (!_pipeteTapped)
        {
            _pipeteTapped = true;
            _confirmationSound.gameObject.SetActive(true);
        }
    }

    public void EnablePipeteCollider()
    {
        if (!_colliderEnabled)
        {
            _pipeteCollider.enabled = true;
        }
    }
}
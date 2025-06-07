using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TrainTracksBehaviour : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector[] tracksTimelines; // Array of timelines for each track
    [SerializeField]
    private BoxCollider[] _pillarColliders;
    [SerializeField]
    private int laidTracks = 0; // Counter for the number of tracks laid
    [SerializeField]
    private PlayableDirector _transformationTimeline; // Reference to the train timeline
    private bool _allTracksSet = false;
    [SerializeField]
    private float _loopAtTime;
    [SerializeField]
    private GameObject _trainSoundObject;
    private bool _pillarsColliderEnabled;

    private void Start()
    {
        _trainSoundObject.gameObject.SetActive(false);
        foreach (BoxCollider collider in _pillarColliders)
        {
            collider.enabled = false;
        }

    }
    public void CheckLoop()
    {
        if (!_allTracksSet)
        {
            _transformationTimeline.time = _loopAtTime;
        }
        else
        {
            return;
        }
    }

    // Method to set a track based on the pillar ID
    public void SetTrack(int trackId)
    {
        if (trackId >= 0 && trackId < tracksTimelines.Length)
        {
            tracksTimelines[trackId].Play();
            laidTracks++;
            CheckTracks();
        }
        else
        {
            Debug.LogError("Invalid trackId: " + trackId);
        }
    }

    // Check if all tracks have been laid
    private void CheckTracks()
    {
        if (laidTracks >= tracksTimelines.Length)
        {
            Debug.Log("All Tracks Set");
            _allTracksSet = true;
            _trainSoundObject.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Must lay more tracks");
        }
    }

    public void EnablePillarCollider()
    {
        if (!_pillarsColliderEnabled)
        {
            foreach(BoxCollider collider in _pillarColliders)
            {
                collider.enabled = true;
            }
            
        }
    }
}

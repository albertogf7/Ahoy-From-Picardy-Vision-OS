using UnityEngine;
using FMODUnity;

public class FMODAudioManager : MonoBehaviour
{
    // Static instance of FMODAudioManager
    private static FMODAudioManager _instance;

    // The tag used to identify audio-related GameObjects
    [SerializeField]
    private string audioSourceTag = "AudioSourceTag";

    public static FMODAudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FMODAudioManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("FMODAudioManager");
                    _instance = obj.AddComponent<FMODAudioManager>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Method to ensure audio sources are available by finding GameObjects with a specific tag
    public void EnsureAudioSourcesAvailable()
    {
        GameObject[] audioSourceObjects = GameObject.FindGameObjectsWithTag(audioSourceTag);

        foreach (GameObject obj in audioSourceObjects)
        {
            StudioEventEmitter emitter = obj.GetComponent<StudioEventEmitter>();
            if (emitter == null)
            {
                emitter = obj.AddComponent<StudioEventEmitter>();
                emitter.PlayEvent = EmitterGameEvent.None; // Customize based on needs
            }
        }
    }

    // Method to initialize or reset audio sources by finding GameObjects with a specific tag
    public void InitializeOrResetAudioSources()
    {
        GameObject[] audioSourceObjects = GameObject.FindGameObjectsWithTag(audioSourceTag);

        foreach (GameObject obj in audioSourceObjects)
        {
            StudioEventEmitter emitter = obj.GetComponent<StudioEventEmitter>();
            if (emitter == null)
            {
                emitter = obj.AddComponent<StudioEventEmitter>();
                emitter.PlayEvent = EmitterGameEvent.None; // Customize based on needs
            }
            else
            {
                emitter.Stop();
                emitter.EventInstance.clearHandle();
            }
        }
    }
}

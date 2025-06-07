using UnityEngine;

public class AudioSetup : MonoBehaviour
{
    private void Start()
    {
        // Ensure all necessary audio sources are available using the FMODAudioManager singleton
        FMODAudioManager.Instance.EnsureAudioSourcesAvailable();
    }
}

using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] tracks;
    [SerializeField] private int currentTrack = 0;



    public void PlayTrack(int trackIndex)
    {
        if (tracks == null || tracks.Length == 0)
        {
            Debug.LogWarning("AudioController: No tracks assigned!");
            return;
        }

        if (trackIndex < 0 || trackIndex >= tracks.Length)
        {
            Debug.LogWarning($"AudioController: Track index {trackIndex} is out of range!");
            return;
        }

        currentTrack = trackIndex;
        audioSource.clip = tracks[currentTrack];
        audioSource.Play();
    }

    public void PlayNext()
    {
        int nextTrack = (currentTrack + 1) % tracks.Length;
        PlayTrack(nextTrack);
    }

    public void PlayPrevious()
    {
        int prevTrack = (currentTrack - 1 + tracks.Length) % tracks.Length;
        PlayTrack(prevTrack);
    }

    public void Stop()
    {
        audioSource.Stop();
        Debug.Log("AudioController: Stopped playback");
    }

}

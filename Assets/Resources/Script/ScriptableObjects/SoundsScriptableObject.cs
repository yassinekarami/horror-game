using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundsScriptableObject", menuName = "Scriptable Objects/SoundsScriptableObject")]
public class SoundsScriptableObject : ScriptableObject
{
    public List<AudioClip> audioClips;

    /// <summary>
    /// Plays the audio clip at the specified index from the audioClips list using the given AudioSource if it is not
    /// already playing.
    /// </summary>
    /// <param name="source">The AudioSource to play the audio clip on.</param>
    /// <param name="index">The index of the audio clip to play from the audioClips list.</param>
    public void PlayAudioClipAtIndex(AudioSource source, int index )
    {
        if (audioClips != null && index >= 0 && index < audioClips.Count)
        {
            AudioClip clip = audioClips[index];
            if (source != null && !source.isPlaying)
            {
                source.PlayOneShot(clip);
            }
        }
    }

    /// <summary>
    /// Retrieves the AudioClip at the specified index from the audioClips collection.
    /// </summary>
    /// <param name="index">The zero-based index of the AudioClip to retrieve.</param>
    /// <returns>The AudioClip at the specified index, or null if the index is out of range or the collection is null.</returns>
    public AudioClip GetAudioClipAtIndex(int index)
    {
        AudioClip audio = null;
        if (audioClips != null && index >= 0 && index < audioClips.Count)
        {
            audio = audioClips[index];
        }
        return audio;
    }

    /// <summary>
    /// Plays the specified audio clip once using the given audio source.
    /// </summary>
    /// <param name="source">The AudioSource component used to play the audio clip.</param>
    /// <param name="audioClip">The AudioClip to be played.</param>
    public void PlayAudioClip(AudioSource source, AudioClip audioClip)
    {
        source.PlayOneShot(audioClip);
    }
    /// <summary>
    /// Plays a random audio clip from the collection on the specified AudioSource if it is not already playing.
    /// </summary>
    /// <param name="source">The AudioSource on which to play the random audio clip.</param>
    public void PlayRandomAudioClip(AudioSource source)
    {
        if (audioClips != null && audioClips.Count > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Count);
            AudioClip randomClip = audioClips[randomIndex];
            if (source != null && !source.isPlaying)
            {
                source.PlayOneShot(randomClip);
            }
        }
    }
}

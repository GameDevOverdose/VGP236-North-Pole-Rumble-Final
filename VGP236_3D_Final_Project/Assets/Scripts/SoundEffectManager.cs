using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager _instance;
    public static SoundEffectManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PlaySoundInSpace(AudioClip clip, Vector3 position)
    {
        PlaySoundInSpace(clip, position, 1.0f);
    }

    public void PlaySoundInSpace(AudioClip clip, Vector3 position, float volume)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
    }

    public AudioClip GetRandomSound(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning("GetRandomSound called with null or empty clips array.");
            return null;
        }

        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }
}

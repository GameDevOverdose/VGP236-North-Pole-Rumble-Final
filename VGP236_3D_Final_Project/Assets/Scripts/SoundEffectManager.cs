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
        AudioSource.PlayClipAtPoint(clip, position);
    }
}

using UnityEngine;

[DefaultExecutionOrder(-10)]
public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public struct Music
    {
        public AudioClip audioClip;
        public float volume;
    }

    private static MusicManager _instance;
    public static MusicManager Instance { get { return _instance; } }

    private AudioSource _audioSource;

    [SerializeField] private Music[] _menuMusic;
    [SerializeField] private Music[] _gameMusic;
    [SerializeField] private Music[] _winMusic;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);

        _instance = this;

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.spatialBlend = 0f; // 2D music
        _audioSource.playOnAwake = false;
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if ((_audioSource.clip == clip && _audioSource.isPlaying) || clip == null)
        {
            return;
        }

        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
        _audioSource.clip = null;
    }

    public void PauseMusic()
    {
        _audioSource.Pause();
    }

    public void ResumeMusic()
    {
        if (_audioSource.clip != null)
        {
            _audioSource.UnPause();
        }
    }

    public void SetVolume(float volume)
    {
        _audioSource.volume = Mathf.Clamp01(volume);
    }

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }

    public void PlayMenuMusic()
    {
        _audioSource.loop = true;

        AudioClip musicToPlay = GetMenuMusic();

        PlayMusic(musicToPlay, GetMusicVolume(musicToPlay));
    }

    public void PlayGameMusic()
    {
        _audioSource.loop = true;

        AudioClip musicToPlay = GetGameMusic();

        PlayMusic(musicToPlay, GetMusicVolume(musicToPlay));
    }

    public void PlayWinMusic()
    {
        _audioSource.loop = false;

        AudioClip musicToPlay = GetWinMusic();

        PlayMusic(musicToPlay, GetMusicVolume(musicToPlay));
    }

    public AudioClip GetMenuMusic()
    {
        return GetRandomMusic(_menuMusic);
    }

    public AudioClip GetGameMusic()
    {
        return GetRandomMusic(_gameMusic);
    }

    public AudioClip GetWinMusic()
    {
        return GetRandomMusic(_winMusic);
    }

    public AudioClip GetRandomMusic(Music[] music)
    {
        return SoundEffectManager.Instance.GetRandomSound(MusicArrayToClips(music));
    }

    private AudioClip[] MusicArrayToClips(Music[] music)
    {
        if (music == null || music.Length == 0)
        {
            return null;
        }

        AudioClip[] clips = new AudioClip[music.Length];

        for (int i = 0; i < music.Length; i++)
        {
            clips[i] = music[i].audioClip;
        }

        return clips;
    }

    public float GetMusicVolume(AudioClip clip)
    {
        if (clip == null)
        {
            return 0f;
        }

        if (_menuMusic != null)
        {
            for (int i = 0; i < _menuMusic.Length; i++)
            {
                if (_menuMusic[i].audioClip == clip)
                {
                    return Mathf.Clamp01(_menuMusic[i].volume);
                }
            }
        }

        if (_gameMusic != null)
        {
            for (int i = 0; i < _gameMusic.Length; i++)
            {
                if (_gameMusic[i].audioClip == clip)
                {
                    return Mathf.Clamp01(_gameMusic[i].volume);
                }
            }
        }

        if (_winMusic != null)
        {
            for (int i = 0; i < _winMusic.Length; i++)
            {
                if (_winMusic[i].audioClip == clip)
                {
                    return Mathf.Clamp01(_winMusic[i].volume);
                }
            }
        }

        return 1f;
    }

    public float GetCurrentMusicStoredVolume()
    {
        return GetMusicVolume(_audioSource.clip);
    }
}

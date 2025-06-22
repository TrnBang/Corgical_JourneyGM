using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;

    public AudioClip grassStepClip;
    public AudioClip barkClip;

    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    public void PlayGrassStep()
    {
        sfxSource.PlayOneShot(grassStepClip);
    }

    public void PlayBark()
    {
        sfxSource.PlayOneShot(barkClip);
    }
}

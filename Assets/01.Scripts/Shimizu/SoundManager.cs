using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource seSource;

    [Header("Audio Clips")]
    public AudioClip[] bgmClips; // 複数のBGM
    public AudioClip[] seClips;  // 複数のSE

    [Header("Volumes")]
    [Range(0f, 1f)] public float bgmVolume = 1.0f;
    [Range(0f, 1f)] public float seVolume = 1.0f;

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

    private void Update()
    {
        if (bgmSource != null) bgmSource.volume = bgmVolume;
        if (seSource != null) seSource.volume = seVolume;

        if (Input.GetMouseButtonDown(0)) // 0は左クリックを示す
        {
            PlaySE(0);
        }
        if (Input.GetMouseButtonDown(1)) // 0は左クリックを示す
        {
            PlaySE(1);
        }
        if (Input.GetMouseButtonDown(2)) // 0は左クリックを示す
        {
            PlaySE(2);
        }
    }

    public void PlayBGM(int index, bool loop = true)
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            bgmSource.clip = bgmClips[index];
            bgmSource.loop = loop;
            bgmSource.Play();
        }
    }

    public void PlaySE(int index)
    {
        if (index >= 0 && index < seClips.Length)
        {
            seSource.PlayOneShot(seClips[index]);
        }
    }

    public AudioClip GetBGMClip(int index)
    {
        return (index >= 0 && index < bgmClips.Length) ? bgmClips[index] : null;
    }

    public AudioClip GetSEClip(int index)
    {
        return (index >= 0 && index < seClips.Length) ? seClips[index] : null;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0f, 1f);
    }

    public void SetSEVolume(float volume)
    {
        seVolume = Mathf.Clamp(volume, 0f, 1f);
    }
}

using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;

    private void Start()
    {
        if (bgmSlider != null)
        {
            bgmSlider.value = SoundManager.Instance.bgmVolume;
            bgmSlider.onValueChanged.AddListener(SoundManager.Instance.SetBGMVolume);
        }

        if (seSlider != null)
        {
            seSlider.value = SoundManager.Instance.seVolume;
            seSlider.onValueChanged.AddListener(SoundManager.Instance.SetSEVolume);
        }
    }
}

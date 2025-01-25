using UnityEngine;

public class AudioController : MonoBehaviour
{
    public int bgmIndex = 0; // 再生するBGMのインデックス
    public int seIndex = 0;  // 再生するSEのインデックス

    private void Start()
    {
        // BGMを再生
        PlayBGM();

        // 必要ならSEを再生
        // PlaySE();
    }

    public void PlayBGM()
    {
        //SoundManager.Instance.PlayBGM(SoundManager.Instance.GetBGMClip(bgmIndex));
    }

    public void PlaySE()
    {
        //SoundManager.Instance.PlaySE(SoundManager.Instance.GetSEClip(seIndex));
    }
}

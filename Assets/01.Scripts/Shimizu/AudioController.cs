using UnityEngine;

public class AudioController : MonoBehaviour
{
    public int bgmIndex = 0; // �Đ�����BGM�̃C���f�b�N�X
    public int seIndex = 0;  // �Đ�����SE�̃C���f�b�N�X

    private void Start()
    {
        // BGM���Đ�
        PlayBGM();

        // �K�v�Ȃ�SE���Đ�
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

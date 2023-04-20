using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //���� �̸�.
    public AudioClip clip; //���� MP3 ������ ���� ��
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] Sound[] sfx = null;
    [SerializeField] Sound[] bgm = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource[] SfxPlayer = null;

    private void Start()
    {
        instance = this;    
    }
    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if(p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                for (int x = 0; x < SfxPlayer.Length; x++)
                {
                    if(!SfxPlayer[x].isPlaying)
                    {
                        SfxPlayer[x].clip = sfx[i].clip;
                        SfxPlayer[x].Play();
                        return;
                    }
                }
                Debug.Log("��� ����� �÷��̾ ��� ���Դϴ�");
                return;
            }
        }

        Debug.Log(p_sfxName + "�̸��� ȿ������ �����ϴ�.");
    }
}

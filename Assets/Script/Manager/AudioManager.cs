using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //사운드 이름.
    public AudioClip clip; //실제 MP3 파일이 들어가는 곳
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
                Debug.Log("모든 오디오 플레이어가 재생 중입니다");
                return;
            }
        }

        Debug.Log(p_sfxName + "이름의 효과음이 없습니다.");
    }
}

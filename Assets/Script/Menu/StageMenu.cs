using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Song
{
    public string name;
    public string composer;
    public int bpm;//곡마다 다르게 해서 노트생성시간을 조절
    public Sprite sprite;
}

public class StageMenu : MonoBehaviour
{
    [SerializeField] Song[] songList = null;

    [SerializeField] Text txtsongName = null;
    [SerializeField] Text txtsongComposer = null;
    [SerializeField] Text txtSongScore = null;
    [SerializeField] Image imgDisk = null;

    [SerializeField] GameObject TitleUI = null;

    DataManager theData;

    int currentSong = 0;

    void OnEnable()//켜질떄마다 호출
    {
        if (theData == null)
            theData = FindObjectOfType<DataManager>();
        SettingSong();
    }

    public void Btnnext()
    {
        AudioManager.instance.PlaySFX("Touch");
        if (++currentSong > songList.Length -1)//인덱스의 최고 갯수는 렝스 -1
        {
            currentSong = 0;
        }
        SettingSong();
    }

    public void BtnPrior()
    {
        AudioManager.instance.PlaySFX("Touch");

        if (--currentSong < 0)
        {
            currentSong = songList.Length - 1;
        }
        SettingSong();
    }

    void SettingSong()
    {
        txtsongName.text = songList[currentSong].name;
        txtsongComposer.text = songList[currentSong].composer;
        txtSongScore.text = string.Format("{0:#,##0}", theData.score[currentSong]);
        imgDisk.sprite = songList[currentSong].sprite;

        AudioManager.instance.PlayBGM("BGM" + currentSong);
    }

    public void BtnBack()
    {
        TitleUI.SetActive(true);
        this.gameObject.SetActive(false);
        AudioManager.instance.StopBGM();
    }

    public void BtnPlay()
    {
        int t_Bpm = songList[currentSong].bpm;
        GameManager.instance.GameStart(currentSong,t_Bpm);
        this.gameObject.SetActive(false);
    }
}

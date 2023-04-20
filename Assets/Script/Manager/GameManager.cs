using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] goGameUi = null;
    [SerializeField] GameObject goTilteUi = null;

    public static GameManager instance;

    public bool isStart_game = false;

    ComboManager thecom;
    ScoreManager thescore;
    TimingManager theTiming;
    StatusManager theStatus;
    PlayerController thePlayer;
    StageManager theStage;
    NoteManager theNote;
    Result theResult;

    [SerializeField] CenterFrame theMusic = null;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        theNote = FindObjectOfType<NoteManager>();
        theStage = FindObjectOfType<StageManager>();
        thecom = FindObjectOfType<ComboManager>();
        thescore = FindObjectOfType<ScoreManager>();
        theTiming = FindObjectOfType<TimingManager>();
        theStatus = FindObjectOfType<StatusManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        theResult = FindObjectOfType<Result>();
        
    }

    public void GameStart(int p_songNum, int p_bpm)
    {
        for (int i = 0; i < goGameUi.Length; i++)
        {
            goGameUi[i].SetActive(true);
        }
        theMusic.bgmName = "BGM" + p_songNum;
        theNote.Bpm = p_bpm;
        theStage.RemoveStage();
        theStage.SettingStage(p_songNum);
        thecom.ResetCombo();
        thescore.Initialized();
        theTiming.Initialized();
        theStatus.Initialized();
        thePlayer.Initialized();
        theResult.SetCurrentSong(p_songNum);

        AudioManager.instance.StopBGM();

        isStart_game = true;
    }

    public void MainMenu()
    {
        for (int i = 0; i < goGameUi.Length; i++)
        {
            goGameUi[i].SetActive(false);
        }
        
        goTilteUi.SetActive(true);
    }    
}

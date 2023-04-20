using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject GoUi = null;
    [SerializeField] Text[] txtCount = null;
    [SerializeField] Text txtCoin = null;
    [SerializeField] Text txtScore = null;
    [SerializeField] Text txt_MaxCombo = null;

    int currentSong = 0; public void SetCurrentSong(int p_songNum) { currentSong = p_songNum; }

    ScoreManager theScore;
    ComboManager theCombo;
    TimingManager theTiming;
    DataManager theData;
    GameManager thegame;
    // Start is called before the first frame update
    void Start()
    {
        theScore = FindObjectOfType<ScoreManager>();
        theCombo = FindObjectOfType<ComboManager>();
        theTiming = FindObjectOfType<TimingManager>();
        theData = FindObjectOfType<DataManager>();
        thegame = FindObjectOfType<GameManager>();
    }

   public void ShowResult()
    {
        FindObjectOfType<CenterFrame>().ResetMusic();

        AudioManager.instance.StopBGM();

        GoUi.SetActive(true);
        thegame.goGameUi[3].gameObject.SetActive(false);

        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = "0";
        }
        txtCoin.text = "0";
        txtScore.text = "0";
        txt_MaxCombo.text = "0";

        int[] t_judgement = theTiming.GetJudgementRecord();
        int t_currentScore = theScore.GetCurrentScore();
        int t_MaxCombo = theCombo.GetMaxCombo();
        int t_coin = (t_currentScore / 50);

        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = string.Format("{0:#,##0}", t_judgement[i]);
        }
        txtScore.text = string.Format("{0:#,##0}", t_currentScore);
        txt_MaxCombo.text = string.Format("{0:#,##0}", t_MaxCombo);
        txtCoin.text = string.Format("{0:#,##0}", t_coin);

        if(t_currentScore > theData.score[currentSong])
        {
            theData.score[currentSong] = t_currentScore;
            theData.SaveScore();
        }
           
    }

    public void BtnMainMenu()
    {
        GoUi.SetActive(false);
        GameManager.instance.MainMenu();
        theCombo.ResetCombo();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text txtScore = null;

    [SerializeField] int increaseScore = 10; //점수가 얼마씩 올라가질에 대한 기본값.
    int currentScore = 0;//현재점수를 담을 변수.

    [SerializeField] float[] weight = null;//판정에따라 다른 점수.
    [SerializeField] int comboBonusScore = 10;

    Animator myanim;
    string animScoreUp = "ScoreUp";

    ComboManager theCombo;

    // Start is called before the first frame update
    void Start()
    {
        theCombo = FindObjectOfType<ComboManager>();
        myanim = GetComponent<Animator>();
        currentScore = 0;
        txtScore.text = "0";
    }
    
    public void Initialized()
    {
        currentScore = 0;
        txtScore.text = "0";
    }

     public void IncreaseScore(int p_JudgeMentState)//어떤 노트 판정을 받아왔는지에 대한 변수.
     {
        //콤보증가.
        theCombo.IncreaseCombo();

        //콤보 보너스점수 계산.
        int t_currentCombo = theCombo.GetcurrentCombo();
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;//콤보 보너스 점수 (현재콤보/10) *10 10점대면 +10점,20점대면 +20점씩 되게.

        //넘어오는 숫자에따라서 가중치 계산.
        int t_IncreaseScore = increaseScore + t_bonusComboScore;
        t_IncreaseScore = (int)(t_IncreaseScore * weight[p_JudgeMentState]);
        //점수 반영
        currentScore += t_IncreaseScore;
        txtScore.text = string.Format("{0:#,##0}", currentScore);
        //애니 실행
        myanim.SetTrigger(animScoreUp);
     }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}

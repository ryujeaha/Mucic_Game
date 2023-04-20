using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text txtScore = null;

    [SerializeField] int increaseScore = 10; //������ �󸶾� �ö����� ���� �⺻��.
    int currentScore = 0;//���������� ���� ����.

    [SerializeField] float[] weight = null;//���������� �ٸ� ����.
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

     public void IncreaseScore(int p_JudgeMentState)//� ��Ʈ ������ �޾ƿԴ����� ���� ����.
     {
        //�޺�����.
        theCombo.IncreaseCombo();

        //�޺� ���ʽ����� ���.
        int t_currentCombo = theCombo.GetcurrentCombo();
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;//�޺� ���ʽ� ���� (�����޺�/10) *10 10����� +10��,20����� +20���� �ǰ�.

        //�Ѿ���� ���ڿ����� ����ġ ���.
        int t_IncreaseScore = increaseScore + t_bonusComboScore;
        t_IncreaseScore = (int)(t_IncreaseScore * weight[p_JudgeMentState]);
        //���� �ݿ�
        currentScore += t_IncreaseScore;
        txtScore.text = string.Format("{0:#,##0}", currentScore);
        //�ִ� ����
        myanim.SetTrigger(animScoreUp);
     }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}

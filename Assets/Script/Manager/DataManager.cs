using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public int[] score;

    private void Start()
    {
        LoadScore();
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score1", score[0]); //데이터를 자체기기에 저장(앱을 지우면 복구불가) 괄호안에 있는거는 문법으로 (key,value)를 설정해주면 되는데 key는 이름 value는 데이터 값이다.
        PlayerPrefs.SetInt("Score2", score[1]);
        PlayerPrefs.SetInt("Score3", score[2]);
    }

    public void LoadScore()
    {
        if(PlayerPrefs.HasKey("Score1"))//아까 설정한 키가 있는지 확인해야 한다 없을시 버그가 뜸. 있을시 true 값이 리턴
        {
            score[0] = PlayerPrefs.GetInt("Score1");
            score[1] = PlayerPrefs.GetInt("Score2");
            score[2] = PlayerPrefs.GetInt("Score3");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();//판정범위에 있는지 비교하기 위해서 생성된 노트 게임오브젝트들을 담을 수 있는 리스트를 만들어준다 

    int[] judgementRecord = new int[5];//기록용 배열

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;//perpect,cool.good,bad를 판정하기 위한 트랜스폼.
    Vector2[] timingBoxs = null;//판정범위의 최소값(x),최대값(y)

    EffectManager the_Effect;
    ScoreManager the_Score;
    ComboManager theCombo;
    StageManager theStage;
    PlayerController the_player;
    StatusManager theStatus;
    AudioManager theAudio;

    void Start()
    {
        theAudio = AudioManager.instance;
        the_Effect = FindObjectOfType<EffectManager>();
        the_Score = FindObjectOfType<ScoreManager>();
        theCombo = FindObjectOfType<ComboManager>();
        theStage = FindObjectOfType<StageManager>();
        the_player = FindObjectOfType<PlayerController>();
        theStatus = FindObjectOfType<StatusManager>();
        //타이밍 박스 설정.
        timingBoxs = new Vector2[timingRect.Length];
        for(int i = 0; i < timingRect.Length;  i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,
                Center.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    public bool CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            float t_NotePosX = boxNoteList[i].transform.localPosition.x;

            for(int x = 0; x < timingBoxs.Length; x++)
            {
                if(timingBoxs[x].x <= t_NotePosX && t_NotePosX <= timingBoxs[x].y)
                {
                    //노트 제거.
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);
                    //이펙트 연출.
                    if (x < timingBoxs.Length - 1)
                        the_Effect.NoteHitEffect();
                    
                    if(CheckCanNextPlate())
                    {
                        the_Score.IncreaseScore(x);//점수증가
                        theStage.ShowNextPlate();//판때기 등장.
                        the_Effect.JudgeMentEffect(x);
                        judgementRecord[x]++;// 판정 기록
                        theStatus.CheckShield();
                    }
                    else
                    {
                        the_Effect.JudgeMentEffect(5);
                    }

                    theAudio.PlaySFX("Clap");

                    return true;
                }
            }
        }
        theCombo.ResetCombo();
        the_Effect.JudgeMentEffect(timingBoxs.Length);
        MissRecord();
        return false;
    }

    bool CheckCanNextPlate()
    {
        if (Physics.Raycast(the_player.destPos,Vector3.down,out RaycastHit t_hitInfo,1.1f)) //가상의 광선을 쏴서 맞은 대상의 정보를 가져오는 함수.(광선위치,방향,충돌정보,길이)
        {
            if(t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_Plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if(t_Plate.flag)
                {
                    t_Plate.flag = false;   
                    return true;
                }
            }
        }
        return false;
    }
    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()
    {
        judgementRecord[4]++;// 판정 기록
        theStatus.ResetShieldCombo(); 
    }

    public void Initialized()
    {
        for (int i = 0; i < judgementRecord.Length; i++)
            judgementRecord[i] = 0;
    }
}

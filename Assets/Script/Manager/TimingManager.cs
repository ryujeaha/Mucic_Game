using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();//���������� �ִ��� ���ϱ� ���ؼ� ������ ��Ʈ ���ӿ�����Ʈ���� ���� �� �ִ� ����Ʈ�� ������ش� 

    int[] judgementRecord = new int[5];//��Ͽ� �迭

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;//perpect,cool.good,bad�� �����ϱ� ���� Ʈ������.
    Vector2[] timingBoxs = null;//���������� �ּҰ�(x),�ִ밪(y)

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
        //Ÿ�̹� �ڽ� ����.
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
                    //��Ʈ ����.
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);
                    //����Ʈ ����.
                    if (x < timingBoxs.Length - 1)
                        the_Effect.NoteHitEffect();
                    
                    if(CheckCanNextPlate())
                    {
                        the_Score.IncreaseScore(x);//��������
                        theStage.ShowNextPlate();//�Ƕ��� ����.
                        the_Effect.JudgeMentEffect(x);
                        judgementRecord[x]++;// ���� ���
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
        if (Physics.Raycast(the_player.destPos,Vector3.down,out RaycastHit t_hitInfo,1.1f)) //������ ������ ���� ���� ����� ������ �������� �Լ�.(������ġ,����,�浹����,����)
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
        judgementRecord[4]++;// ���� ���
        theStatus.ResetShieldCombo(); 
    }

    public void Initialized()
    {
        for (int i = 0; i < judgementRecord.Length; i++)
            judgementRecord[i] = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int Bpm = 0; //1분당 비트수
    double currentTime = 0d;//오차가 있으면 안되기에 플롯대신 오차범위가 적은 더블을 사용.

   

    [SerializeField] Transform tfNoteAppear = null;
    //[SerializeField] GameObject goNote = null; 노트 프리팹을 담고 있던 변수.

    TimingManager thetimingManager;
    EffectManager theEffectManager;
    ComboManager  theComboManager;

    private void Start()
    {
        theEffectManager = FindObjectOfType<EffectManager>();//이걸 가지고 있는 친구에게 접근.
        thetimingManager = GetComponent<TimingManager>();//이 변수에다가 컴포넌트를 가져오는것
        theComboManager = FindObjectOfType<ComboManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isStart_game)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 60d / Bpm)//60d/bpm = 1비트 시간. 120이면 1비트당 0.5초
            {
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue(); //큐에 담긴 객체를 뺴오기.
                t_note.transform.position = tfNoteAppear.position;
                t_note.SetActive(true);
                //GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
                //t_note.transform.SetParent(this.transform);//이게 없으면 캔버스 안에서가 아닌 하이어라키에서 생성돼서 보이지가 않는다. 부모를 이 스크립트가 붙어있는 객체로 설정.
                thetimingManager.boxNoteList.Add(t_note);
                currentTime -= 60d / Bpm;
            }
        }

       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if(collision.GetComponent<Note>().GetNoteFlag())
            {
                thetimingManager.MissRecord();
                theEffectManager.JudgeMentEffect(4);
                theComboManager.ResetCombo();
            }   
            thetimingManager.boxNoteList.Remove(collision.gameObject);
            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);//반납하기
            collision.gameObject.SetActive(false);  
            //Destroy(collision.gameObject);
        }
    }

    public void ReMoveNote()
    {
        GameManager.instance.isStart_game = false;

        for (int i = 0; i < thetimingManager.boxNoteList.Count; i++)
        {
            thetimingManager.boxNoteList[i].SetActive(false);
            ObjectPool.instance.noteQueue.Enqueue(thetimingManager.boxNoteList[i]);
        }

        thetimingManager.boxNoteList.Clear();//노트 제거후 초기화
    }
}

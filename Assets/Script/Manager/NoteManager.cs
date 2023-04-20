using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int Bpm = 0; //1�д� ��Ʈ��
    double currentTime = 0d;//������ ������ �ȵǱ⿡ �÷Դ�� ���������� ���� ������ ���.

   

    [SerializeField] Transform tfNoteAppear = null;
    //[SerializeField] GameObject goNote = null; ��Ʈ �������� ��� �ִ� ����.

    TimingManager thetimingManager;
    EffectManager theEffectManager;
    ComboManager  theComboManager;

    private void Start()
    {
        theEffectManager = FindObjectOfType<EffectManager>();//�̰� ������ �ִ� ģ������ ����.
        thetimingManager = GetComponent<TimingManager>();//�� �������ٰ� ������Ʈ�� �������°�
        theComboManager = FindObjectOfType<ComboManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isStart_game)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 60d / Bpm)//60d/bpm = 1��Ʈ �ð�. 120�̸� 1��Ʈ�� 0.5��
            {
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue(); //ť�� ��� ��ü�� ������.
                t_note.transform.position = tfNoteAppear.position;
                t_note.SetActive(true);
                //GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
                //t_note.transform.SetParent(this.transform);//�̰� ������ ĵ���� �ȿ����� �ƴ� ���̾��Ű���� �����ż� �������� �ʴ´�. �θ� �� ��ũ��Ʈ�� �پ��ִ� ��ü�� ����.
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
            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);//�ݳ��ϱ�
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

        thetimingManager.boxNoteList.Clear();//��Ʈ ������ �ʱ�ȭ
    }
}

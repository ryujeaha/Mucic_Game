using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float NoteSpeed = 400f;

    UnityEngine.UI.Image noteImage;

    void OnEnable()//��ü�� Ȱ��ȭ �ɋ����� ȣ��Ǵ� �Լ�.
    {
        if(noteImage == null)
        {
            noteImage = GetComponent<UnityEngine.UI.Image>();
        }
        noteImage.enabled = true;
    }

    public void HideNote()
    {
        noteImage.enabled = false;
    }

    void Update()
    {
        transform.localPosition += Vector3.right * NoteSpeed * Time.deltaTime;//�� ��ũ��Ʈ�� �پ��ִ� ��ü�� ��������(local) �����ǰ��� �ð��� ���������� ���������� ���ǵ尪��ŭ �����ش�. 
    }

    public bool GetNoteFlag()
    {
        return noteImage.enabled;
    }
}

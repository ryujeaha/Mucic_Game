using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float NoteSpeed = 400f;

    UnityEngine.UI.Image noteImage;

    void OnEnable()//객체가 활성화 될떄마다 호출되는 함수.
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
        transform.localPosition += Vector3.right * NoteSpeed * Time.deltaTime;//이 스크립트가 붙어있는 객체의 절대적인(local) 포지션값을 시간이 지날떄마다 오른쪽으로 스피드값만큼 곱해준다. 
    }

    public bool GetNoteFlag()
    {
        return noteImage.enabled;
    }
}

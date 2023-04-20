using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource theaudio;
    NoteManager thenote;

    Result theResult;
    // Start is called before the first frame update
    void Start()
    {
        theaudio = GetComponent<AudioSource>();
        thenote = FindObjectOfType<NoteManager>();
        theResult = FindObjectOfType<Result>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            theaudio.Play();
            PlayerController.s_canPresskey = false;
            thenote.ReMoveNote();
            theResult.ShowResult();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerController>().ResetFalling(); //ºÎ¸ð °´Ã¼ÀÇ Æ¯Á¤ ÄÄÆ÷³ÍÆ®¸¦ °¡Á®¿È.
        }
    }
}

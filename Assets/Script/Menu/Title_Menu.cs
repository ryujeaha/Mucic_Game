using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_Menu : MonoBehaviour
{
    [SerializeField] GameObject goStageUI = null;

    public void BtnPlay()
    {
        goStageUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}

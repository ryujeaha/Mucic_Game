using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject goComboImage = null;
    [SerializeField] UnityEngine.UI.Text txtCombo = null;

    int currentCombo = 0    ;
    int maxCombo = 0;

    Animator myanim;
    string animComboUp = "ComboUp";

    private void Start()
    {
        myanim = GetComponent<Animator>();
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }

    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += p_num;
        txtCombo.text = string.Format("{0:#,##0}", currentCombo);//세자리 단위마다 콤마를 찍어줌.

        if(maxCombo < currentCombo)
        {
            maxCombo = currentCombo;
        }
       

        if(currentCombo > 2)
        {
            txtCombo.gameObject.SetActive(true);
            goComboImage.SetActive(true);

            myanim.SetTrigger(animComboUp);
        }
    }

    public int GetcurrentCombo()//현재콤보를 다른곳에서 쓰기위해서 반환.
    {
        return currentCombo;
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        txtCombo.text = "0";
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }
    public int GetMaxCombo()
    {
        return maxCombo;
    }
}

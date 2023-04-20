using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusManager : MonoBehaviour
{
    [SerializeField] float blickSpeed = 0.1f;
    [SerializeField] int blickCount = 10;
    int currentBlinkCount = 0; 
    bool isBlink = false;    

    bool isDead = false;

    int maxHp = 3;
    int currentHp = 3;

    int maxShield = 3;
    int currentShield = 0;

    [SerializeField] Image[] hpImages = null;
    [SerializeField] Image[] ShieldImages = null;

    [SerializeField] int ShieldIncreaseCombo = 5;
    int currentShieldCombo = 0;
    [SerializeField] Image ShieldGauge = null;

    Result theResult;
    NoteManager theNote;
    [SerializeField] MeshRenderer PlayerMesh = null;

    private void Start()
    {
        theResult = FindObjectOfType<Result>();
        theNote = FindObjectOfType<NoteManager>();
    }

    public void Initialized()
    {
        currentHp = maxHp;
        currentShield = 0;
        currentShieldCombo = 0;
        ShieldGauge.fillAmount = 0;
        isDead = false;
        SettingHPImage();
        SettingShieldImage();
    }

    public void CheckShield()
    {
        currentShieldCombo++;

        if(currentShieldCombo >= ShieldIncreaseCombo)
        {
            currentShieldCombo = 0;
            IncreaseShield();
        }

        ShieldGauge.fillAmount = (float)currentShieldCombo / ShieldIncreaseCombo; 
    }

    public void ResetShieldCombo()
    {
        currentShieldCombo = 0;
        ShieldGauge.fillAmount = (float)currentShieldCombo / ShieldIncreaseCombo;
    }

    public void IncreaseShield()
    {
        currentShield++;

        if(currentShield >= maxShield)
        {
            currentShield = maxShield;
        }

        SettingShieldImage();
    }

    public void IncreaseHP(int p_num)
    {
        currentHp += p_num;
        if(currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
        SettingHPImage();
    }

    public void DecreasShield(int p_num)
    {
        currentShield -= p_num;

        if (currentShield <= 0)
        {
            currentShield = 0;
        }
            

        SettingShieldImage(); 


    }

    public void DecreaseHP(int p_num)
    {
        if(!isBlink)
        {
            if(currentShield > 0)
            {
                DecreasShield(p_num);
            }
            else
            {
                currentHp -= p_num;

                if (currentHp <= 0)
                {
                    isDead = true;
                    theResult.ShowResult();
                    theNote.ReMoveNote();
                }
                else
                {
                    StartCoroutine(BlinkCo());
                }

                SettingHPImage();
            }
           

           
        }
       
    }

    void SettingHPImage()
    {
        for (int i = 0; i < hpImages.Length; i++)
        {
            if( i < currentHp)
            {
                hpImages[i].gameObject.SetActive(true);
            }
            else
            {
                hpImages[i].gameObject.SetActive(false);
            }
        }
    }
    void SettingShieldImage()
    {
        for (int i = 0; i < ShieldImages.Length; i++)
        {
            if (i < currentShield)
            {
                ShieldImages[i].gameObject.SetActive(true);
            }
            else
            {
                ShieldImages[i].gameObject.SetActive(false);
            }
        }
    }
    public bool IsDead()
    {
        return isDead;
    }

    IEnumerator BlinkCo()
    {
        isBlink = true;

        while(currentBlinkCount <= blickCount)
        {
            PlayerMesh.enabled = !PlayerMesh.enabled;
            yield return new WaitForSeconds(blickSpeed);
            currentBlinkCount++;
        }

        PlayerMesh.enabled = true;
        currentBlinkCount = 0;
        isBlink = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject[] stageArray = null;//클래스.
    GameObject currentStage;
    Transform[] stagePlates;//스테이지 클래스에 있는 플레이트들.

    [SerializeField] float offsetY = -5;
    [SerializeField] float PlateSpeed = 10;

    int stepCount = 0;//걸음수 판단 변수.  
    int totalPlateCount = 0;//들어온 현재스테이지를 구성하는 발판의 총 갯수.
        
    public void RemoveStage()
    {
        if (currentStage != null)
        {
            Destroy(currentStage);
        }
    }

    public void SettingStage(int p_songNum)
    {
        stepCount = 0;

        currentStage =  Instantiate(stageArray[p_songNum], Vector3.zero,Quaternion.identity); //(생성물,위치,회전(안준다는 기본ㄴ값이라는 뜻)
        stagePlates = currentStage.GetComponent<Stage>().Plates;//스테이지 안에 있는 객체들을 불러온다.
        totalPlateCount = stagePlates.Length;

        for (int i = 0; i < totalPlateCount; i++)
        {
            stagePlates[i].position = new Vector3(stagePlates[i].position.x, 
                                                  stagePlates[i].position.y + offsetY,
                                                  stagePlates[i].position.z);
        }
    }

    public void ShowNextPlate()
    {
        if(stepCount < totalPlateCount)
        {
            StartCoroutine(MovePlateCo(stepCount++));
        }
    }

    IEnumerator MovePlateCo(int p_num)
    {
        stagePlates[p_num].gameObject.SetActive(true);
        Vector3 t_desPos = new Vector3(stagePlates[p_num].position.x,
                                       stagePlates[p_num].position.y - offsetY,
                                       stagePlates[p_num].position.z);
        while(Vector3.SqrMagnitude(stagePlates[p_num].position - t_desPos) >= 0.001f)//제곱근을 반환시키는 함수.
        {
            stagePlates[p_num].position = Vector3.Lerp(stagePlates[p_num].position, t_desPos, PlateSpeed * Time.deltaTime);
            yield return null;
        }

        stagePlates[p_num].position = t_desPos;

    }
}

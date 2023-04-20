using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo//객체 정보.
{
    public GameObject goPrefab;
    public int count;
    public Transform tfPoolParent;
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField] ObjectInfo[] objectInfo = null;

    public static ObjectPool instance;//공유자원화 시켜서 어디서든 참조가능

    public Queue<GameObject> noteQueue = new Queue<GameObject>(); //Queue 선입선출 자료형 (가장 먼저 들어간 데이터가 가장 먼저 빠져나옴.)

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        noteQueue = InsertQueue(objectInfo[0]);
    }

    Queue<GameObject> InsertQueue(ObjectInfo p_objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        for (int i = 0; i <  p_objectInfo.count; i++)
        {
            GameObject t_clone = Instantiate(p_objectInfo.goPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);
            if(p_objectInfo.tfPoolParent != null)//부모객체가 존재한다면 그 객체를 부모로 없다면 이 스크립트가 붙어이는 객체를 부모로.
            {
                t_clone.transform.SetParent(p_objectInfo.tfPoolParent);
            }
            else
            {
                t_clone.transform.SetParent(this.transform);
            }

            t_queue.Enqueue(t_clone);//생성한객체를 큐에 넣기.
        }
        return t_queue; 
    }
}

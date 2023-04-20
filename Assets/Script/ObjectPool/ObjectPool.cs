using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo//��ü ����.
{
    public GameObject goPrefab;
    public int count;
    public Transform tfPoolParent;
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField] ObjectInfo[] objectInfo = null;

    public static ObjectPool instance;//�����ڿ�ȭ ���Ѽ� ��𼭵� ��������

    public Queue<GameObject> noteQueue = new Queue<GameObject>(); //Queue ���Լ��� �ڷ��� (���� ���� �� �����Ͱ� ���� ���� ��������.)

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
            if(p_objectInfo.tfPoolParent != null)//�θ�ü�� �����Ѵٸ� �� ��ü�� �θ�� ���ٸ� �� ��ũ��Ʈ�� �پ��̴� ��ü�� �θ��.
            {
                t_clone.transform.SetParent(p_objectInfo.tfPoolParent);
            }
            else
            {
                t_clone.transform.SetParent(this.transform);
            }

            t_queue.Enqueue(t_clone);//�����Ѱ�ü�� ť�� �ֱ�.
        }
        return t_queue; 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform thePlayer = null; //����ٴ� Ÿ��.
    [SerializeField] float followSpeed = 15;

    Vector3 PlayerDistance = new Vector3();//�Ÿ� ���̸� ����ų ����.

    float hitDistance = 0f;
    [SerializeField] float zoomDistance = -1.25f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerDistance = transform.position - thePlayer.position;//�Ÿ�����.
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 t_destPos = thePlayer.position + PlayerDistance + (transform.forward * hitDistance);
        transform.position = Vector3.Lerp(transform.position, t_destPos, followSpeed * Time.deltaTime); //Lerp(A,B,C) =A��B������ ������ C������ ���� ���� 
    }

    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;

        yield return new WaitForSeconds(0.15f);

        hitDistance = 0;
    }
}

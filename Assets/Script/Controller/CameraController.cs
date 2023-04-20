using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform thePlayer = null; //따라다닐 타겟.
    [SerializeField] float followSpeed = 15;

    Vector3 PlayerDistance = new Vector3();//거리 차이를 기억시킬 변수.

    float hitDistance = 0f;
    [SerializeField] float zoomDistance = -1.25f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerDistance = transform.position - thePlayer.position;//거리차이.
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 t_destPos = thePlayer.position + PlayerDistance + (transform.forward * hitDistance);
        transform.position = Vector3.Lerp(transform.position, t_destPos, followSpeed * Time.deltaTime); //Lerp(A,B,C) =A와B사이의 값에서 C비율의 값을 추출 
    }

    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;

        yield return new WaitForSeconds(0.15f);

        hitDistance = 0;
    }
}

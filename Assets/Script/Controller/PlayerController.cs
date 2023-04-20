using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool s_canPresskey = true;

    // 이동
    [SerializeField] float moveSpeed = 3;
    Vector3 dir = new Vector3(); //어느방향으로 움작알지.
    public  Vector3 destPos = new Vector3();
    Vector3 originpos = new Vector3();
    public int X_Key;
    public int Y_Key;
    //회전
    [SerializeField] float spinSpeed = 270;
    Vector3 rotDir = new Vector3();//어느 방향으로 회전시킬지.
    Quaternion destRot = new Quaternion(); //목표 회전값.
    //반동
    [SerializeField] float recoilPosY = 0.25f;
    [SerializeField] float recoilSpeed = 1.5f;
    //기타
    [SerializeField] Transform fakecube = null;
    [SerializeField] Transform realCube = null;

    bool canMove = true;
    bool isFalling = false;

    TimingManager thetimingManager;
    CameraController thecam;
    Rigidbody myRigid;
    StatusManager theStatus;

    private void Start()
    {
        thetimingManager = FindObjectOfType<TimingManager>();
        theStatus = FindObjectOfType<StatusManager>();
        thecam = FindObjectOfType<CameraController>();
        myRigid = GetComponentInChildren<Rigidbody>();//자식객체 중에 해당 컴포넌트가 있으면 가져옴
        originpos = transform.position;
    }

    public void Initialized()
    {
        transform.position = Vector3.zero;
        destPos = Vector3.zero;
        realCube.localPosition = Vector3.zero;
        canMove = true;
        s_canPresskey = true;
        isFalling = false;
        myRigid.useGravity = false;
        myRigid.isKinematic = true;
    }

    void Update()
    {
        if (GameManager.instance.isStart_game)
        {
            CheckFalling();
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
            {
                if (canMove && s_canPresskey && !isFalling)
                {
                    Calc();
                    if (thetimingManager.CheckTiming())
                    {
                        StartAction();//트루값이 넘어왔을때만. 올바른 판정에서만 움직임.
                    }
                }
            }
        }
    }

    public void A_Clik()
    {
        if (canMove && s_canPresskey && !isFalling)
        {
            X_Key = -1;
            Y_Key = 0;
            Calc();
            if (thetimingManager.CheckTiming())
            {
                StartAction();//트루값이 넘어왔을때만. 올바른 판정에서만 움직임.
            }
        }
    }
    public void D_Clik()
    {
        if (canMove && s_canPresskey && !isFalling)
        {
            X_Key = 1;
            Y_Key = 0;
            Calc();
            if (thetimingManager.CheckTiming())
            {
                StartAction();//트루값이 넘어왔을때만. 올바른 판정에서만 움직임.
            }
        }
    }

    public void W_Clik()
    {
        if (canMove && s_canPresskey && !isFalling)
        {
            Y_Key = 1;
            X_Key = 0;
            Calc();
            if (thetimingManager.CheckTiming())
            {
                StartAction();//트루값이 넘어왔을때만. 올바른 판정에서만 움직임.
            }
        }
    }

    public void S_Clik()
    {
        if (canMove && s_canPresskey && !isFalling)
        {
            Y_Key = -1;
            X_Key = 0;
            Calc();
            if (thetimingManager.CheckTiming())
            {
                StartAction();//트루값이 넘어왔을때만. 올바른 판정에서만 움직임.
            }
        }
    }

    void Calc()
    {
        //방향 계산
        dir.Set(Y_Key, 0, X_Key);//x축을 왼쪽 z축을 오른쪽을 바라보게 했기때문에 그 부분에 해당하는 키를 눌렀을때 값이 반환되게하여 움직임을 제어.

        //이동 목표값 계산.
        destPos = transform.position + new Vector3(-dir.x, 0, dir.z);

        //회전 목표값 계산.
        rotDir = new Vector3(-dir.z, 0f, -dir.x);
        fakecube.RotateAround(transform.position, rotDir, spinSpeed); //태양주변을 공전하는 지구등을 구현할때 사용.(공전대상,회전 축,회전값)
        destRot = fakecube.rotation;
    }
    void StartAction()
    {
        StartCoroutine(MoveGo());
        StartCoroutine(Spin());
        StartCoroutine(RecoilCo());
        StartCoroutine(thecam.ZoomCam());
    }

    IEnumerator MoveGo()
    {
        canMove = false;
        // while(Vector3.Distance(transform.position,destPos)!= 0)//Vector3.Distance = A좌표와 B좌표간의 거리차를 반환. 
        while (Vector3.SqrMagnitude(transform.position -destPos) >= 0.001f)//SqrMagnitude : 제곱근을 리턴, ex SqrMagnitude(4) = 2
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = destPos;
        canMove = true;
    }
    IEnumerator Spin()
    {
        while (Quaternion.Angle(realCube.rotation, destRot) > 0.5f) //두개의 값에 차이를 구하는 문법.
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, destRot, spinSpeed * Time.deltaTime);
            yield return null;
        }

        realCube.rotation = destRot;
    }

    IEnumerator RecoilCo()
    {
        while(realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        while(realCube.position.y > 0)
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        realCube.localPosition = new Vector3(0, 0, 0);
    }

    void CheckFalling()
    {
        if(!isFalling && canMove)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))//충돌한게 없을 경우.
            {
                Falling();
            }
        }
        
    }

    void Falling()
    {
        isFalling = true;
        myRigid.useGravity = true;
        myRigid.isKinematic = false; //물리효과를 켜주기위해서 펄스
    }

    public void ResetFalling()
    {
        theStatus.DecreaseHP(1);
        AudioManager.instance.PlaySFX("Falling");
        if(!theStatus.IsDead())
        {
            isFalling = false;
            myRigid.useGravity = false;
            myRigid.isKinematic = true;
            transform.position = originpos;
            realCube.localPosition = new Vector3(0, 0, 0);
        }
       
    }
}

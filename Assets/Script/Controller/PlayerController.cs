using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool s_canPresskey = true;

    // �̵�
    [SerializeField] float moveSpeed = 3;
    Vector3 dir = new Vector3(); //����������� ���۾���.
    public  Vector3 destPos = new Vector3();
    Vector3 originpos = new Vector3();
    public int X_Key;
    public int Y_Key;
    //ȸ��
    [SerializeField] float spinSpeed = 270;
    Vector3 rotDir = new Vector3();//��� �������� ȸ����ų��.
    Quaternion destRot = new Quaternion(); //��ǥ ȸ����.
    //�ݵ�
    [SerializeField] float recoilPosY = 0.25f;
    [SerializeField] float recoilSpeed = 1.5f;
    //��Ÿ
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
        myRigid = GetComponentInChildren<Rigidbody>();//�ڽİ�ü �߿� �ش� ������Ʈ�� ������ ������
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
                        StartAction();//Ʈ�簪�� �Ѿ��������. �ùٸ� ���������� ������.
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
                StartAction();//Ʈ�簪�� �Ѿ��������. �ùٸ� ���������� ������.
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
                StartAction();//Ʈ�簪�� �Ѿ��������. �ùٸ� ���������� ������.
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
                StartAction();//Ʈ�簪�� �Ѿ��������. �ùٸ� ���������� ������.
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
                StartAction();//Ʈ�簪�� �Ѿ��������. �ùٸ� ���������� ������.
            }
        }
    }

    void Calc()
    {
        //���� ���
        dir.Set(Y_Key, 0, X_Key);//x���� ���� z���� �������� �ٶ󺸰� �߱⶧���� �� �κп� �ش��ϴ� Ű�� �������� ���� ��ȯ�ǰ��Ͽ� �������� ����.

        //�̵� ��ǥ�� ���.
        destPos = transform.position + new Vector3(-dir.x, 0, dir.z);

        //ȸ�� ��ǥ�� ���.
        rotDir = new Vector3(-dir.z, 0f, -dir.x);
        fakecube.RotateAround(transform.position, rotDir, spinSpeed); //�¾��ֺ��� �����ϴ� �������� �����Ҷ� ���.(�������,ȸ�� ��,ȸ����)
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
        // while(Vector3.Distance(transform.position,destPos)!= 0)//Vector3.Distance = A��ǥ�� B��ǥ���� �Ÿ����� ��ȯ. 
        while (Vector3.SqrMagnitude(transform.position -destPos) >= 0.001f)//SqrMagnitude : �������� ����, ex SqrMagnitude(4) = 2
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = destPos;
        canMove = true;
    }
    IEnumerator Spin()
    {
        while (Quaternion.Angle(realCube.rotation, destRot) > 0.5f) //�ΰ��� ���� ���̸� ���ϴ� ����.
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
            if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))//�浹�Ѱ� ���� ���.
            {
                Falling();
            }
        }
        
    }

    void Falling()
    {
        isFalling = true;
        myRigid.useGravity = true;
        myRigid.isKinematic = false; //����ȿ���� ���ֱ����ؼ� �޽�
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

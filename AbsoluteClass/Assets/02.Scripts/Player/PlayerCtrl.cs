using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [System.Serializable] //클래스를 인스펙터 뷰에 노출시켜줄 어트리뷰트(Attribute)
    public class PlayerAnim
    {
        public AnimationClip idle;
        public AnimationClip runF;
        public AnimationClip runB;
        public AnimationClip runR;
        public AnimationClip runL;
    }

    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;

    //[SerializeField] 
    private Transform tr;
    //이동 관련
    public float moveSpeed = 10.0f;
    public float rotateSpeed = 80.0f;

    //애니메이션 관련
    //인스펙터 뷰에 표시할 애니메이션 클래스 변수
    [HideInInspector] public PlayerAnim playerAnim; //노출 뒤 컴포넌트 연결 후 다시 숨겨줌 or [System.NonSerialized]
    //애니메이션 컴포넌트를 저장하기 위한 변수
    public Animation anim;

    void Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        anim.clip = playerAnim.idle;
        anim.Play();
    }
    
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Debug.Log("h =" + h.ToString());
        Debug.Log("v =" + v.ToString());

        //전후좌우 이동방향
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        //Translate(이동 방향, 이동 속도, 변위값, 델타타임, 기준좌표)
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        //Vector3.up 을 기준으로 rotateSpeed 속도로 회전
        tr.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * r);

        //키보드 입력 값에 따라 다른 애니메이션 수행
        if (v >= 0.1f)
        {
            anim.CrossFade(playerAnim.runF.name, 0.3f); //전진 애니메이션
        }

        else if (v <= -0.1f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.3f); //후진 애니메이션
        }

        else if (h >= 0.1f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.3f); //오른쪽 이동 애니메이션
        }

        else if (h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.3f); //왼쪽 이동 애니메이션
        }

        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f); //입력 없으면 다시 아이들로
        }
    }
}

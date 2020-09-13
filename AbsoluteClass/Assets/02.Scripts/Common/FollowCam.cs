using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;             //추적대상
    public float moveDamping    = 15.0f; //이동속도계수
    public float rotateDamping  = 10.0f; //회전속도계수
    public float distance       = 5.0f;  //추적대상과의거리
    public float height         = 4.0f;  //추적대상과의 높이
    public float targetOffset   = 2.0f;  //추적대상좌표의 오프셋

    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    //플레이어 이동이 완료된 후 처리하기 위해
    private void LateUpdate()
    {
        //카메라 높이와 거리 계산
        var camPos = target.position - (target.forward * distance) + (target.up * height);

        //이동할 때 속도계수 적용
        tr.position = Vector3.Slerp(tr.position, camPos, Time.deltaTime * moveDamping);

        //회전할 때 속도계수 적용
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);

        //카메라를 추적 대상으로 Z축 회전
        tr.LookAt(target.position + (target.up * targetOffset));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //추적 및 시야를 맞출 위치 표시
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);
        //메인 카메라와 추적 지점 간의 선을 표시
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera;                //캔버스 렌더링하는 카메라
    private Canvas canvas;                  //UI용 최상위 카메라
    private RectTransform rectParent;       //부모 RectTransform 컴포넌트
    private RectTransform rectHp;           //자기자신 RectTransform 컴포넌트

    //체력바 이미지 조절할 오프셋
    [HideInInspector] public Vector3 offset = Vector3.zero;
    //추적할 대상의 컴포넌트
    [HideInInspector] public Transform targetTr;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }
    
    void LateUpdate()
    {
        //월드좌표->스크린좌표 전환
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
        //카메라 뒤쪽 영역(180도 회전)일 때 좌푯값 보정
        //z축이 음수가 되면 -1 곱함
        if (screenPos.z < 0.0f)
            screenPos *= -1.0f;

        //RectTransform 좌푯값을 전달 받을 함수
        var localPos = Vector2.zero;
        //스크린 좌표를 RectTransform 기준의 좌표로 변환
        //(부모의 RectTransform, 스크린 좌표, UI 렌더링 카메라, out 변환된 좌표)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);

        rectHp.localPosition = localPos;
    }
}

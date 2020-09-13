using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform itemTr;
    private Transform inventoryTr;
    private Transform itemListTr;
    private CanvasGroup canvasGroup;

    public static GameObject draggingItem = null;
    
    void Start()
    {
        itemTr = GetComponent<Transform>();
        inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();
        itemListTr = GameObject.Find("ItemList").GetComponent<Transform>();

        canvasGroup = GetComponent<CanvasGroup>();
    }

    //IDragHandler 빠른 작업 리팩터링 후
    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }

    //드래그 시작할 때 한 번 호출되는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        //부모를 인벤토리로 변경
        this.transform.SetParent(inventoryTr);
        //드래그가 시작되면 아이템 정보 저장
        draggingItem = this.gameObject;

        //드래그가 시작되면 다른 UI 이벤트 받지 않기
        canvasGroup.blocksRaycasts = false;
    }

    //드래그 종료했을 때 한 번 호출되는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        //드래그가 종료되면 드래그 아이템 null 변환
        draggingItem = null;
        //종료되면 다른 UI 이벤트 받기
        canvasGroup.blocksRaycasts = true;

        //슬롯에 드래그하지 않으면 다시 아이템 리스트로 돌린다
        if (itemTr.parent == inventoryTr)
        {
            itemTr.SetParent(itemListTr.transform);
        }
    }
}

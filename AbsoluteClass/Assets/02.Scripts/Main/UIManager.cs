using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //인자로 사용할 수 있는 데이터 타입
    //int, float, bool, string, Object
    public void OnClickStartBtn(RectTransform rt)
    {
        //Debug.Log("Click Button");
        Debug.Log("Scale X : " + rt.localScale.x.ToString());
    }
}

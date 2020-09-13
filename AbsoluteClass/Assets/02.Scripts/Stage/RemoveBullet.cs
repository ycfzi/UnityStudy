using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect;

    private void OnCollisionEnter(Collision collision)
    {
        //충돌
        if (collision.collider.tag == "BULLET")
        {
            //스파크 효과
            ShowEffect(collision);
            //게임오브젝트 삭제
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }

    void ShowEffect(Collision collision)
    {
        //충돌 지점의 정보 추출
        ContactPoint contact = collision.contacts[0];
        //법선 벡터가 이루는 회전 각도 추출
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        //스파크 효과 생성
        //Instantiate(sparkEffect, contact.point, rot);
        GameObject spark = Instantiate(sparkEffect, contact.point + (-contact.normal * 0.5f), rot);
        //스파크 효과의 부모를 드럼통or벽(this)로 생성
        spark.transform.SetParent(this.transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamange : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private float hp = 100.0f;
    private GameObject bloodEffect;

    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == bulletTag)
        {
            ShowBloodEffect(collision);
            Destroy(collision.gameObject);

            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;

            if (hp <= 0.0f)
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
        }
    }

    void ShowBloodEffect(Collision collision)
    {
        //총알이 충돌한 지점 산출
        Vector3 pos = collision.contacts[0].point;
        //총알 충돌 했을 때의 법선 벡터
        Vector3 _normal = collision.contacts[0].normal;
        //충돌 시 방향 벡터의 회전값 계산(총알 입사각의 반대)
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);

        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}

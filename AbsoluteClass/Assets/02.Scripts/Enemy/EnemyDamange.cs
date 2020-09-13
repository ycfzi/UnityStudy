using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamange : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private float hp = 100.0f;
    private float initHp = 100.0f;

    private GameObject bloodEffect;
    private EnemyAI enemyAI;

    //생명 게이지 프리팹을 저장할 변수
    public GameObject hpBarPrefab;
    //생명 게이지 위치를 보정할 오프셋
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    //부모가 될 캔버스 객체
    private Canvas uiCanvas;
    //생명 수치에 따라 fillAmount 속성을 변경할 Image
    private Image hpBarImage;

    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();

        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
        SetHpBar();
    }

    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        //UI 캔버스 하위로 생명 게이지 생성
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        //체력바가 따라가야 할 대상과 오프셋 값 설정
        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == bulletTag)
        {
            ShowBloodEffect(collision);
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);

            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            hpBarImage.fillAmount = hp / initHp;

            if (hp <= 0.0f)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                //적 캐릭터가 사망한 이후 생명 게이지 투명 처리
                hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
            }

            else if (!enemyAI.isHit)
            {
                enemyAI.isHit = true;
            }
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

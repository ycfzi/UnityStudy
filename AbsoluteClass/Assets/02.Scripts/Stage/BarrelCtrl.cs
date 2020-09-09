using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    //폭발 효과 프리팹 담을 오브젝트
    public GameObject expEffect;
    //찌그러진 드럼통의 메쉬를 저장할 배열
    public Mesh[] meshes;
    //드럼통의 텍스처를 저장할 배열
    public Texture[] testures;

    //맞은 횟수
    private int hitCount = 0;
    private Rigidbody rb;
    private MeshFilter meshFilter;
    //MeshRenderer 컴포넌트를 저장할 변수
    private MeshRenderer _renderer;
    //오디오 소스
    private AudioSource _audio;

    //폭발 반경
    public float expRadius = 10.0f;
    public AudioClip expSfx;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //메쉬필터 컴포넌트 추출 후 저장
        meshFilter = GetComponent<MeshFilter>();
        //메쉬랜더 컴포넌트 추출, 저장 후 랜덤으로 적용
        _renderer = GetComponent<MeshRenderer>();
        //오디오 컴포넌트를 추출해 저장
        _audio = GetComponent<AudioSource>();
        //난수를 발생시켜 불규칙적인 텍스쳐 적용
        _renderer.material.mainTexture = testures[Random.Range(0, testures.Length)];
    }

    //충돌할 때 한 번 호출되는 콜백 함수
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            if (++hitCount == 3) ExpBarrel();
        }
    }

    //폭발 효과 처리 함수
    void ExpBarrel()
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);
        //(삭제 대상, 딜레이 시간)
        Destroy(effect, 2.0f);

        //Quaternion.identity -> 무회전
        //Instantiate(gameObject, transform.position, Quaternion.identity);

        //무게(mass)를 가볍게 해서
        //rb.mass = 1.0f;
        //위로 떠오르게 힘을 가함
        //rb.AddForce(Vector3.up * 1000.0f);

        //폭발력 생성
        IndirectDamage(transform.position);

        //난수 발생
        int idx = Random.Range(0, meshes.Length);
        //찌그러진 메쉬 적용
        meshFilter.sharedMesh = meshes[idx];
        GetComponent<MeshCollider>().sharedMesh = meshes[idx];

        //폭발음 적용
        _audio.PlayOneShot(expSfx, 1.0f);
    }

    //폭발력을 주변에 전달하는 함수
    void IndirectDamage(Vector3 pos)
    {
        //주변에 있는 모든 드럼통 추출
        //physics(원점, 반지름, 레이어)
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8);

        foreach(var collision in colls)
        {
            //폭발 범위에 포함된 드럼통 rigidbody 컴포넌트 추출
            var _rb = collision.GetComponent<Rigidbody>();
            _rb.mass = 1.0f;
            //폭발력 전달
            //(횡 폭발력, 폭발 원점, 폭발 반경, 총 폭발력)
            _rb.AddExplosionForce(1200.0f, pos, expRadius, 1000.0f);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    private AudioSource audio;
    private Animator animator;
    private Transform playerTr;
    private Transform enemyTr;

    //애니메이터 컨트롤러에 정의한 파라미터의 해시값 추출
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    //다음 발사할 시간 계산용 변수
    private float nextFire = 0.0f;
    //총알 발사 간격
    private readonly float fireRate = 0.1f;
    //주인공 향해 회전할 속도 계수
    private readonly float damping = 10.0f;

    //재장전시간
    private readonly float reloadTime = 2.0f;
    //탄창 최대 총알 수
    private readonly int maxBullet = 10;
    //초기 총알 수
    private int currentBullet = 10;
    //재장전여부
    private bool isReload = false;
    //재장전 동안 기다리는 변수
    private WaitForSeconds wsReload;

    public bool isFire = false;
    public AudioClip fireSfx;
    public AudioClip realodSfx;

    public GameObject Bullet;
    public Transform firePos;
    public MeshRenderer muzzleFlash;
    
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);
    }
    
    void Update()
    {
        if (!isReload && isFire)
        {
            if (Time.time >= nextFire)
            {
                Fire();
                //Time.time 스크립트가 실행됐을 때부터 시작해서 계속 흘러가는 시간
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }

            //플레이어가 있는 위치까지 회전 각도 계산
            //벡터의 뺄셈은 (a, b) b좌표에서 a좌표로 가는 벡터
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            //보간 함수를 이용해 점진적으로 회전
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    void Fire()
    {
        animator.SetTrigger(hashFire);
        audio.PlayOneShot(fireSfx, 1.0f);
        StartCoroutine(ShowMuzzleFlash());

        //총알 생성
        GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);
        //일정 시간 후 삭제
        Destroy(_bullet, 3.0f);

        isReload = (--currentBullet % maxBullet == 0);
        if (isReload)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled = true;

        //불규칙한 회전 각도 계산
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        //MuzzleFlash를 z축 방향으로 회전
        muzzleFlash.transform.localRotation = rot;
        //Vector3.one = Vector3(1, 1, 1)
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1.0f, 2.0f);

        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        //muzzleFlash의 머티리얼의 Offset값 적용
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);

        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        muzzleFlash.enabled = false;
    }

    IEnumerator Reloading()
    {
        muzzleFlash.enabled = false;
        animator.SetTrigger(hashReload);
        audio.PlayOneShot(realodSfx, 1.0f);

        //재장전 시간 만큼 대기하는 동안 제어권 양보 
        yield return wsReload;

        currentBullet = maxBullet;
        isReload = false;
    }
}

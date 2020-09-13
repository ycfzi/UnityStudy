using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//오디오 클립 저장할 구조체
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        SHOTGUN = 0,
        RIFLE
    }

    //현재 무기 변수
    public WeaponType currentWeapon = WeaponType.SHOTGUN;

    //프리팹
    public GameObject bullet;
    //탄피 추출 파티클
    public ParticleSystem cartridge;
    //총구 화염 파티클
    private ParticleSystem muzzleFlash;
    private AudioSource _audio;

    //발사 좌표
    public Transform firePos;
    //오디오 클립 저장할 변수
    public PlayerSfx playerSfx;

    private Shake shake;

    void Start()
    {
        //FirePos 하위의 컴포넌트 추출
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }
    
    void Update()
    {
        //마우스 왼쪽 버튼
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        StartCoroutine(shake.ShakeCamera());
        //프리팹 동적 실행
        Instantiate(bullet, firePos.position, firePos.rotation);
        cartridge.Play();
        muzzleFlash.Play();
        FireSfx();
    }

    void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currentWeapon];
        _audio.PlayOneShot(_sfx, 1.0f);
    }
}

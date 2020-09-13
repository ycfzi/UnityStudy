using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";

    private float initHp = 100.0f;
    public float currentHp;
    public Image bloodScreen;

    public Image hpBar;
    private readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    private Color currentColor;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    void Start()
    {
        currentHp = initHp;

        hpBar.color = initColor;
        currentColor = initColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            Destroy(other.gameObject);

            StartCoroutine(ShowBloodScreen());

            currentHp -= 5.0f;
            Debug.Log("Player Hp = " + currentHp.ToString());

            DisplayHpbar();

            if (currentHp <= 0.0f)
                PlayerDie();
        }
    }

    IEnumerator ShowBloodScreen()
    {
        //블러드 스크린 텍스쳐의 알파값을 불규칙하게 변경
        bloodScreen.color = new Color(1, 0, 0, Random.Range(0.2f, -0.3f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;
    }

    void PlayerDie()
    {
        OnPlayerDie();
        GameManager.instance.isGameOver = true;

        //델리게이트 만들기 전
        //Debug.Log("PlayerDie !");
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        //
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
    }

    void DisplayHpbar()
    {
        //생명 50퍼까지는 초록->노랑
        if ((currentHp / initHp) > 0.5f)
            currentColor.r = (1 - (currentHp / initHp)) * 2.0f;
        //0일 때까지는 노랑->빨강
        else
            currentColor.g = (currentHp / initHp) * 2.0f;

        hpBar.color = currentColor;
        //체력바 크기 변경
        hpBar.fillAmount = (currentHp / initHp);
    }
}

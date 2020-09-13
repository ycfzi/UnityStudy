using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";

    private float initHp = 100.0f;
    public float currentHp;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    void Start()
    {
        currentHp = initHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            Destroy(other.gameObject);

            currentHp -= 5.0f;
            Debug.Log("Player Hp = " + currentHp.ToString());

            if (currentHp <= 0.0f)
                PlayerDie();
        }
    }

    void PlayerDie()
    {
        OnPlayerDie();

        //델리게이트 만들기 전
        //Debug.Log("PlayerDie !");
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        //
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
    }
}

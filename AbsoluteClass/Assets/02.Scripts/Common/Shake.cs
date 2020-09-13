using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Transform shakeCamera;
    //회전시킬 것인지 판단할 변수
    public bool shakeRotate = false;

    private Vector3 originPos;
    private Quaternion originRotation;

    void Start()
    {
        originPos = shakeCamera.localPosition;
        originRotation = shakeCamera.localRotation;
    }

    public IEnumerator ShakeCamera(float duration = 0.05f, float magnitudePos = 0.03f, float magnitudeRot = 0.1f)
    {
        //지나간 시간 저장할 변수
        float passTime = 0.0f;

        while (passTime < duration)
        {
            //insideUnitSphere -> 반경 1인 구체 내부의 3차원 좌푯값을 불규칙하게 반환
            Vector3 shakePos = Random.insideUnitSphere;
            shakeCamera.localPosition = shakePos * magnitudePos;

            if (shakeRotate)
            {
                //불규칙한 회전값을 펄린노이즈 함수를 이용해 추출
                Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magnitudeRot, 0.0f));
                shakeCamera.localRotation = Quaternion.Euler(shakeRot);
            }
            passTime += Time.deltaTime;

            yield return null;
        }

        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRotation;
    }
}

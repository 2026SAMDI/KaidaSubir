using System.Collections;
using UnityEngine;

public class PlayerCameraEffects : MonoBehaviour
{
    [Header("카메라 흔들림")]
    [SerializeField] private float shakeDuration = 0.1f; // 흔들리는 시간
    [SerializeField] private float shakeDelay = 0.05f; // 텀
    [SerializeField] private float shakeMagnitude = 0.15f; // 흔들리는 강도
    
    public void PlayLandShake()
    {
        StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        if (Camera.main == null) yield break;
        
        if (shakeDelay > 0f)
        {
            yield return new WaitForSeconds(shakeDelay);
        }
        
        Transform cam = Camera.main.transform;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            Vector3 shakeOffset = new Vector3(x, y, 0f);
            
            cam.position += shakeOffset; // 흔들기

            yield return null; // 한 프레임 대기(이 상태로 화면에 출력)

            cam.position -= shakeOffset; // 꼬임 방지를 위해 원상복구

            elapsed += Time.deltaTime;
        }
    }
}

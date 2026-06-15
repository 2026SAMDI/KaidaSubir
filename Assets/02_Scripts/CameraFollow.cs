using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("따라갈 대상 (플레이어)")]
    public Transform target;

    [Header("카메라 이동 제한 구역 (X축)")]
    public float minX; //왼쪽 벽 끝자락
    public float maxX; //오른쪽 벽 끝자락

    [Header("카메라 부드러움 정도")]
    public float smoothSpeed = 5f;
    
    private void LateUpdate()
    {
        //따라갈 플레이어가 없으면 중지
        if (target == null) return;

        //위치 설정: 플레이어의 X 위치만 따라가고 Y와 Z는 카메라의 원래 높이를 유지
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);

        //가두기: 목표 X 위치가 minX와 maxX를 절대 넘지 못하게 수학적으로 자르기
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);

        //부드러운 이동: 현재 위치에서 목표 위치까지 부드럽게 미끄러지듯 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}

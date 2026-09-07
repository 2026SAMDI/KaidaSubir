using UnityEngine;

public class Billboard : MonoBehaviour
{
    //메인 카메라를 담을 변수
    private Transform mainCamera;

    void Start()
    {
        //시작 시 메인 카메라의 Transform을 찾아 저장
        mainCamera = Camera.main.transform;
    }

    //모든 이동이 끝난 후 화면을 그리기 직전 호출되는 LateUpdate를 사용
    void LateUpdate()
    {
        if (mainCamera != null)
        {
            Vector3 targetPosition = mainCamera.position;
            targetPosition.y = transform.position.y; //오브젝트의 높이를 기준으로 카메라를 바라봅니다.
            transform.LookAt(targetPosition);
            
            // 스프라이트가 뒤집혀 보인다면, 180도 회전시켜주는 코드가 필요할 수 있습니다.
            // transform.Rotate(0, 180, 0); 
        }
    }
}

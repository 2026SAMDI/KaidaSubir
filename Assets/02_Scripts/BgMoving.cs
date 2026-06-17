using UnityEngine;

public class BgMoving : MonoBehaviour

{
    [Header("원근감 조절")]
    [SerializeField] private float parallaxEffect = 0.5f;

    private Transform cam;
    private Vector3 lastCamPos;

    void Start()
    {
        //시작할 때 메인 카메라 위치 기억
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }
    
    void LateUpdate() 
    {
        //이전 프레임 대비 카메라가 X축으로 얼마나 이동했는지 계산
        float deltaX = cam.position.x - lastCamPos.x;
        
        //배경 이동
        transform.position += new Vector3(deltaX * parallaxEffect, 0f, 0f);
        
        //현재 위치 갱신
        lastCamPos = cam.position;
    }
}

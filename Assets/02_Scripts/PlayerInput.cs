using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // 💡 다른 스크립트에서 읽을 수는 있지만(get), 수정할 수는 없게(private set) 보호해둡니다.
    public bool JumpInput { get; private set; }       // W키 (누른 순간)
    public bool DieInput { get; private set; }        // R키 (누른 순간)
    public bool DashLeftInput { get; private set; }   // A키 (누른 순간)
    public bool DashRightInput { get; private set; }  // D키 (누른 순간)
    
    public bool MoveLeftInput { get; private set; }   // A키 (누르고 있는 동안)
    public bool MoveRightInput { get; private set; }  // D키 (누르고 있는 동안)
    public bool FastFallInput { get; private set; }   // S키 (누르고 있는 동안)

    void Update()
    {
        // 💡 매 프레임 키보드 상태를 확인해서 변수에 저장합니다.
        // 나중에 조이스틱이나 모바일 버튼을 추가하려면 여기만 수정하면 됩니다!
        JumpInput = Input.GetKeyDown(KeyCode.W);
        DieInput = Input.GetKeyDown(KeyCode.R);
        DashLeftInput = Input.GetKeyDown(KeyCode.A);
        DashRightInput = Input.GetKeyDown(KeyCode.D);
        
        MoveLeftInput = Input.GetKey(KeyCode.A);
        MoveRightInput = Input.GetKey(KeyCode.D);
        FastFallInput = Input.GetKey(KeyCode.S);
    }
}

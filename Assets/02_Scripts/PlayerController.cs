using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private GameManager gameManager;
    
    [Header("이동")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("아이템")]
    [SerializeField] private int wCount = 20;
    [SerializeField] private int aCount = 20;
    [SerializeField] private int dCount = 20;
    
    [Header("아이템 감소 간격")]
    private float consumeInterval = 1f; //초마다 1씩 차감
    private float aTimer = 0f;
    private float dTimer = 0f;
    
    [Header("중력")]
    [SerializeField] private float fall = 2.5f; //떨어질 때 중력 배수
    
    [Header("바닥 확인")]
    [SerializeField] private float groundDistance = 1.1f; //플레이어 중심에서 바닥까지 거리
    [SerializeField] private LayerMask groundLayer; //바닥으로 인식할 레이어
    
    [Header("체공 후 조작")]
    [Range(0f, 1f)]
    [SerializeField] private float airControl = 0.1f;
    
    [Header("코요테 타임")]
    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    
    [Header("고속 착지")]
    [SerializeField] private float fastFallSpeed = 20f;

    [Header("공중 대시")]
    [SerializeField] private float airDashSpeed = 15f; //대시 순간 속도
    [SerializeField] private float dashDuration = 0.15f; //대시 지속 시간
    private float currentDashTime = 0f; //남은 대시 시간
    private float dashDirection = 0f; //대시 방향
    
    private bool isGameStarted = false; //게임 시작까지 대기
    
    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //Null에러 방지
        
        //지금 씬 튜토리얼 아닌지 확인
        if (SceneManager.GetActiveScene().name != "Stage00")
        {
            //아니면 제약 풀어주기
            isGameStarted = true;
        }
        
        if (uiManager != null) {
            uiManager.UpdateItemUI("W", wCount);
            uiManager.UpdateItemUI("A", aCount);
            uiManager.UpdateItemUI("D", dCount);
            uiManager.UpdateItemUI("R", "∞");
        }
    }
    
    void Update()
    {
        if (!isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                isGameStarted = true;
            }
            else
            {
                return; //W키 제외 입력 안 받기
            }
        }
        
        // 코요테 타임 타이머 계산
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime; 
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; 
        }

        //점프 입력은 오직 여기!!!!!
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            Jump();
        }
        
        //자폭(R)
        if (transform.position.y < -10f || Input.GetKeyDown(KeyCode.R))
        {
            Die();
        }
        
        if (!IsGrounded() && currentDashTime <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.A) && aCount > 0)
            {
                StartDash(-1f);
                aCount--;
                if (uiManager != null) uiManager.UpdateItemUI("A", aCount);
            }
            else if (Input.GetKeyDown(KeyCode.D) && dCount > 0)
            {
                StartDash(1f);
                dCount--;
                if (uiManager != null) uiManager.UpdateItemUI("D", dCount);
            }
        }
    }
    
    void FixedUpdate()
    {
        if (rb == null || !isGameStarted) return;

        //낙하 중력 보정
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * (Physics.gravity.y * (fall - 1) * Time.fixedDeltaTime);
        }
        
        Move();
        
        //최고 속도 제한
        if (rb.linearVelocity.y > jumpForce)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    void StartDash(float dir)
    {
        currentDashTime = dashDuration; //타이머 시작
        dashDirection = dir;            //방향 설정
    }
    
    void Move()
    {
        if (currentDashTime > 0)
        {
            currentDashTime -= Time.fixedDeltaTime;
            
            //대시 도중 Y축 고정
            rb.linearVelocity = new Vector3(dashDirection * airDashSpeed, 0f, 0f);
            
            return; //아래 로직은 대시 끝날 떄까지 완전 무시
        }
        
        float moveInput = 0;

        //왼쪽 이동(A)
        if (Input.GetKey(KeyCode.A) && aCount > 0)
        {
            moveInput = -1;
            aTimer += Time.fixedDeltaTime;
            if (aTimer >= consumeInterval)
            {
                aCount--;
                aTimer = 0f;
                if (uiManager != null) uiManager.UpdateItemUI("A", aCount);
                Debug.Log("왼쪽ㅣ남은 횟수: " + aCount);
            }
        }
        else
        {
            aTimer = consumeInterval; 
        }

        //오른쪽 이동(D)
        if (Input.GetKey(KeyCode.D) && dCount > 0)
        {
            moveInput = 1;
            dTimer += Time.fixedDeltaTime; // !! [수정] FixedUpdate 내부이므로 Time.fixedDeltaTime으로 변경
            if (dTimer >= consumeInterval)
            {
                dCount--;
                dTimer = 0f;
                if (uiManager != null) uiManager.UpdateItemUI("D", dCount);
                Debug.Log("오른쪽ㅣ남은 횟수: " + dCount);
            }
        }
        else
        {
            dTimer = consumeInterval;
        }

        //공중 조작 배수 계산
        float currentSpeed = IsGrounded() ? moveSpeed : moveSpeed * airControl;
        
        //Y축 속도 결정
        float targetVerticalVelocity = rb.linearVelocity.y;

        //고속 착지
        if (Input.GetKey(KeyCode.S) && !IsGrounded())
        {
            targetVerticalVelocity = -fastFallSpeed;
            Debug.Log("고속 착지");
        }

        //최종 속도 적용
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, targetVerticalVelocity);
    }
    
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer);
    }
        
    void Jump()
    {
        if (wCount > 0 && coyoteTimeCounter > 0f)
        {
            //상승 속도 리셋
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
            wCount--; 
            coyoteTimeCounter = 0f; //점프시 타이머 중지

            if (uiManager != null) uiManager.UpdateItemUI("W", wCount);
            Debug.Log("점프ㅣ남은 횟수:" + wCount);
        }
    }
    
    private void Die()
    {
        Debug.Log("사망ㅣ게임오버");
        
        if (gameManager != null) gameManager.AddDeath();
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
    }
}

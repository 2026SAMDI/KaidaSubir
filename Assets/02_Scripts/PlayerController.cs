using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(PlayerResourceManager))]
public class PlayerController : MonoBehaviour
{
    private PlayerResourceManager resourceManager;
    private PlayerAnimator playerAnimator;
    private PlayerInput playerInput;
    private Collider playerCollider;
    
    public event Action OnJumpEvent;
    public event Action OnDashEvent;
    public event Action OnLandEvent;
    public event Action OnSwordModeEvent;
    
    [Header("이동")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float swordMoveSpeed = 2.5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("아이템 감소 간격")]
    private float consumeInterval = 1f; //초마다 1씩 차감
    private float aTimer = 0f;
    private float dTimer = 0f;
    private float sTimer = 0f;
    
    [Header("중력")]
    [SerializeField] private float fall = 2.5f; //떨어질 때 중력 배수
    
    [Header("바닥 확인")]
    [SerializeField] private float groundDistance = 0.1f; //플레이어 바닥까지 거리
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
    
    [Header("검 모드")]
    private bool hasSwordMode = false;
    public bool HasSwordMode => hasSwordMode;
    
    private Rigidbody rb;
    private bool wasGrounded = true; 
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        resourceManager = GetComponent<PlayerResourceManager>();
        playerAnimator = GetComponent<PlayerAnimator>(); 
        playerInput = GetComponent<PlayerInput>();
        playerCollider = GetComponent<Collider>();
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
    }
    
    void Update()
    {
        bool grounded = IsGrounded(); //바닥 상태를 변수에 저장
        
        if (!wasGrounded && grounded)
        {
            OnLandEvent?.Invoke();
        }
        wasGrounded = grounded; 
        
        //코요테 타임 타이머 계산
        if (grounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        if (!isGameStarted)
        {
            if (playerInput.JumpInput)
            {
                isGameStarted = true;
            }
            else
            {
                return; //W키 제외 입력 안 받기
            }
        }
        
        //점프 입력은 오직 여기
        if (playerInput.JumpInput) 
        {
            Jump();
        }
        
        //자폭(R)
        if (transform.position.y < -10f || playerInput.DieInput)
        {
            resourceManager.Die();
        }
        
        if (!grounded && currentDashTime <= 0f)
        {
            if (playerInput.DashLeftInput && resourceManager.HasItem("A"))
            {
                StartDash(-1f);
                resourceManager.UseItem("A"); //아이템 소모
            }
            else if (playerInput.DashRightInput && resourceManager.HasItem("D"))
            {
                StartDash(1f);
                resourceManager.UseItem("D"); //아이템 소모
            }
        }

        //애니메이터에게 현재 상태 넘기기
        if (playerAnimator != null)
        {
            playerAnimator.UpdateAnimationState(grounded, rb.linearVelocity, hasSwordMode);
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
        dashDirection = dir; //방향 설정
        
        if (playerAnimator != null) playerAnimator.TriggerDash(); //애니메이션 트리거
        
        OnDashEvent?.Invoke();
    }
    
    void Move()
    {
        if (currentDashTime > 0)
        {
            currentDashTime -= Time.fixedDeltaTime;
            
            //대시 도중 Y축 고정
            rb.linearVelocity = new Vector3(dashDirection * airDashSpeed, 0f, 0f);
            
            return; //아래 로직은 대시 끝날 때까지 완전 무시
        }
        
        float moveInput = 0;

        //(입력 상태, 아이템 이름, 이동 방향, 타이머 변수, 최종 이동값)
        HandleDirection(playerInput.MoveLeftInput, "A", -1f, ref aTimer, ref moveInput);
        HandleDirection(playerInput.MoveRightInput, "D",  1f, ref dTimer, ref moveInput);
        
        float activeMoveSpeed = hasSwordMode ? swordMoveSpeed : moveSpeed;
        
        //공중 조작 배수 계산
        float currentSpeed = IsGrounded() ? activeMoveSpeed : activeMoveSpeed * airControl;
        
        //Y축 속도 결정
        float targetVerticalVelocity = rb.linearVelocity.y;

        //고속 착지
        if (playerInput.FastFallInput && !IsGrounded() && resourceManager.HasItem("S"))
        {
            targetVerticalVelocity = -fastFallSpeed;
            sTimer += Time.fixedDeltaTime; 
            if (sTimer >= consumeInterval)
            {
                resourceManager.UseItem("S"); //아이템 소모
                sTimer = 0f;
            }
        }
        else
        {
            sTimer = consumeInterval; 
        }

        //최종 속도 적용
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, targetVerticalVelocity);
    }
    
    private void HandleDirection(bool isPressed, string itemType, float dir, ref float timer, ref float moveInput)
    {
        if (isPressed && resourceManager.HasItem(itemType))
        {
            moveInput = dir; //이동 방향 설정
            timer += Time.fixedDeltaTime;
            if (timer >= consumeInterval)
            {
                resourceManager.UseItem(itemType); //아이템 소모
                timer = 0f;
            }
        }
        else
        {
            timer = consumeInterval; 
        }
    }
    
    private bool IsGrounded()
    {
        if (playerCollider == null) return false;
        Vector3 rayStart = new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y + 0.05f, playerCollider.bounds.center.z);
        return Physics.Raycast(rayStart, Vector3.down, groundDistance + 0.05f, groundLayer);
    }
        
    void Jump()
    {
        if (resourceManager.HasItem("W") && coyoteTimeCounter > 0f)
        {
            resourceManager.UseItem("W"); //아이템 소모
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            if (playerAnimator != null) playerAnimator.TriggerJump(); //애니메이션 트리거
            
            OnJumpEvent?.Invoke();
    
            coyoteTimeCounter = 0f; //점프시 타이머 중지
        }
    }

    public void ActivateSwordMode()
    {
        hasSwordMode = true;
        Debug.Log("검 모드 활성화");
        
        OnSwordModeEvent?.Invoke();
    }
}
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("아이템")]
    public int aCount = 10;
    public int dCount = 10;
    public int wCount = 5;

    private Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.W)) Jump();
        
        if (transform.position.y < -10f)
        {
            Die();
        }
    
        // R키를 누르면 언제든 자살(리셋) - 횟수 부족 대비
        if (Input.GetKeyDown(KeyCode.R))
        {
            Die();
        }
    }

    [Header("차감 간격 설정")]
    public float consumeInterval = 0.2f; //0.2초마다 1씩 차감
    private float aTimer = 0f;
    private float dTimer = 0f;

    void Move()
    {
        float moveInput = 0;

        //왼쪽 이동
        if (Input.GetKey(KeyCode.A) && aCount > 0)
        {
            moveInput = -1;
        
            //꾹 누르고 있을 때 실시간 차감
            aTimer += Time.deltaTime;
            if (aTimer >= consumeInterval)
            {
                aCount--;
                aTimer = 0f;
                Debug.Log("왼쪽 소모! 남은 횟수: " + aCount);
            }
        }
        else
        {
            aTimer = consumeInterval; //뗐다가 다시 누를 때 즉시 1 차감
        }

        //오른쪽 이동
        if (Input.GetKey(KeyCode.D) && dCount > 0)
        {
            moveInput = 1;

            //꾹 누르고 있을 때 실시간 차감 로
            dTimer += Time.deltaTime;
            if (dTimer >= consumeInterval)
            {
                dCount--;
                dTimer = 0f;
                Debug.Log("오른쪽ㅣ남은 횟수: " + dCount);
            }
        }
        else
        {
            dTimer = consumeInterval; //뗐다가 다시 누를 때 즉시 1 차감
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
    
    void Jump()
    {
        if (wCount > 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            wCount--;
            Debug.Log("점프ㅣ남은 횟수:" + wCount);
        }
    }
    
    public void Die()
    {
        Debug.Log("플레이어 사망! 씬을 재시작합니다.");
        
        //GameManager에 있는 죽은 횟수 올리기
        GameManager.AddDeath(); 
        
        //현재 씬을 다시 처음부터 불러오기
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}

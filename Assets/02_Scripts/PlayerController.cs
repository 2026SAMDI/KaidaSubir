using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("아이템")]
    [SerializeField] private int aCount = 10;
    [SerializeField] private int dCount = 10;
    [SerializeField] private int wCount = 5;
    
    [Header("차감 간격 설정")]
    public float consumeInterval = 0.2f; //0.2초마다 1씩 차감
    private float aTimer = 0f;
    private float dTimer = 0f;

    Rigidbody rb;

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
    
    private void Die()
    {
        Debug.Log("사망ㅣ게임오버");
        
        if (gameManager != null)
        {
            gameManager.AddDeath(); 
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
    }
}

using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 3f;
    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //왕복 이동
        transform.position += Vector3.right * direction * speed * Time.deltaTime;
        if (Mathf.Abs(transform.position.x - startPos.x) > distance)
        {
            direction *= -1; //반대 방향으로 전환
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            //검 모드 여부
            if (player != null && player.HasSwordMode)
            {
                Debug.Log("끼야아아악!!!");
                Destroy(gameObject); //처치
                return;
            }
            
            bool isFastFalling = Input.GetKey(KeyCode.S);
            
            bool isAboveSpike = collision.transform.position.y > transform.position.y + 0.3f;

            if (isFastFalling && isAboveSpike)
            {
                Debug.Log("끼야아악!");
                Destroy(gameObject); //처치
                
                // 밟았을 때 튕겨지기
                Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 1200f);
                }
            }
            else
            {
                //다른 경우에 사망
                PlayerResourceManager res = collision.gameObject.GetComponent<PlayerResourceManager>();
                if (res != null) res.Die();
            }
        }
    }
}

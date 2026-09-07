using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [Header("이동할 다음 스테이지 이름")]
    [SerializeField] private string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        //부딪힌 오브젝트가 플레이어 태그를 가졌으면 다음 스테이지 불러오기
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("다음 스테이지 이른이 지정된 적 없네요.");
            }
        }
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("골인 지점 도착! GameManager에게 전송 요청...");
            
            FindAnyObjectByType<GameManager>().NotifyStageClear();
        }
    }
    
    private void OnDrawGizmos()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        
        if (boxCollider != null)
        {
            //박스 색깔
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + boxCollider.center, boxCollider.size);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("골인 지점 도착! GameManager에게 전송 요청...");
            
            FindAnyObjectByType<GameManager>().NotifyStageClear();
        }
    }
}
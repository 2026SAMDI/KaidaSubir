using UnityEngine;

public class Bag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //닿은 오브젝트가 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivateSwordMode(); //검 모드 활성화
                Destroy(gameObject); //삭제
            }
        }
    }
}

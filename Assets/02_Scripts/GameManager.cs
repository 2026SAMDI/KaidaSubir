using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int deathCount = 0; //게임 전체에서 공유되는 데스 수

    public void AddDeath()
    {
        deathCount++;
        Debug.Log("총 데스 수: " + deathCount);
        //여기서 나중에 서버로 데이터를 보낼거
    }
}

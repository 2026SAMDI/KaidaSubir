using UnityEngine;
using UnityEngine.Networking; // 통신을 위해 반드시 추가!
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static string currentUsername = "tester"; 
    
    public static int deathCount = 0; // 게임 전체에서 공유되는 데스 수

    [Header("서버 주소 설정")]
    public string serverBaseUrl = "https://barn-countable-shine.ngrok-free.dev"; 

    public void AddDeath()
    {
        deathCount++;
        Debug.Log("총 데스 수: " + deathCount);
        
        // 데스 수가 증가할 때마다 서버로 전송
        StartCoroutine(SendDeathDataToServer());
    }

    //스테이지 클리어 시 StageTrigger가 이 함수를 호출
    public void NotifyStageClear()
    {
        StartCoroutine(SendDeathDataToServer());
    }

    IEnumerator SendDeathDataToServer()
    {
        string uploadUrl = serverBaseUrl + "/death"; 
        
        WWWForm form = new WWWForm();
        form.AddField("username", currentUsername); 
        form.AddField("deathCount", deathCount);    // 플레이하면서 쌓인 총 데스 수

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uploadUrl, form))
        {
            webRequest.SetRequestHeader("ngrok-skip-browser-warning", "true");
            webRequest.SetRequestHeader("User-Agent", "UnityPlayer");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("★ 기록 전송 대성공! ★");
                
                UnityEngine.SceneManagement.SceneManager.LoadScene("RankingScene"); 
            }
            else
            {
                Debug.LogError("기록 전송 실패: " + webRequest.downloadHandler.text);
            }
        }
    }
}

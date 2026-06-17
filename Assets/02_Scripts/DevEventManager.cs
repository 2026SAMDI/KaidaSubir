using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DevEventManager : MonoBehaviour
{
    private string baseUrl = "https://barn-countable-shine.ngrok-free.dev";
    
    void Start()
    {
        StartCoroutine(CheckForEvents());
    }

    IEnumerator CheckForEvents()
    {
        while (true)
        {
            //서버의 특정 엔드포인트에서 나에게 할당된 명령이 있는지 확인
            string checkUrl = baseUrl + "/dev/check?username=" + GameManager.currentUsername;

            using (UnityWebRequest webRequest = UnityWebRequest.Get(checkUrl))
            {
                webRequest.SetRequestHeader("ngrok-skip-browser-warning", "true");
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string eventType = webRequest.downloadHandler.text;
                    if (!string.IsNullOrEmpty(eventType) && eventType != "NONE")
                    {
                        ProcessEvent(eventType);
                    }
                }
            }
            yield return new WaitForSeconds(0.5f); //0.5초마다 서버 감시
        }
    }

    void ProcessEvent(string eventType)
    {
        Debug.Log("서버에서 받은 명령: " + eventType);
        
        switch (eventType)
        {
            case "SPAWN_WALL":
                //벽 소환 로직
                break;
            case "DISABLE_JUMP":
                //점프 제어 로직
                break;
            case "RANDOM_TELEPORT":
                //텔레포트 로직
                break;
        }
    }
}
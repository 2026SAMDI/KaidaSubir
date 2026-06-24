using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class RankManager : MonoBehaviour
{
    [Header("서버 설정")]
    public string serverBaseUrl = "https://barn-countable-shine.ngrok-free.dev";
    public string rankingEndpoint = "/ranking"; 

    [Header("UI 연결")]
    public TMP_Text rankText; 

    void Start()
    {
        StartCoroutine(GetRankingFromServer());
    }

    IEnumerator GetRankingFromServer()
    {
        string url = serverBaseUrl + rankingEndpoint;
        rankText.text = "랭킹 불러오는 중...";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("ngrok-skip-browser-warning", "true");
            webRequest.SetRequestHeader("User-Agent", "UnityPlayer");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("서버가 준 랭킹 데이터: " + jsonResponse);
                
                DisplayRanking(jsonResponse);
            }
            else
            {
                rankText.text = "랭킹 불러오기 실패!";
                Debug.LogError("랭킹 불러오기 실패: " + webRequest.error);
            }
        }
    }

    void DisplayRanking(string json)
    {
        try
        {
            if (json.StartsWith("[")) {
                json = "{\"rankings\":" + json + "}";
            }

            RankingList rawData = JsonUtility.FromJson<RankingList>(json);
            string displayText = "\n\n";

            int rank = 1;
            foreach (var user in rawData.rankings)
            {
                displayText += $"{rank}등. {user.username} ➔ ({user.deathCount} 데스)\n";
                rank++;
            }
            
            rankText.text = displayText;
        }
        catch (System.Exception e)
        {
            rankText.text = "랭킹 데이터 표시 오류!";
            Debug.LogError("JSON 파싱 에러: " + e.Message);
        }
    }
}

[System.Serializable]
public class RankingData
{
    public string username;
    public int deathCount;
}

[System.Serializable]
public class RankingList
{
    public List<RankingData> rankings;
}
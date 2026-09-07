using UnityEngine;
using UnityEngine.SceneManagement;

public class Next : MonoBehaviour
{
    [Header("씬 이름 접두사")]
    [SerializeField] private string stagePrefix = "Stage";
    
    public void OnClickNextStage()
    {
        //마지막으로 플레이했던 스테이지 번호를 가져옴
        int lastStage = PlayerPrefs.GetInt("LastPlayedStage", 0);

        // 다음 스테이지 번호 계산
        int nextStage = lastStage + 1;

        //자릿수를 2자리로 맞춰서 씬 이름 생성
        string nextSceneName = stagePrefix + nextStage.ToString("00");

        Debug.Log($"다음 스테이지로 이동: {nextSceneName}");

        //다음 스테이지로 씬 전환
        SceneManager.LoadScene(nextSceneName);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTracker : MonoBehaviour
{
    void Start()
    {
        //현재 활성화된 씬 이름 가져오기
        string currentSceneName = SceneManager.GetActiveScene().name;

        //이름이 Stage로 시작하는 경우에만 작동
        if (currentSceneName.StartsWith("Stage"))
        {
            string numberPart = currentSceneName.Replace("Stage", "");

            // 문자열dmf 숫자 변환 성공하면 저장
            if (int.TryParse(numberPart, out int stageIndex))
            {
                PlayerPrefs.SetInt("LastPlayedStage", stageIndex);
                PlayerPrefs.Save();
                Debug.Log($"🎯 현재 스테이지 번호 기록 완료: Stage {stageIndex}");
            }
        }
    }
}

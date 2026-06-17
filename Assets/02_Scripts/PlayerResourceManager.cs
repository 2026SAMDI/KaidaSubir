using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerResourceManager : MonoBehaviour
{
    [Header("매니저 연결")]
    [SerializeField] private UiManager uiManager;
    [SerializeField] private GameManager gameManager;
    
    [Header("초기 아이템 개수")]
    [SerializeField] private int wCount = 20;
    [SerializeField] private int aCount = 20;
    [SerializeField] private int dCount = 20;

    void Start()
    {
        UpdateAllUI();
    }
    
    private void UpdateAllUI()
    {
        if (uiManager != null) {
            uiManager.UpdateItemUI("W", wCount);
            uiManager.UpdateItemUI("A", aCount);
            uiManager.UpdateItemUI("D", dCount);
            uiManager.UpdateItemUI("R", "∞");
        }
    }
    
    public bool HasItem(string itemType)
    {
        if (itemType == "W") return wCount > 0;
        if (itemType == "A") return aCount > 0;
        if (itemType == "D") return dCount > 0;
        return false;
    }
    
    public void UseItem(string itemType)
    {
        if (itemType == "W" && wCount > 0) {
            wCount--;
            if (uiManager != null) uiManager.UpdateItemUI("W", wCount);
            Debug.Log("점프(W) 사용! 남은 횟수: " + wCount);
        }
        else if (itemType == "A" && aCount > 0) {
            aCount--;
            if (uiManager != null) uiManager.UpdateItemUI("A", aCount);
            Debug.Log("왼쪽(A) 사용! 남은 횟수: " + aCount);
        }
        else if (itemType == "D" && dCount > 0) {
            dCount--;
            if (uiManager != null) uiManager.UpdateItemUI("D", dCount);
            Debug.Log("오른쪽(D) 사용! 남은 횟수: " + dCount);
        }
    }

    //사망 처리
    public void Die()
    {
        Debug.Log("사망ㅣ게임오버");
        
        if (gameManager != null) gameManager.AddDeath();
        
        FindAnyObjectByType<BackgroundManager>().ChangeBackgroundOnDeath();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

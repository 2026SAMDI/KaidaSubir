using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [Header("배경 설정")]
    public SpriteRenderer bgRenderer;
    public Sprite[] bgSprites;
    
    private static int currentBgIndex = 0;
    private bool isGameStarted = false;
    private bool isDead = false;
    
    
    private Animator anim;

    void Start()
    {
        if (bgSprites.Length > 0 && bgRenderer != null)
        {
            bgRenderer.sprite = bgSprites[currentBgIndex];
        }
    }
    
    public void ChangeBackgroundOnDeath()
    {
        currentBgIndex++;
        
        if (currentBgIndex >= bgSprites.Length)
        {
            currentBgIndex = 0;
        }
    }
}

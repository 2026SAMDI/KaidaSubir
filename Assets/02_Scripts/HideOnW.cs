using UnityEngine;
using UnityEngine.SceneManagement;

public class HideOnW : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //튜토리얼에만 작동
        if (SceneManager.GetActiveScene().name == "Stage00")
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false; 
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}

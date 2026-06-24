using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerCameraEffects cameraEffects;

    private bool wasGrounded = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cameraEffects = GetComponent<PlayerCameraEffects>();
    }
    
    public void UpdateAnimationState(bool isGrounded, Vector2 velocity, bool hasSwordMode)
    {
        //착지 애니메이션 및 카메라 흔들림 처리
        if (!wasGrounded && isGrounded)
        {
            anim.SetTrigger("Land");
            if (cameraEffects != null)
            {
                cameraEffects.PlayLandShake();
            }
        }
        wasGrounded = isGrounded;

        //스프라이트 좌우 반전 처리
        if (velocity.x < -0.1f)
        {
            sr.flipX = true;  
        }
        else if (velocity.x > 0.1f)
        {
            sr.flipX = false; 
        }

        //애니메이터 파라미터 업데이트
        bool isWalking = Mathf.Abs(velocity.x) > 0.1f;
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("yVelocity", velocity.y);
        anim.SetBool("IsSwordMode", hasSwordMode);
    }

    public void TriggerJump()
    {
        if (anim != null) anim.SetTrigger("Jump");
    }

    public void TriggerDash()
    {
        if (anim != null) anim.SetTrigger("Dash");
    }
}
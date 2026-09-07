using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource), typeof(PlayerController))]
public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private PlayerController playerController;

    [Header("효과음 클립")]
    [SerializeField] private AudioClip jumpSound; //점프 소리
    [SerializeField] private AudioClip dashSound; //대시 소리
    [SerializeField] private AudioClip landSound; //착지 소리
    [SerializeField] private AudioClip swordModeSound; //검 모드 변환 소리

    [Header("볼륨 조절")]
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1.0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
        
        audioSource.playOnAwake = false; 
    }
    
    void OnEnable()
    {
        if (playerController != null)
        {
            playerController.OnJumpEvent += PlayJump;
            playerController.OnDashEvent += PlayDash;
            playerController.OnLandEvent += PlayLand;
            playerController.OnSwordModeEvent += PlaySwordMode;
        }
    }
    
    void OnDisable()
    {
        if (playerController != null)
        {
            playerController.OnJumpEvent -= PlayJump;
            playerController.OnDashEvent -= PlayDash;
            playerController.OnLandEvent -= PlayLand;
            playerController.OnSwordModeEvent -= PlaySwordMode;
        }
    }
    
    private void PlayJump()
    {
        if (jumpSound != null) audioSource.PlayOneShot(jumpSound, volume);
    }

    private void PlayDash()
    {
        if (dashSound != null) audioSource.PlayOneShot(dashSound, volume);
    }

    private void PlayLand()
    {
        if (landSound != null) audioSource.PlayOneShot(landSound, volume);
    }

    private void PlaySwordMode()
    {
        if (swordModeSound != null) audioSource.PlayOneShot(swordModeSound, volume);
    }
}

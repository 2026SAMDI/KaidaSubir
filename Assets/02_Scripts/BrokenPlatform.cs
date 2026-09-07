using System.Collections;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    [Header("발판 붕괴 설정")]
    [SerializeField] private float fallDelay = 1f; 
    [SerializeField] private float shakeIntensity = 0.05f;

    [Header("시각적 오브젝트 연결")]
    [SerializeField] private Transform visualTransform;

    private bool isStepped = false;
    private Vector3 initialVisualPosition;
    
    private MeshRenderer meshRenderer; 
    private Collider col;              

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        col = GetComponent<Collider>();
        
        if (visualTransform == null && transform.childCount > 0)
        {
            visualTransform = transform.GetChild(0);
        }

        if (visualTransform != null)
        {
            initialVisualPosition = visualTransform.localPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isStepped)
        {
            StartCoroutine(CrumbleSequence());
        }
    }

    private IEnumerator CrumbleSequence()
    {
        isStepped = true;

        float elapsed = 0f;
        while (elapsed < fallDelay)
        {
            if (visualTransform != null)
            {
                visualTransform.localPosition = initialVisualPosition + (Random.insideUnitSphere * shakeIntensity);
            }
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        //시간 다 되면 이미지 위치 원상복구 후 물리/렌더러 끄기
        if (visualTransform != null)
        {
            visualTransform.localPosition = initialVisualPosition;
        }

        if (meshRenderer != null) meshRenderer.enabled = false;
        if (col != null) col.enabled = false; 
        
        Debug.Log("발판 무너짐!");
    }
}
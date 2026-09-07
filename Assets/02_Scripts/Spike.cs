using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerResourceManager res = collision.gameObject.GetComponent<PlayerResourceManager>();
            
            if (res != null)
            {
                Debug.Log("가시다!");
                res.Die();
            }
        }
    }
}

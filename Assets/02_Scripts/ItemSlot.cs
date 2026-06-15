using UnityEngine;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public string itemID;
    [SerializeField] private TextMeshProUGUI countText;
    
    public void SetCount(int count)
    {
        if (countText != null)
            countText.text = count.ToString();
    }
}

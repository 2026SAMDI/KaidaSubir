using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("아이템 텍스트 연결")]
    [SerializeField] private TextMeshProUGUI aText;
    [SerializeField] private TextMeshProUGUI dText;
    [SerializeField] private TextMeshProUGUI wText;
    [SerializeField] private TextMeshProUGUI rText;
    
    public void UpdateItemUI(string type, int count)
    {
        switch (type)
        {
            case "A": if (aText != null) aText.text = $"A\n{count}"; break;
            case "D": if (dText != null) dText.text = $"D\n{count}"; break;
            case "W": if (wText != null) wText.text = $"W\n{count}"; break;
            case "R": if (rText != null) rText.text = $"R\n{count}"; break;
        }
    }
    public void UpdateItemUI(string itemName, string textValue)
    {
        if(itemName == "R") {
            rText.text = "R\n" + textValue;
        }
    }
}

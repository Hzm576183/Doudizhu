using UnityEngine;

public class BasicCard : MonoBehaviour
{
    public string cardName = "基础卡牌";
    public int cost = 1;
    
    void Start()
    {
        Debug.Log("基础卡牌创建成功: " + cardName);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("按下空格键，卡牌: " + cardName);
        }
    }
}
using UnityEngine;

public class SimpleCardTest : MonoBehaviour
{
    [Header("简单卡牌测试")]
    public string cardName = "测试卡牌";
    public int cost = 3;
    public int attack = 2;
    public int health = 3;
    
    private void Start()
    {
        Debug.Log($"创建了卡牌: {cardName} - 费用:{cost} 攻击:{attack} 生命:{health}");
    }
    
    private void Update()
    {
        // 按鼠标点击测试
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"点击了卡牌: {cardName}");
        }
    }
    
    private void OnMouseDown()
    {
        Debug.Log($"选中卡牌: {cardName}");
    }
}
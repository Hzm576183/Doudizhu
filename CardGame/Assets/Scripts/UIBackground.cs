using UnityEngine;

public class UIBackground : MonoBehaviour
{
    [Header("背景颜色设置")]
    public Color backgroundColor = Color.blue;
    public bool showBackground = true;
    
    private void OnGUI()
    {
        if (!showBackground) return;
        
        // 获取UI元素的屏幕位置和大小
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null) return;
        
        // 计算屏幕坐标
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        
        // 转换为屏幕坐标
        Vector2 min = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[0]);
        Vector2 max = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[2]);
        
        // Unity GUI坐标系Y轴是反的
        min.y = Screen.height - min.y;
        max.y = Screen.height - max.y;
        
        if (min.y > max.y)
        {
            float temp = min.y;
            min.y = max.y;
            max.y = temp;
        }
        
        // 绘制背景
        Rect backgroundRect = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        
        // 设置颜色
        Color oldColor = GUI.color;
        GUI.color = backgroundColor;
        GUI.DrawTexture(backgroundRect, Texture2D.whiteTexture);
        GUI.color = oldColor;
    }
}
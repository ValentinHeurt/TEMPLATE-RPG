using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToolTipIcon : MonoBehaviour
{
    public RectTransform backgroundRectTransform;
    [SerializeField] Image m_Image;
    RectTransform m_RectTransform;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    public void InitToolTip(Sprite icon, Vector2 verticalOffset, Vector2 horizontalOffset)
    {
        m_Image.sprite = icon;

        m_RectTransform.anchoredPosition = backgroundRectTransform.anchoredPosition - verticalOffset + horizontalOffset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ToolTip : MonoBehaviour
{
    public RectTransform backgroundRectTransform;
    [SerializeField] TextMeshProUGUI m_TextMesh;
    RectTransform m_RectTransform;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    public void InitToolTip(string text, Vector2 verticalOffset, Vector2 horizontalOffset)
    {
        m_TextMesh.SetText(text);
        m_TextMesh.ForceMeshUpdate();

        Vector2 textSize = m_TextMesh.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;

        m_RectTransform.anchoredPosition = backgroundRectTransform.anchoredPosition - verticalOffset + horizontalOffset;
    }
}

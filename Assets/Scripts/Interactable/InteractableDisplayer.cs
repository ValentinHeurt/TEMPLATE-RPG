using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InteractableDisplayer : MonoBehaviour
{
    [SerializeField] LayerMask m_LayerMask;
    [SerializeField] ToolTip m_toolTipPrefab;
    RectTransform m_RectTransform;
    public float verticalPadding;
    public float horizontalPadding;
    List<ToolTip> toolTips = new List<ToolTip>();

    public string[] test;
    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        //ToolTip tp = Instantiate(m_toolTipPrefab, transform);
        //tp.InitToolTip("oui blabla bla oui non d", new Vector2(0,0));
        //Test(test);
    }
    private void OnEnable()
    {
        PlayerController.OnDisplayedInteractableChanged += DisplayInteractable;
    }
    private void OnDisable()
    {
        PlayerController.OnDisplayedInteractableChanged -= DisplayInteractable;
    }

    void DisplayInteractable(Collider[] interactables)
    {
        if (GameManager.Instance.IsPlaying)
        {
            float height = 0;
            float biggestWidth = 0;
            DeleteAllToolTips();
            foreach (Collider interactable in interactables)
            {
                ToolTip tp = Instantiate(m_toolTipPrefab, transform);
                tp.InitToolTip(interactable.GetComponent<Interactable>().ColoredName, new Vector2(0, height + verticalPadding), new Vector2(horizontalPadding, 0));
                height += tp.backgroundRectTransform.sizeDelta.y + (verticalPadding);
                if (biggestWidth < tp.backgroundRectTransform.sizeDelta.x)
                {
                    biggestWidth = tp.backgroundRectTransform.sizeDelta.x;
                }
                toolTips.Add(tp);
            }
            m_RectTransform.sizeDelta = new Vector2(biggestWidth, height);
        }
    }

    void DeleteAllToolTips()
    {
        foreach (ToolTip tp in toolTips)
        {
            Destroy(tp.gameObject);
        }
        toolTips = new List<ToolTip>();
    }
    void Test(string[] interactables)
    {

        float height = 0;
        float biggestWidth = 0;
        foreach (string interactable in interactables)
        {
            ToolTip tp = Instantiate(m_toolTipPrefab, transform);
            tp.InitToolTip(interactable, new Vector2(0, height + verticalPadding), new Vector2(horizontalPadding, 0));
            height += tp.backgroundRectTransform.sizeDelta.y + (verticalPadding);
            if (biggestWidth < tp.backgroundRectTransform.sizeDelta.x)
            {
                biggestWidth = tp.backgroundRectTransform.sizeDelta.x;
            }
        }
        m_RectTransform.sizeDelta = new Vector2(biggestWidth, height);
    }

}

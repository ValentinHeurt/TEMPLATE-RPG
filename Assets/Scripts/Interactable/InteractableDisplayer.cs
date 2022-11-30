using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InteractableDisplayer : MonoBehaviour
{
    [SerializeField] LayerMask m_LayerMask;
    [SerializeField] ToolTip m_toolTipNamePrefab;
    [SerializeField] ToolTipIcon m_toolTipIconPrefab;
    RectTransform m_RectTransform;
    public float verticalPadding;
    public float horizontalPadding;
    List<ToolTip> toolTipsNames = new List<ToolTip>();
    List<ToolTipIcon> toolTipsIcons = new List<ToolTipIcon>();

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
            DeleteAllToolTipsNames();
            DeleteAllToolTipsIcons();
            foreach (Collider interactable in interactables)
            {
                
                Interactable interac = interactable.GetComponent<Interactable>();
                if (interac.displayOption == InteractableDisplayOption.NAME)
                {
                    ToolTip tp = Instantiate(m_toolTipNamePrefab, transform);
                    tp.InitToolTip(interac.ColoredName, new Vector2(0, height + verticalPadding), new Vector2(horizontalPadding, 0));
                    height += tp.backgroundRectTransform.sizeDelta.y + (verticalPadding);
                    if (biggestWidth < tp.backgroundRectTransform.sizeDelta.x)
                    {
                        biggestWidth = tp.backgroundRectTransform.sizeDelta.x;
                    }
                    toolTipsNames.Add(tp);
                }
                if (interac.displayOption == InteractableDisplayOption.ICON)
                {
                    ToolTipIcon tp = Instantiate(m_toolTipIconPrefab, transform);
                    tp.InitToolTip(interac.icon, new Vector2(0, height + verticalPadding), new Vector2(horizontalPadding, 0));
                    height += tp.backgroundRectTransform.sizeDelta.y + (verticalPadding);
                    if (biggestWidth < tp.backgroundRectTransform.sizeDelta.x)
                    {
                        biggestWidth = tp.backgroundRectTransform.sizeDelta.x;
                    }
                    toolTipsIcons.Add(tp);
                }

                

                
            }
            m_RectTransform.sizeDelta = new Vector2(biggestWidth, height);
        }
    }

    void DeleteAllToolTipsNames()
    {
        foreach (ToolTip tp in toolTipsNames)
        {
            Destroy(tp.gameObject);
        }
        toolTipsNames = new List<ToolTip>();
    }
    void DeleteAllToolTipsIcons()
    {
        foreach (ToolTipIcon tp in toolTipsIcons)
        {
            Destroy(tp.gameObject);
        }
        toolTipsIcons = new List<ToolTipIcon>();
    }
    void Test(string[] interactables)
    {

        float height = 0;
        float biggestWidth = 0;
        foreach (string interactable in interactables)
        {
            ToolTip tp = Instantiate(m_toolTipNamePrefab, transform);
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

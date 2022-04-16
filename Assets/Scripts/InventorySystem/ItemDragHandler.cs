using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
[RequireComponent(typeof(CanvasGroup))]
public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected ItemSlotUI itemSlotUI = null;
    [SerializeField] protected ItemEvent onMouseStartHoverItem = null;
    [SerializeField] protected ItemEvent onDoubleClick = null;
    [SerializeField] protected VoidEvent onMouseEndHoverItem = null;
    [SerializeField] protected Canvas mainCanvas = null;
    private CanvasGroup canvasGroup = null;
    private Transform originalParent = null;
    private int originalSortingOrder;
    private bool isHovering = false;
    private bool pointerIsDown = false;
    public bool isDragging = false;
    private int clickCount;
    public ItemSlotUI ItemSlotUI => itemSlotUI;

    private void Start() => canvasGroup = GetComponent<CanvasGroup>();

    private void OnDisable()
    {
        if (isHovering)
        {
            onMouseEndHoverItem.Raise();
            isHovering = false;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("down");
            pointerIsDown = true;
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && pointerIsDown)
        {
            if (!isDragging)
            {
                isDragging = true;
                onMouseEndHoverItem.Raise();
                originalParent = transform.parent;
                transform.SetParent(transform.parent.parent);
                originalSortingOrder = mainCanvas.sortingOrder;
                mainCanvas.sortingOrder = 600;
                canvasGroup.blocksRaycasts = false;
            }
            Vector2 pos = Mouse.current.position.ReadValue();
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && isDragging)
        {
            pointerIsDown = false;
            isDragging = false;
            transform.SetParent(originalParent);
            mainCanvas.sortingOrder = originalSortingOrder;
            transform.localPosition = Vector3.zero;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Mouse.current.leftButton.isPressed)
        {
            onMouseStartHoverItem.Raise(itemSlotUI.SlotItem);
        }
        isHovering = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
        onMouseEndHoverItem.Raise();
        isHovering = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            clickCount = eventData.clickCount;
            Debug.Log("click: " + clickCount);
            if (clickCount == 2)
            {
                itemSlotUI.SlotItem.UseItem(itemSlotUI);
            }
        }
    }

}

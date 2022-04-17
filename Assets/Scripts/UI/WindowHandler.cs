using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class WindowHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    public RectTransform clickArea;
    private bool pointerIsDown = false;
    private Vector3 positionWhenDown;
    private Vector3 localPositionWhenDown;
    private Vector3 centerPoint;
    private Vector3 positionOffset;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 pos = Mouse.current.position.ReadValue();
            Vector3 currentMousePosition = new Vector3(pos.x, pos.y, 0);

            Vector3[] rectClick = new Vector3[4];
            clickArea.GetWorldCorners(rectClick);

            float maxYClick = Mathf.Max(rectClick[0].y, rectClick[1].y, rectClick[2].y, rectClick[3].y);
            float minYClick = Mathf.Min(rectClick[0].y, rectClick[1].y, rectClick[2].y, rectClick[3].y);
            float maxXClick = Mathf.Max(rectClick[0].x, rectClick[1].x, rectClick[2].x, rectClick[3].x);
            float minXClick = Mathf.Min(rectClick[0].x, rectClick[1].x, rectClick[2].x, rectClick[3].x);

            if (currentMousePosition.x > minXClick && currentMousePosition.x < maxXClick && currentMousePosition.y > minYClick && currentMousePosition.y < maxYClick)
            {
                pointerIsDown = true;
                positionWhenDown = new Vector3(eventData.position.x, eventData.position.y, 0);
                positionOffset = positionWhenDown - rectTransform.position;
            }
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        pointerIsDown = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && pointerIsDown)
        {
            Vector2 pos = Mouse.current.position.ReadValue();
            Vector3 currentMousePosition = new Vector3(pos.x, pos.y, 0);
            Vector3 potentialPosition = currentMousePosition - positionOffset;
            Vector3 oldPosition = rectTransform.position;
            float finalX;
            float finalY;
            Vector3[] rect = new Vector3[4];
            rectTransform.position = potentialPosition;
            rectTransform.GetWorldCorners(rect);
            float maxY = Mathf.Max(rect[0].y, rect[1].y, rect[2].y, rect[3].y);
            float minY = Mathf.Min(rect[0].y, rect[1].y, rect[2].y, rect[3].y);
            float maxX = Mathf.Max(rect[0].x, rect[1].x, rect[2].x, rect[3].x);
            float minX = Mathf.Min(rect[0].x, rect[1].x, rect[2].x, rect[3].x);

            float rectWidth = rectTransform.rect.width;
            float rectHeight = rectTransform.rect.height;

            if (maxY - rectHeight < 0 || minY + rectHeight > Screen.height)
            {
                finalY = oldPosition.y;
            }
            else
            {
                finalY = potentialPosition.y;
            }

            if (maxX - rectWidth < 0 || minX + rectWidth > Screen.width)
            {
                finalX = oldPosition.x;
            }
            else
            {
                finalX = potentialPosition.x;
            }

            rectTransform.position = new Vector3(finalX, finalY, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
public class Answer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TextMeshProUGUI text;
    public AnswerData answerData;

    public void SetData(AnswerData answerData)
    {
        this.text.text = answerData.text;
        this.answerData = answerData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = transform.localScale * 1.2f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = transform.localScale / 1.2f;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandleAnswer();
        }
    }

    public void HandleAnswer()
    {
        EventManager.Instance.QueueEvent(new OnAnswerChosen(answerData));
    }

}

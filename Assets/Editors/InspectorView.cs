using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    Editor editor;
    public InspectorView()
    {

    }

    internal void UpdateSelection(DialogueLineView dialogueLineView)
    {

    }
}

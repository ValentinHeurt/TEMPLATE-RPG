using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
public class DialogueLineView : UnityEditor.Experimental.GraphView.Node
{
    public DialogueLine line;
    public Port input;
    public Port output;
    public Port outputDirectLine;
    TextField textField;
    public DialogueLineView(DialogueLine line)
    {
        this.line = line;
        this.title = "Ligne de dialogue";
        this.viewDataKey = line.guid;

        style.left = line.position.x;
        style.top = line.position.y;

        CreateInputPorts();
        CreateOutputPorts();

        Label textLabel = new Label("Text Box");
        mainContainer.Add(textLabel);
        textField = new TextField("");
        textField.value = this.line.line;
        textField.multiline = true;
        textField.RegisterValueChangedCallback(val => this.line.line = val.newValue);
        textField.AddToClassList("TextBox");
        mainContainer.Add(textField);

    }
    private void CreateInputPorts()
    {
        input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(AnswerView));

        if (input != null)
        {
            input.portName = "";
            inputContainer.Add(input);
        }
    }
    private void CreateOutputPorts()
    {
        output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(AnswerView));

        if (output != null)
        {
            output.portName = "Answers";
            outputContainer.Add(output);
        }

        outputDirectLine = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

        if (outputDirectLine != null)
        {
            outputDirectLine.portName = "Direct Next Line";
            outputContainer.Add(outputDirectLine);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);

        line.position.x = newPos.xMin;
        line.position.y = newPos.yMin;

    }

}

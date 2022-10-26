using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
public class AnswerView : UnityEditor.Experimental.GraphView.Node
{

    public AnswerData answer;
    public Port input;
    public Port outputLine;
    public Port outputQuest;
    public Port outputQuestNeeded;

    TextField textField;
    ObjectField questToGive;
    ObjectField questRequired;
    ObjectField itemToGive;
    public AnswerView(AnswerData answer)
    {
        this.answer = answer;
        this.title = "Answer";
        this.viewDataKey = answer.guid;

        style.left = answer.position.x;
        style.top = answer.position.y;

        CreateInputPorts();
        CreateOutputPorts();
        //Label text box
        Label textLabel = new Label("Text Box");
        mainContainer.Add(textLabel);
        //Text box
        textField = new TextField("");
        textField.value = this.answer.text;
        textField.multiline = true;
        textField.RegisterValueChangedCallback(val => this.answer.text = val.newValue);
        mainContainer.Add(textField);

        //Label quest to give
        Label questToGiveLabel = new Label("Quest To Give");
        mainContainer.Add(questToGiveLabel);
        //Quest To Give
        questToGive = new ObjectField
        {
            objectType = typeof(Quest),
            allowSceneObjects = false,
            value = this.answer.questToStart
        };
        questToGive.RegisterValueChangedCallback(val => this.answer.questToStart = val.newValue as Quest);
        mainContainer.Add(questToGive);

        //Label quest required
        Label questRequiredLabel = new Label("Quest Required");
        mainContainer.Add(questRequiredLabel);

        //Quest required
        questRequired = new ObjectField
        {
            objectType = typeof(Quest),
            allowSceneObjects = false,
            value = this.answer.questNeededToDisplay
        };
        questRequired.RegisterValueChangedCallback(val => this.answer.questNeededToDisplay = val.newValue as Quest);
        mainContainer.Add(questRequired);

        //Label item to give
        Label itemToGiveLabel = new Label("Give Item");
        mainContainer.Add(itemToGiveLabel);

        //Item to give
        itemToGive = new ObjectField
        {
            objectType = typeof(Item),
            allowSceneObjects = false,
            value = this.answer.itemToGive
        };

        itemToGive.RegisterValueChangedCallback(val => this.answer.itemToGive = val.newValue as Item);
        mainContainer.Add(itemToGive);

    }

    private void CreateInputPorts()
    {
        input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(DialogueLineView));

        if (input != null)
        {
            input.portName = "";
            inputContainer.Add(input);
        }
    }
    private void CreateOutputPorts()
    {
        outputLine = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(DialogueLineView));

        if (outputLine != null)
        {
            outputLine.portName = "Next Line";
            outputContainer.Add(outputLine);
        }

    }
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);

        answer.position.x = newPos.xMin;
        answer.position.y = newPos.yMin;

    }
}

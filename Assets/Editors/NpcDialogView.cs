using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;

public class NpcDialogView : GraphView
{
    public new class UxmlFactory : UxmlFactory<NpcDialogView, GraphView.UxmlTraits> { }
    public Action<DialogueLineView> OnDialogueSelected;
    Dialogue dialogue;
    public NpcDialogView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editors/NpcDialogEditor.uss");
        styleSheets.Add(styleSheet);
    }

    internal void PopulateView(Dialogue dialogue)
    {
        this.dialogue = dialogue;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements.ToList());
        graphViewChanged += OnGraphViewChanged;
        if (dialogue.dialogueLine != null)
        {
            DialogueLineView d = CreateLineView(dialogue.dialogueLine);
            if (dialogue.dialogueLine.answers != null) CreateRecursiveAnswersViews(dialogue.dialogueLine.answers, d);
            if (dialogue.dialogueLine.directNextLine != null) CreateRecursiveLineViewsFromLine(dialogue.dialogueLine.directNextLine, d);
        }

    }

    void CreateRecursiveAnswersViews(List<AnswerData> answers, DialogueLineView parent)
    {
        answers.ForEach(answer => {
            AnswerView a = CreateAnswerView(answer);
            Edge edge = parent.output.ConnectTo(a.input);
            AddElement(edge);
            if (answer.nextDialogueLine != null)
            {
                if (answer.nextDialogueLine.answers.Count != 0 || answer.nextDialogueLine.directNextLine != null
                    || (answer.nextDialogueLine.line != null))
                {
                    CreateRecursiveLineViews(answer.nextDialogueLine, a);
                }
            }

        });
    }
    void CreateRecursiveLineViews(DialogueLine line, AnswerView parent)
    {
        DialogueLineView d = CreateLineView(line);
        Edge edge = parent.outputLine.ConnectTo(d.input);
        AddElement(edge);
        if (line.answers != null) CreateRecursiveAnswersViews(line.answers, d);
        if (line.directNextLine != null) CreateRecursiveLineViewsFromLine(line.directNextLine, d);
    }

    void CreateRecursiveLineViewsFromLine(DialogueLine line, DialogueLineView lineParent)
    {
        DialogueLineView d = CreateLineView(line);
        Edge edge = lineParent.outputDirectLine.ConnectTo(d.input);
        AddElement(edge);
        if (line.answers != null) CreateRecursiveAnswersViews(line.answers, d);
        if (line.directNextLine != null) CreateRecursiveLineViewsFromLine(line.directNextLine, d);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        //if (graphViewChange.elementsToRemove != null)
        //{
        //    graphViewChange.elementsToRemove.ForEach(elem =>
        //    {
        //        DialogueLineView lineView = elem as DialogueLineView;
        //        if (lineView != null)
        //        {
        //            dialogue.DeleteDialogueLine();
        //        }
        //    });
        //}
        // Creation des edges
        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                // AnswerView --> DialogView
                if (edge.output.node as DialogueLineView != null)
                {
                    if (edge.input.node as AnswerView != null)
                    {
                        (edge.output.node as DialogueLineView).line.answers.Add((edge.input.node as AnswerView).answer);
                    }
                }

                // DialogueView --> AnswerView
                if (edge.output.node as AnswerView != null)
                {
                    if (edge.input.node as DialogueLineView != null)
                    {
                        (edge.output.node as AnswerView).answer.nextDialogueLine = (edge.input.node as DialogueLineView).line;
                    }
                }

                // DialogueView --> DialogueView
                if (edge.output.node as DialogueLineView != null)
                {
                    if (edge.input.node as DialogueLineView != null)
                    {
                        if ((edge.input.node as DialogueLineView).line.directNextLine == null)
                        {
                            (edge.output.node as DialogueLineView).line.directNextLine = (edge.input.node as DialogueLineView).line;
                        }
                    }
                }

            });
        }


        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            evt.menu.AppendAction($"[test] Dialog Line", (a) => CreateLine());
        }
        {
            evt.menu.AppendAction($"[test] Answer", (a) => CreateAnswer());
        }
    }

    void CreateAnswer()
    {
        CreateAnswerView(dialogue.CreateAnswerData());
    }

    AnswerView CreateAnswerView(AnswerData answer)
    {
        AnswerView answerView = new AnswerView(answer);
        AddElement(answerView);
        return answerView;

    }

    void CreateLine()
    {
        if (dialogue.dialogueLine != null)
        {
            CreateLineView(dialogue.CreateDialogueLine());
        }
        else
        {
            CreateLineView(dialogue.CreateFirstDialogueLine());
        }
        
    }

    DialogueLineView CreateLineView(DialogueLine line)
    {
        DialogueLineView lineView = new DialogueLineView(line);
        AddElement(lineView);
        return lineView;
    }
}

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class NpcDialogEditor : EditorWindow
{
    NpcDialogView dialogView;
    InspectorView inspectorView;

    [MenuItem("NpcDialogEditor/Editor")]
    public static void OpenWindow()
    {
        NpcDialogEditor wnd = GetWindow<NpcDialogEditor>();
        wnd.titleContent = new GUIContent("NpcDialogEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;


        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editors/NpcDialogEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editors/NpcDialogEditor.uss");
        root.styleSheets.Add(styleSheet);

        dialogView = root.Q<NpcDialogView>();
        inspectorView = root.Q<InspectorView>();
        dialogView.OnDialogueSelected = OnNodeSelectionChanged;
        OnSelectionChange();

    }

    private void OnSelectionChange()
    {
        Dialogue dialogue = Selection.activeObject as Dialogue;
        if (dialogue)
        {
            dialogView.PopulateView(dialogue);
        }
    }

    void OnNodeSelectionChanged(DialogueLineView dialogueLineView)
    {
        inspectorView.UpdateSelection(dialogueLineView);
    }


}
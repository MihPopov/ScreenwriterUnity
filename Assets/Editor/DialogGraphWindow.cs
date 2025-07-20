using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraphWindow : EditorWindow
{
    private DialogGraphView graphView;

    [MenuItem("Window/Dialog Graph")]
    public static void Open()
    {
        var window = GetWindow<DialogGraphWindow>();
        window.titleContent = new GUIContent("Dialog Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView()
    {
        graphView = new DialogGraphView
        {
            name = "Dialog Graph"
        };
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new VisualElement();
        toolbar.style.flexDirection = FlexDirection.Row;
        toolbar.style.paddingLeft = 10;
        toolbar.style.paddingTop = 4;
        var loadButton = new Button(() => graphView.LoadFromJson())
        {
            text = "Загрузить JSON"
        };
        toolbar.Add(loadButton);
        rootVisualElement.Add(toolbar);
    }
}
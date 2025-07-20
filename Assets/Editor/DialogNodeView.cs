using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

public class DialogNodeView : Node
{
    public Port inputPort;
    public Port outputPort;
    public DialogueNode node;

    public DialogNodeView(DialogueNode node)
    {
        this.node = node;
        this.title = $"ID: {node.id}";
        this.capabilities |= Capabilities.Movable;
        inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        inputPort.portName = "Вход";
        inputContainer.Add(inputPort);
        outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        outputPort.portName = "Выход";
        outputContainer.Add(outputPort);
        Label lineLabel = new Label(node.line)
        {
            style =
            {
                whiteSpace = WhiteSpace.Normal,
                unityTextAlign = TextAnchor.UpperLeft
            }
        };
        mainContainer.Add(lineLabel);
        Vector2 pos = node.meta != null ? new Vector2(node.meta.x, node.meta.y) : Vector2.zero;
        SetPosition(new Rect(pos, new Vector2(300, 150)));
    }
}
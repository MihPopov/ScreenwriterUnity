using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class DialogGraphView : GraphView
{
    public DialogGraphView()
    {
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        Insert(0, new GridBackground());
    }

    public void LoadFromJson()
    {
        TextAsset json = Resources.Load<TextAsset>("dialog_graph");
        if (json == null)
        {
            Debug.LogError("Τΰιλ dialog_graph.json νε νΰιδεν β Resources!");
            return;
        }
        var scenes = JsonUtility.FromJson<MyScenes>("{\"scene\":" + json.text + "}");
        if (scenes == null || scenes.scene.Count == 0) return;
        var nodes = scenes.scene[0].data;
        Dictionary<int, DialogNodeView> nodeViews = new();
        foreach (var node in nodes)
        {
            var nodeView = new DialogNodeView(node);
            nodeViews[node.id] = nodeView;
            AddElement(nodeView);
        }
        foreach (var node in nodes)
        {
            if (!nodeViews.ContainsKey(node.id)) continue;
            var fromNode = nodeViews[node.id];
            foreach (var link in node.to)
            {
                if (!nodeViews.ContainsKey(link.id)) continue;
                var toNode = nodeViews[link.id];
                var edge = fromNode.outputPort.ConnectTo(toNode.inputPort);
                AddElement(edge);
            }
        }
    }
}
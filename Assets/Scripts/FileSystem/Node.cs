using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum NodeT
{
    Folder,
    TextFile,
    Computer,
}

public class Node
{
    public string Name { get; set; }
    public Node Parent { get; set; }
    public NodeT NodeType { get; set; }

    public Node(string name, Node parent, NodeT nodeType)
    {
        Name = name;
        Parent = parent;
        NodeType = nodeType;
    }
}


public class FolderNode : Node
{
    public List<Node> Children { get; private set; } = new List<Node>();

    public FolderNode(string name, Node parent)
        : base(name, parent, NodeT.Folder) 
    {
        if (parent != null)
        {
            var parentNode = new FolderNode(FSConstants.ParentName, null)
            {
                Parent = parent
            };
            Children.Add(parentNode);
        }
    }

    public void AddChild(Node child)
    {
        child.Parent = this;
        Children.Add(child);
    }

    public void RemoveChild(Node child)
    {
        Children.Remove(child);
        child.Parent = null;
    }
}


public class FileNode : Node
{
    public FileNode(string name, Node parent, string content)
        : base(name, parent, NodeT.TextFile)
    {
        Content = content;
    }
    public string Content { get; set; }
}
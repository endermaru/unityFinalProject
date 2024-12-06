using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static Unity.VisualScripting.Metadata;

public enum NodeT
{
    Folder,
    TextFile,
    Computer,
    ZipFile,
    Email,
    Exe,
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
        if (!Children.Contains(child))
        {
            child.Parent = this;
            Children.Add(child);
        }
        
    }

    public void RemoveChild(Node child)
    {
        Children.Remove(child);
        child.Parent = null;
    }
}


public class FileNode : Node
{
    public FileNode(string name, Node parent, string content, string password)
        : base(name, parent, NodeT.TextFile)
    {
        Content = content;
        Password = password;
    }
    public string Content { get; set; }
    public string Password { get; set; }
}

public class ZipNode : Node
{
    public ZipNode(string name, Node parent, FolderNode zipRoot, string password)
        : base(name, parent, NodeT.ZipFile)
    {
        ZipRoot = zipRoot;
        Password = password;
        if (parent != null)
        {
            var parentNode = new FolderNode(FSConstants.ParentName, null)
            {
                Parent = parent
            };
            ZipRoot.Children.Insert(0, parentNode);
        }
    }
    public FolderNode ZipRoot { get; set; }
    public string Password { get; set; }



}
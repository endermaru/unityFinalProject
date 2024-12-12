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
    Image,
    Item,
}

public class Node
{
    public string Name { get; set; }
    public Node Parent { get; set; }
    public NodeT NodeType { get; set; }
    public bool Hidden = false;

    public Node(string name, Node parent, NodeT nodeType, bool hidden)
    {
        Name = name;
        Parent = parent;
        NodeType = nodeType;
        Hidden = hidden;
    }
}


public class FolderNode : Node
{
    public List<Node> Children { get; private set; } = new List<Node>();

    public FolderNode(string name, Node parent, bool hidden=false)
        : base(name, parent, NodeT.Folder, hidden) 
    {
        if (parent != null)
        {
            var parentNode = new FolderNode(FSConstants.ParentName, null)
            {
                Parent = parent
            };
            Children.Add(parentNode);
            Hidden = hidden;
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
    public FileNode(string name, Node parent, string content, bool hidden = false, string password = null, FolderNode zipRoot = null)
        : base(name, parent, GetNodeType(name), hidden)
    {
        Content = content;
        Password = password;
        ZipRoot = zipRoot;

        if (ZipRoot != null && parent != null)
        {
            var parentNode = new FolderNode(FSConstants.ParentName, null)
            {
                Parent = parent
            };
            ZipRoot.Children.Insert(0, parentNode);
        }
        Hidden = hidden;
    }
    public string Content { get; set; }
    public string Password { get; set; }
    public FolderNode ZipRoot { get; set; }
    public string ImagePath { get; set; }

    private static NodeT GetNodeType(string name)
    {
        string ext = name.Length >= 3 ? name[^3..].ToLower() : string.Empty;
        return ext switch
        {
            "zip" => NodeT.ZipFile,
            "exe" => NodeT.Exe,
            "png" or "jpg" => NodeT.Image,
            _ => NodeT.TextFile,
        };
    }
}

public class ItemNode : Node
{
    public ItemNode(string name, Node parent)
        : base(name, parent, NodeT.Item, false)
    {

    }
}
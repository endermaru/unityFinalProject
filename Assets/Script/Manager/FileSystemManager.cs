using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public static class FSConstants
{
    public const string ParentName = "���� ������"; // ���� ���� �̸�
}

public class FileSystemManager : MonoBehaviour
{
    public static FileSystemManager Instance { get; private set; }

    public FolderNode Root { get; private set; }

    public FolderNode CurrentNode { get; private set; }

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFileSystem();
        }
        else
        {
            Destroy(gameObject);
        }

        saveFilePath = Path.Combine(Application.dataPath, "FileSystem.json");  // ���� �߿��� Assets ����
        //saveFilePath = Path.Combine(Application.persistentDataPath, "FileSystem.json"); // ���� �Ŀ��� ���� ���� ���
        LoadFileSystem(); // ���� ���� �� �ε�
        CurrentNode = Root;
        //PrintTree(Root);
    }

    public void PrintTree(Node node, string indent = "")
    {
        Debug.Log($"{indent}{node.Name} ({node.NodeType})");

        if (node is FolderNode folder)
        {
            foreach (var child in folder.Children)
            {
                PrintTree(child, indent + "  ");
            }
        }
    }


    // ���� �ý��� ����
    public void SaveFileSystem()
    {
        try
        {
            string json = JsonConvert.SerializeObject(Root, Formatting.Indented);
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"FileSystem saved to {saveFilePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save FileSystem: {ex.Message}");
        }
    }

    // ���� �ý��� �ε�
    public void LoadFileSystem()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);

                // JSON �����͸� JObject�� �Ľ�
                JObject rootObject = JObject.Parse(json);

                // ��Ʈ ��带 ����
                Root = ParseNode(rootObject, null) as FolderNode;

            }
            else
            {
                //Debug.LogWarning("Save file not found. Initializing new file system.");
                InitializeFileSystem();
            }
        }
        catch (Exception)
        {
            //Debug.LogError($"Failed to load FileSystem: {ex.Message}");
            InitializeFileSystem();
        }
    }

    private Node ParseNode(JObject jsonObject, FolderNode parent)
    {
        string name = jsonObject["Name"]?.ToString();
        string nodeType = jsonObject["NodeType"]?.ToString();

        if (nodeType == "Folder")
        {
            // FolderNode ����
            FolderNode folder = new(name, parent);

            // �ڽ� ó��
            var childrenArray = jsonObject["Children"] as JArray;
            if (childrenArray != null)
            {
                foreach (var child in childrenArray)
                {
                    var childNode = ParseNode(child as JObject, folder);
                    if (childNode != null)
                    {
                        folder.AddChild(childNode);
                    }
                }
            }

            return folder;
        }
        else if (nodeType == "Computer")
        {
            var computer = new Node(name, Root, NodeT.Computer);

            return computer;
        }
        else if (nodeType == "TextFile")
        {
            // FileNode ����
            string content = jsonObject["Content"]?.ToString();
            string password = jsonObject["Password"]?.ToString();
            return new FileNode(name, parent, content, password);
        }
        else if (nodeType == "ZipFile")
        {
            string content = jsonObject["Content"]?.ToString();
            JObject jObj = JObject.Parse(content);
            FolderNode ZipRoot = ParseNode(jObj, null) as FolderNode;

            string password = jsonObject["Password"]?.ToString();
            return new FileNode(name, parent, null, password, ZipRoot);
        }
        else if (nodeType == "Exe")
        {
            return new FileNode(name, parent, null, null);
        }
        else if (nodeType == "Item")
        {
            return new ItemNode(name, parent);
        }
        else if (nodeType == "Image")
        {
            string imagePath = jsonObject["ImagePath"]?.ToString();
            return new FileNode(name, parent, null);
        }

        return null;
    }

    public void ChangeCurrentNode(FolderNode folder)
    {
        if (folder != null && folder.Name == FSConstants.ParentName) CurrentNode = folder.Parent as FolderNode;
        else CurrentNode = folder;
    }

    private void InitializeFileSystem()
    {
        Root = new FolderNode("Root",null);
        CurrentNode = Root;

        var desktopNode = new FolderNode("Desktop", Root);
        var myComputer = new Node("MyComputer", desktopNode, NodeT.Computer);
        desktopNode.AddChild(myComputer);
        Root.AddChild(desktopNode);
    }

    public Node FindNode(string path)
    {
        // ����/���� ��θ� Ž�� (��Ʈ ���� ��� �Է�)
        string[] parts = path.Split('/');
        Node current = Root;

        foreach (string part in parts)
        {
            if (current is FolderNode folder)
            {
                current = folder.Children.Find(c => c.Name == part);
                if (current == null) return null;
            }
            else
            {
                Debug.Log("FAIL!!\n");
                return null;
            }
        }
        return current;
    }


}



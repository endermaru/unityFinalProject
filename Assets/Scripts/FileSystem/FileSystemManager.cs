using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public static class FSConstants
{
    public const string ParentName = ".."; // 상위 폴더 이름
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

        saveFilePath = Path.Combine(Application.dataPath, "FileSystem.json");  // 개발 중에는 Assets 폴더
        //saveFilePath = Path.Combine(Application.persistentDataPath, "FileSystem.json"); // 빌드 후에는 영구 저장 경로
        LoadFileSystem(); // 게임 시작 시 로드
        CurrentNode = Root;
        PrintTree(Root);
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


    // 파일 시스템 저장
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

    // 파일 시스템 로드
    public void LoadFileSystem()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);

                // JSON 데이터를 JObject로 파싱
                JObject rootObject = JObject.Parse(json);

                // 루트 노드를 생성
                Root = ParseNode(rootObject, null) as FolderNode;

                Debug.Log($"FileSystem loaded from {saveFilePath}");
            }
            else
            {
                Debug.LogWarning("Save file not found. Initializing new file system.");
                InitializeFileSystem();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load FileSystem: {ex.Message}");
            InitializeFileSystem();
        }
    }

    private Node ParseNode(JObject jsonObject, FolderNode parent)
    {
        string name = jsonObject["Name"]?.ToString();
        string nodeType = jsonObject["NodeType"]?.ToString();

        if (nodeType == "Folder")
        {
            // FolderNode 생성
            FolderNode folder;
            if (name == "Desktop") folder = new FolderNode(name, null);
            else folder = new FolderNode(name, parent);

            // 자식 처리
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
            var computer = new Node(name, parent, NodeT.Computer);

            return computer;
        }
        else if (nodeType == "TextFile")
        {
            // FileNode 생성
            string content = jsonObject["Content"]?.ToString();
            return new FileNode(name, parent, content);
        }

        return null;
    }

    public void ChangeCurrentNode(string childName)
    {
        var folder = CurrentNode;

        var nextNode = (FolderNode)folder.Children.Find(child => child is FolderNode && child.Name == childName);

        if (nextNode != null)
        {
            if (childName == FSConstants.ParentName) // 부모로 이동
            {
                CurrentNode = (FolderNode)CurrentNode.Parent;
            }
            else // 다른 자식으로 이동
            {
                CurrentNode = nextNode;
            }
        }
        else
        {
            Debug.LogWarning($"No child named '{childName}' found in {folder.Name}.");
        }
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
        // 파일/폴더 경로를 탐색 (루트 기준 경로 입력)
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



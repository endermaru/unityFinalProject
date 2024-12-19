using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public static class FSConstants
{
    public const string ParentName = "[..Parent]"; // 상위 폴더 이름
}

public class FileSystemManager : MonoBehaviour
{
    public static FileSystemManager Instance { get; private set; }

    public FolderNode Root { get; private set; }

    public FolderNode CurrentNode { get; private set; }

    public bool ShowHidden;

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            InitializeFileSystem();
        }
        else
        {
            Destroy(gameObject);
        }

        //saveFilePath = Path.Combine(Application.dataPath, "FileSystem.json");  // 개발 중에는 Assets 폴더
        saveFilePath = Path.Combine(Application.persistentDataPath, "FileSystem.json"); // 빌드 후에는 영구 저장 경로
        LoadFileSystem(); // 게임 시작 시 로드
        CurrentNode = Root;
        //PrintTree(Root);

        ShowHidden = false;
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

    // 파일 시스템 로드
    public void LoadFileSystem()
    {
        try
        {
            string json = Resources.Load<TextAsset>("FileSystem").text;

            // JSON 데이터를 JObject로 파싱
            JObject rootObject = JObject.Parse(json);

            // 루트 노드를 생성
            Root = ParseNode(rootObject, null) as FolderNode;
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
        bool hidden = jsonObject["Hidden"] != null;

        if (nodeType == "Folder")
        {
            // FolderNode 생성
            FolderNode folder = new(name, parent, hidden);

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
            var computer = new Node(name, Root, NodeT.Computer, false);

            return computer;
        }
        else if (nodeType == "TextFile")
        {
            // FileNode 생성
            string content = jsonObject["Content"]?.ToString();
            string password = jsonObject["Password"]?.ToString();
            return new FileNode(name, parent, content, hidden, password);
        }
        else if (nodeType == "ZipFile")
        {
            string content = jsonObject["Content"]?.ToString();
            JObject jObj = JObject.Parse(content);
            FolderNode ZipRoot = ParseNode(jObj, null) as FolderNode;

            string password = jsonObject["Password"]?.ToString();
            return new FileNode(name, parent, null, hidden, password, ZipRoot);
        }
        else if (nodeType == "Exe")
        {
            return new FileNode(name, parent, null, hidden, null);
        }
        else if (nodeType == "Item")
        {
            return new ItemNode(name, parent);
        }
        else if (nodeType == "Image")
        {
            return new FileNode(name, parent, null, hidden);
        }
        else if (nodeType == "Ink")
        {
            string path = jsonObject["Path"]?.ToString();
            return new InkNode(name, parent, false, path);
        }

        return null;
    }

    public void ChangeCurrentNode(FolderNode folder)
    {
        if (folder != null && folder.Name == FSConstants.ParentName) CurrentNode = folder.Parent as FolderNode;
        else CurrentNode = folder;
        if (NodeIconRunner.Instance.getCurrentIndex() > 20 || PlayerInteract.Instance.HasZipper) return;
        string path = GetPath(folder);
        if (path == "Root/Tools/Trap/")
        {
            if (ScenarioManager.Instance.CurrentScene < 4)
                ScenarioManager.Instance.CurrentScene = 4;
            ScenarioManager.Instance.seeZipper = true;
            NodeIconRunner.Instance.RunnerRun(0);
            NodeIconRunner.Instance.RunnerRun(4);
        }
        else if (path == "Root/Tools/Trap/tmp-extra/") NodeIconRunner.Instance.RunnerRun(2);
        else if (path == "Root/Tools/Trap/tmp2/") NodeIconRunner.Instance.RunnerRun(6);
        else if (path == "Root/Tools/Trap/tmp2/tmp2-extra/") NodeIconRunner.Instance.RunnerRun(8);
        else if (path == "Root/Tools/Trap/tmp3/") NodeIconRunner.Instance.RunnerRun(10);
        else if (path == "Root/Tools/Trap/tmp3/tmp3-4/") NodeIconRunner.Instance.RunnerRun(12);
        else if (path == "Root/Tools/Trap/tmp1/") NodeIconRunner.Instance.RunnerRun(14);
        else if (path == "Root/Tools/Trap/tmp1/tmp1-4/") NodeIconRunner.Instance.RunnerRun(16);
        else if (path == "Root/Tools/Trap/tmp1/tmp1-4/tmp1-4-1/")
        {
            NodeIconRunner.Instance.getTrembling();
        }

    }

    private void InitializeFileSystem()
    {
        Root = new FolderNode("Root",null);
        CurrentNode = Root;

        var desktopNode = new FolderNode("Desktop", Root);
        var myComputer = new Node("MyComputer", desktopNode, NodeT.Computer, false);
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

    public string GetPath(FolderNode node)
    {
        string path = "";
        while (node != null)
        {
            if (node.Name != FSConstants.ParentName)
                path = node.Name + "/" + path;
            node = node.Parent as FolderNode;
        }

        return path;
    }


}



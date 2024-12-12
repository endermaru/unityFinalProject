using System.Collections.Generic;
using UnityEngine;

public class TaskbarWindow : WindowComponent
{
    public Transform NodeContainer;
    public GameObject NodeIcon;
    public override string ComponentName => "Taskbar";

    public static TaskbarWindow Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void Display()
    {
        foreach (Transform child in NodeContainer)
        {
            Destroy(child.gameObject); // 자식 객체 삭제
        }

        // 자식 노드 리스트 가져오기
        List<Canvas> activeWindows = WindowManager.Instance.activeWindows;

        // 각 자식 노드에 대해 ui 생성
        foreach (Canvas c in activeWindows)
        {
            WindowComponent windowComponent =  c.GetComponent<WindowComponent>();
            Node icon = windowComponent.ComponentName switch
            {
                "FileExplorer" => FileSystemManager.Instance.CurrentNode,
                "TextEditor" => TextEditor.Instance.currentTextFile,
                "ZipExtractWindow" => ZipExtractWindow.Instance.Node,
                "ImageViewer" => ImageViewer.Instance.ImageNode,
                _ => null,
            };
            if (icon == null) continue;

            // 프리팹 생성
            GameObject nodeObject = Instantiate(NodeIcon, NodeContainer);
            nodeObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // nodeui 초기화
            NodeIcon nodeIcon = nodeObject.GetComponent<NodeIcon>();
            nodeIcon.Initialize(icon, "Taskbar");
            nodeIcon.NameText.text = "";
        }
    }

    void Start()
    {
        Display();
    }
}

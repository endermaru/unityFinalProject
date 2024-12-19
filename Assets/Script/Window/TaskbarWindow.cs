using System.Collections.Generic;
using UnityEngine;

public class TaskbarWindow : WindowComponent
{
    public Transform NodeContainer;
    public GameObject NodeIcon;
    public override string ComponentName => "Taskbar";

    public GameObject Cursor;
    public GameObject Zipper;
    public GameObject Hidden;

    public static TaskbarWindow Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Cursor.SetActive(false);
        Zipper.SetActive(false);
        Hidden.SetActive(false);
    }

    public override void Display()
    {
        foreach (Transform child in NodeContainer)
        {
            Destroy(child.gameObject); // �ڽ� ��ü ����
        }

        // �ڽ� ��� ����Ʈ ��������
        List<Canvas> activeWindows = WindowManager.Instance.activeWindows;

        // �� �ڽ� ��忡 ���� ui ����
        foreach (Canvas c in activeWindows)
        {
            WindowComponent windowComponent =  c.GetComponent<WindowComponent>();
            Node icon = windowComponent.ComponentName switch
            {
                "FileExplorer" => FileSystemManager.Instance.CurrentNode,
                "TextEditor" => TextEditor.Instance.currentTextFile,
                "ImageViewer" => ImageViewer.Instance.ImageNode,
                "ZipExtractWindow" => ZipExtractWindow.Instance.Node,
                "PasswordWindow" => new Node("", null, NodeT.Key, false),
                "AdPopup" => new Node("",null, NodeT.Default, false),
                "ExeWindow" => ExeWindow.Instance.ExeNode,
                _ => null,
            };
            if (icon == null) continue;

            // ������ ����
            GameObject nodeObject = Instantiate(NodeIcon, NodeContainer);
            nodeObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // nodeui �ʱ�ȭ
            NodeIcon nodeIcon = nodeObject.GetComponent<NodeIcon>();
            nodeIcon.Initialize(icon, "Taskbar");
            nodeIcon.NameText.text = "";
        }
    }

    void Start()
    {
        Display();
    }

    private void Update()
    {
        Cursor.SetActive(PlayerInteract.Instance.HasCursor);
        Zipper.SetActive(PlayerInteract.Instance.HasZipper);
        Hidden.SetActive(PlayerInteract.Instance.HasHidden);
    }
}

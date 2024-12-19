using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Collections;
using JetBrains.Annotations;

public class NodeIcon : MonoBehaviour, IComponent
{
    string IComponent.ComponentType => "NodeIcon";

    public void Interact() {
        if (Usage == "Taskbar")
        {
            Canvas c = Node.NodeType switch
            {
                NodeT.Folder => WindowManager.Instance.FileExplorer,
                NodeT.TextFile => WindowManager.Instance.TextEditor,
                NodeT.Image => WindowManager.Instance.ImageViewer,
                _ => null,
            };
            if (c == null) return;
            WindowManager.Instance.OpenWindow(c);
            return;
        }
        if (!PlayerInteract.Instance.HasCursor)
        {
            if (Node.NodeType == NodeT.Item)
            {
                if (Node.Name == "Cursor")
                {
                    PlayerInteract.Instance.HasCursor = true;
                    ScenarioManager.Instance.getCursor = true;
                }
                FolderNode parent = Node.Parent as FolderNode;
                parent.Children.Remove(Node);
                FileExplorer.Instance.Display();
                Desktop.Instance.Display();
            }
            return;
        }
        switch (Node.NodeType)
        {
            case NodeT.Computer:
                FileSystemManager.Instance.ChangeCurrentNode(FileSystemManager.Instance.Root);
                FileExplorer.Instance.Display();
                WindowManager.Instance.OpenWindow(WindowManager.Instance.FileExplorer);
                if (ScenarioManager.Instance.CurrentScene < 3)
                    ScenarioManager.Instance.CurrentScene = 3;
                break;
            case NodeT.Folder:
                FileSystemManager.Instance.ChangeCurrentNode(Node as FolderNode);
                FileExplorer.Instance.Display();
                WindowManager.Instance.OpenWindow(WindowManager.Instance.FileExplorer);
                if (Node.Name == "Secrets")
                {
                    if (ScenarioManager.Instance.CurrentScene < 8)
                        ScenarioManager.Instance.CurrentScene = 8;
                    ScenarioManager.Instance.enterSecrets = true;
                }
                else if (Node.Name == "Escape")
                {
                    ScenarioManager.Instance.enterEscape = true;
                    WindowManager.Instance.OpenWindow(WindowManager.Instance.FinalWindow);
                }
                break;
            case NodeT.TextFile:
                FileNode textNode = Node as FileNode;
                if (textNode.Name == "day1¡á7.txt")
                {
                    ScenarioManager.Instance.openLastMessage = true;
                }
                if (textNode.Password != null)
                {
                    PasswordWindow.Instance.SetFile(textNode);
                    WindowManager.Instance.OpenWindow(WindowManager.Instance.PasswordWindow);
                }
                else
                {
                    TextEditor.Instance.setTextFile(textNode);
                    TextEditor.Instance.Display();
                    WindowManager.Instance.OpenWindow(WindowManager.Instance.TextEditor);
                }
                break;
            case NodeT.ZipFile:
                if (!PlayerInteract.Instance.HasZipper)
                {
                    ScenarioManager.Instance.tryOpenZip = true;
                    return;
                }
                FileNode zipNode = Node as FileNode;
                ZipExtractWindow.Instance.SetFile(zipNode);
                if (zipNode.Password != null)
                {
                    if (ScenarioManager.Instance.CurrentScene <= 6)
                    {
                        ScenarioManager.Instance.CurrentScene = 6;
                        ScenarioManager.Instance.openPassword = true;
                    }
                    PasswordWindow.Instance.SetFile(zipNode);
                    WindowManager.Instance.OpenWindow(WindowManager.Instance.PasswordWindow);
                }
                else
                {
                    WindowManager.Instance.OpenWindow(WindowManager.Instance.ZipExtractWindow);
                }
                break;
            case NodeT.Image:
                FileNode ImageNode = Node as FileNode;
                if (ImageNode.Name == "body.png")
                {
                    ScenarioManager.Instance.openLastPhoto = true;
                }
                ImageViewer.Instance.SetFile(ImageNode);
                ImageViewer.Instance.Display();
                WindowManager.Instance.OpenWindow(WindowManager.Instance.ImageViewer);

                break;
            case NodeT.Item:
                if (Node.Name == "Seeker")
                {
                    PlayerInteract.Instance.HasHidden = true;
                    FileSystemManager.Instance.ShowHidden = true;
                    if (ScenarioManager.Instance.CurrentScene < 7)
                        ScenarioManager.Instance.CurrentScene = 7;
                    ScenarioManager.Instance.getSeeker = true;

                }
                
                // use(delete item)
                FolderNode parent = Node.Parent as FolderNode;
                parent.Children.Remove(Node);
                FileExplorer.Instance.Display();
                Desktop.Instance.Display();

                break;
            case NodeT.Exe:
                ExeWindow.Instance.SetFile(Node as FileNode);
                ExeWindow.Instance.Display();
                WindowManager.Instance.OpenWindow(WindowManager.Instance.ExeWindow);
                break;
            case NodeT.Ink:
                string path = ((InkNode)Node).Path;
                FileSystemManager.Instance.ChangeCurrentNode(FileSystemManager.Instance.FindNode(path) as FolderNode);
                FileExplorer.Instance.Display();
                WindowManager.Instance.OpenWindow(WindowManager.Instance.FileExplorer);
                break;

        }
    }

    public NodeIconManager nodeIconManager;
    public TMP_Text NameText;
    public Image Icon;
    public Image HighlightBox;

    public string Usage;
    public Node Node;
    public GameObject thisIcon;

    public Canvas ThisCanvas;

    public void Initialize(Node node, string usage)
    {
        Node = node;
        Usage = usage;
        NameText.text = Node.Name;
        NameText.color = usage == "Desktop" ? Color.white : Color.black;
        Icon.sprite = nodeIconManager.GetIcon(node);

        if (node.Hidden) Icon.color = new Color(Icon.color.r, Icon.color.g, Icon.color.b, 0.5f);

        HighlightBox.enabled = false;
        ThisCanvas = WindowManager.Instance.WhoseCanvas(thisIcon);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlayerInteract.Instance.HasCursor && Node.NodeType != NodeT.Item) return;

        if (collision.CompareTag("Player") && PlayerInteract.Instance.IsInteractValid(thisIcon))
        {
            HighlightBox.enabled = true;
            if (Node.NodeType==NodeT.Item) PlayerInteract.Instance.ShowMessage("È¹µæ (E)");
            else if (Node.NodeType==NodeT.ZipFile && !PlayerInteract.Instance.HasZipper)
                PlayerInteract.Instance.ShowMessage("¿­ ¼ö ¾øÀ½");
            else PlayerInteract.Instance.ShowMessage("¿­±â (E)");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!PlayerInteract.Instance.HasCursor && Node.NodeType != NodeT.Item) return;

        if (collision.CompareTag("Player"))
        {
            HighlightBox.enabled = false;
            PlayerInteract.Instance.HideMessage();
        }
    }
}

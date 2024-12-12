using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class NodeIcon : MonoBehaviour, IComponent
{
    string IComponent.ComponentType => "NodeIcon";

    public void Interact()
    {
        if (!PlayerInteract.Instance.HasCursor)
        {
            if (Node.NodeType == NodeT.Item)
            {
                if (Node.Name == "Ä¿¼­")
                {
                    PlayerInteract.Instance.HasCursor = true;
                }
                FolderNode parent = Node.Parent as FolderNode;
                parent.Children.Remove(Node);
                FileExplorer.Instance.Display();
                Desktop.Instance.Display();
                ScenarioManager.Instance.StartDialog();
            }
            return;
        }
        switch (Node.NodeType)
        {
            case NodeT.Computer:
                FileSystemManager.Instance.ChangeCurrentNode(FileSystemManager.Instance.Root);
                FileExplorer.Instance.Display();
                WindowManager.Instance.OpenWindow(WindowManager.Instance.FileExplorer);
                break;
            case NodeT.Folder:
                FileSystemManager.Instance.ChangeCurrentNode(Node as FolderNode);
                FileExplorer.Instance.Display();
                WindowManager.Instance.OpenWindow(WindowManager.Instance.FileExplorer);
                break;
            case NodeT.TextFile:
                FileNode textNode = Node as FileNode;
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
                FileNode zipNode = Node as FileNode;
                ZipExtractWindow.Instance.SetFile(zipNode);
                if (zipNode.Password != null)
                {
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
                ImageViewer.Instance.SetFile(ImageNode);
                ImageViewer.Instance.Display();
                WindowManager.Instance.OpenWindow(WindowManager.Instance.ImageViewer);
                break;
            case NodeT.Item:
                if (Node.Name == "¼û±è ÇØÁ¦")
                {
                    FileSystemManager.Instance.ShowHidden = true;
                    
                }
                
                // use(delete item)
                FolderNode parent = Node.Parent as FolderNode;
                parent.Children.Remove(Node);
                FileExplorer.Instance.Display();
                Desktop.Instance.Display();

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

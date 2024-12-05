using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private List<GameObject> collidingObjects = new List<GameObject>();

    private NodeUI currentNodeUI;

    
    private bool IsObjectVisible(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.enabled; // Renderer�� Ȱ�� �������� Ȯ��
        }

        CanvasRenderer canvasRenderer = obj.GetComponent<CanvasRenderer>();
        if (canvasRenderer != null)
        {
            return canvasRenderer.cull == false; // CanvasRenderer�� �������� �ʾҴ��� Ȯ��
        }

        // �������� ������ �׻� "������ ����"���� ����
        return false;
    }

    public void Start()
    {
        collidingObjects.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            List<GameObject> fronts = WindowManager.Instance.FrontObjects(collidingObjects);
            Canvas frontCanvas = fronts[0].GetComponent<Canvas>();
            foreach (GameObject obj in fronts)
            {
                switch (obj.tag)
                {
                    // ����ȭ���� ������
                    case "NodeUI":
                        NodeUIDesktop nodeD = obj.GetComponent<NodeUIDesktop>();
                        switch (nodeD.nodeData.NodeType)
                        {
                            case NodeT.Computer:
                                FileSystemManager.Instance.ChangeCurrentNode(FileSystemManager.Instance.Root);
                                WindowManager.Instance.openWindow(WindowManager.Instance.FileExplorer);
                                FileExplorerNodeManager.Instance.DisplayNodes();
                                break;
                            case NodeT.Folder:
                                FileSystemManager.Instance.ChangeCurrentNode(nodeD.nodeData as FolderNode);
                                WindowManager.Instance.openWindow(WindowManager.Instance.FileExplorer);
                                FileExplorerNodeManager.Instance.DisplayNodes();
                                break;
                            case NodeT.TextFile:
                                FileNode f = nodeD.nodeData as FileNode;
                                if (f.Password != null)
                                {
                                    PassWordManager.Instance.setFile(f);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.PassWordWindow);
                                }
                                else
                                {
                                    TextEditorManager.Instance.setTextFile(nodeD.nodeData as FileNode);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.TextEditor);
                                    TextEditorManager.Instance.Display();
                                }
                                break;
                        }
                        break;

                    // ����Ž������ ������
                    case "NodeUIBlack":
                        NodeUI node = obj.GetComponent<NodeUI>();
                        switch (node.nodeData.NodeType)
                        {
                            case NodeT.Folder:
                                FileSystemManager.Instance.ChangeCurrentNode(node.nodeData as FolderNode);
                                FileExplorerNodeManager.Instance.DisplayNodes();
                                break;
                            case NodeT.TextFile:
                                FileNode f = node.nodeData as FileNode;
                                if (f.Password != null)
                                {
                                    PassWordManager.Instance.setFile(f);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.PassWordWindow);
                                }
                                else
                                {
                                    TextEditorManager.Instance.setTextFile(node.nodeData as FileNode);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.TextEditor);
                                    TextEditorManager.Instance.Display();
                                }
                                break;

                        }
                        break;

                    case "CloseButton":
                        WindowManager.Instance.closeWindow(frontCanvas);
                        break;

                    case "OkButton":
                        
                        
                        if (PassWordManager.Instance.CheckPassword())
                        {
                            FileNode nodePW = PassWordManager.Instance.node;
                            WindowManager.Instance.closeWindow(WindowManager.Instance.PassWordWindow);
                            TextEditorManager.Instance.setTextFile(nodePW);
                            WindowManager.Instance.openWindow(WindowManager.Instance.TextEditor);
                            TextEditorManager.Instance.Display();
                        }
                        else
                        {
                            PassWordManager.Instance.invalid.SetActive(true);
                        }
                        PassWordManager.Instance.ResetContent();
                        break;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹 ���� �� ������Ʈ �߰�
        if (/*IsObjectVisible(other.gameObject) &&*/ 
                !collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Add(other.gameObject);
            //Debug.Log("in:" + other.gameObject.ToString());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // �浹 ���� �� ������Ʈ ����
        if (/*IsObjectVisible(other.gameObject) &&*/
                collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Remove(other.gameObject);
            //Debug.Log("out:" + other.gameObject.ToString());
        }
    }
}

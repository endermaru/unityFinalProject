using Unity.VisualScripting;
using UnityEngine;

public class OKButtonPW : MonoBehaviour, IComponent
{
    public GameObject activeImage;
    public GameObject Window;

    string IComponent.ComponentType => "OKButton";
    public string ComponentName => "OKButtonPW";

    private void Start()
    {
        activeImage.SetActive(false);
    }

    public virtual void Interact()
    {
        if (PasswordWindow.Instance.CheckPassword())
        {
            if (PasswordWindow.Instance.Node?.NodeType == NodeT.TextFile)
            {
                FileNode nodePW = PasswordWindow.Instance.Node as FileNode;
                WindowManager.Instance.CloseWindow(WindowManager.Instance.PasswordWindow);
                TextEditor.Instance.setTextFile(nodePW);
                WindowManager.Instance.OpenWindow(WindowManager.Instance.TextEditor);
                TextEditor.Instance.Display();
            }
            else if (PasswordWindow.Instance.Node?.NodeType == NodeT.ZipFile)
            {
                WindowManager.Instance.CloseWindow(WindowManager.Instance.PasswordWindow);
                WindowManager.Instance.OpenWindow(WindowManager.Instance.ZipExtractWindow);

            }
            PasswordWindow.Instance.resetNode();
        }

        PasswordWindow.Instance.ResetContent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerInteract.Instance.IsInteractValid(Window))
        {
            if (activeImage != null)
            {
                activeImage.SetActive(true); // X �̹��� ǥ��
                PlayerInteract.Instance.ShowMessage("Ȯ�� (E)");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" �±׸� ���� ������Ʈ�� ������ ��� ��
        {
            if (activeImage != null)
            {
                activeImage.SetActive(false); // X �̹��� ����
                PlayerInteract.Instance.HideMessage();
            }
        }
    }
}

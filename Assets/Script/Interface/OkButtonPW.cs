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
                activeImage.SetActive(true); // X 이미지 표시
                PlayerInteract.Instance.ShowMessage("확인 (E)");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그를 가진 오브젝트가 범위를 벗어날 때
        {
            if (activeImage != null)
            {
                activeImage.SetActive(false); // X 이미지 숨김
                PlayerInteract.Instance.HideMessage();
            }
        }
    }
}

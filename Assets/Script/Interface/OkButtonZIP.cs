using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class OkButtonZIP : MonoBehaviour, IComponent
{
    public GameObject activeImage;
    string IComponent.ComponentType => "OKButton";
    public string ComponentName => "OKButtonZIP";

    public GameObject Window;

    private void Start()
    {
        activeImage.SetActive(false);
    }

    public virtual void Interact()
    {
        FileNode ZipFile = ZipExtractWindow.Instance.Node;
        FolderNode parent = ZipFile.Parent as FolderNode;
        parent.AddChild(ZipFile.ZipRoot);
        FileExplorer.Instance.Display();
        Desktop.Instance.Display();
        WindowManager.Instance.CloseWindow(WindowManager.Instance.ZipExtractWindow);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerInteract.Instance.IsInteractValid(Window))
        {
            if (activeImage != null)
            {
                activeImage.SetActive(true);
                PlayerSingleton.Instance.ShowMessage("확인 (E)");
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
                PlayerSingleton.Instance.HideMessage();
            }
        }
    }
}

using TMPro;
using UnityEngine;

public class OkButtonEXE : MonoBehaviour, IComponent
{
    public GameObject activeImage;
    public GameObject Window;
    public TMP_Text text;
    string IComponent.ComponentType => "OKButton";
    public string ComponentName => "OKButtonEXE";


    private void Start()
    {
        activeImage.SetActive(false);
    }

    public virtual void Interact()
    {
        if (ExeWindow.Instance.Status == 0) //0->1
        {
            ExeWindow.Instance.Execute();
        }
        else if (ExeWindow.Instance.Status == 1) //1->2
        {
            ExeWindow.Instance.Cancel();
        }
        else if (ExeWindow.Instance.Status == 2) //2->end
        {
            WindowManager.Instance.CloseWindow(WindowManager.Instance.ExeWindow);
            ExeWindow.Instance.ResetContent();
        }
        else if (ExeWindow.Instance.Status == 3) //3->end
        {
            WindowManager.Instance.CloseWindow(WindowManager.Instance.ExeWindow);
            ExeWindow.Instance.ResetContent();
        }
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

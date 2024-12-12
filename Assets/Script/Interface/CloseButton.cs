using UnityEngine;

public class CloseButton : MonoBehaviour, IComponent
{
    public Canvas canvas;
    public GameObject xImage; // X 이미지 오브젝트 (활성화/비활성화)

    string IComponent.ComponentType => "CloseButton";
    public string ComponentName => "Window";

    private void Start()
    {
        // X 이미지를 초기에는 비활성화
        if (xImage != null)
        {
            xImage.SetActive(false);
        }
    }

    public virtual void Interact()
    {
        
        if (canvas == WindowManager.Instance.PasswordWindow)
        {
            canvas.GetComponent<PasswordWindow>().ResetWindow();
        }

        WindowManager.Instance.CloseWindow(canvas);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerInteract.Instance.IsInteractValid(canvas.gameObject))
        {
            if (xImage != null)
            {
                xImage.SetActive(true); // X 이미지 표시
                PlayerSingleton.Instance.ShowMessage("닫기 (E)");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (xImage != null)
            {
                xImage.SetActive(false); // X 이미지 숨김
                PlayerSingleton.Instance.HideMessage();
            }
        }
    }
}

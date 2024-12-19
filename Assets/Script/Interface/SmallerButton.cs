using UnityEngine;

public class SmallerButton : MonoBehaviour, IComponent
{
    public Canvas canvas;
    public GameObject xImage; // X 이미지 오브젝트 (활성화/비활성화)

    string IComponent.ComponentType => "SmallerButton";
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
        WindowManager.Instance.StopWindow(canvas);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerInteract.Instance.IsInteractValid(canvas.gameObject))
        {
            if (xImage != null)
            {
                xImage.SetActive(true); // X 이미지 표시
                PlayerInteract.Instance.ShowMessage("최소화 (E)");
            }
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (xImage != null)
            {
                xImage.SetActive(false); // X 이미지 숨김
                PlayerInteract.Instance.HideMessage();
            }
        }
    }
}

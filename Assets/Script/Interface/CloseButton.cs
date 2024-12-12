using UnityEngine;

public class CloseButton : MonoBehaviour, IComponent
{
    public Canvas canvas;
    public GameObject xImage; // X �̹��� ������Ʈ (Ȱ��ȭ/��Ȱ��ȭ)

    string IComponent.ComponentType => "CloseButton";
    public string ComponentName => "Window";

    private void Start()
    {
        // X �̹����� �ʱ⿡�� ��Ȱ��ȭ
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
                xImage.SetActive(true); // X �̹��� ǥ��
                PlayerSingleton.Instance.ShowMessage("�ݱ� (E)");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (xImage != null)
            {
                xImage.SetActive(false); // X �̹��� ����
                PlayerSingleton.Instance.HideMessage();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseButtonLast : MonoBehaviour, IComponent
{
    string IComponent.ComponentType => "CloseButtonLast";
    public GameObject xImage;
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
        SceneManager.LoadScene("BeforeGame");

    }
    private void OnMouseDown()
    {
        Interact();
    }

    private void OnMouseEnter()
    {
        if (xImage != null)
        {
            xImage.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (xImage != null)
        {
            xImage.SetActive(false);
        }
    }
}

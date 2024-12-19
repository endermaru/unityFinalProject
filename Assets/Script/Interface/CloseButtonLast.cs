using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseButtonLast : MonoBehaviour, IComponent
{
    string IComponent.ComponentType => "CloseButtonLast";
    public GameObject xImage;
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

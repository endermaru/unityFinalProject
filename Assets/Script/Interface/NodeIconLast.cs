using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class NodeIconLast : MonoBehaviour, IComponent
{
    string IComponent.ComponentType => "NodeIconLast";
    public GameObject TextEditor;
    public GameObject HighlightBox;
    public TMP_Text name;

    void Start()
    {
        if (TextEditor != null)
            TextEditor.SetActive(false);
        HighlightBox.SetActive(false);
    }

    public void Interact()
    {
        if (TextEditor != null)
            TextEditor.SetActive(true);
        else
            if (name.text == "시작하기")
                SceneManager.LoadScene("DesktopScene");
            else
                Application.Quit();
    }

    private void OnMouseDown()
    {
        Interact();
    }

    private void OnMouseEnter()
    {
        if (HighlightBox != null)
        {
            HighlightBox.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (HighlightBox != null)
        {
            HighlightBox.SetActive(false);
        }
    }
}

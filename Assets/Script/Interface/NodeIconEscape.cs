using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Collections;


public class NodeIconEscape : MonoBehaviour, IComponent
{
    string IComponent.ComponentType => "NodeIconEscape";
    public static NodeIconEscape Instance { get; private set; }

    public void Interact()
    {
        FinalWindow.Instance.Done();
    }

    public TMP_Text NameText;
    public Image Icon;
    public Image HighlightBox;
    public Node Node;
    public GameObject thisIcon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        Node = new FileNode("Escape.exe", null, null);
        NameText.text = Node.Name;
        NameText.color = Color.black;
        HighlightBox.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerInteract.Instance.IsInteractValid(thisIcon))
        {
            HighlightBox.enabled = true;
            PlayerInteract.Instance.ShowMessage("È¹µæ (E)");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HighlightBox.enabled = false;
            PlayerInteract.Instance.HideMessage();
        }
    }
}

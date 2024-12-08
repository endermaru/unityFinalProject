using TMPro;
using UnityEngine;
using System.Collections;

public class PasswordWindow : WindowComponent
{
    public static PasswordWindow Instance { get; private set; }
    public override string ComponentName => "PasswordWindow";

    public Node Node;
    public TMP_Text Content;

    public GameObject invalid;
    public float blinkDuration = 1f; // ±ôºýÀÌ´Â ÃÑ ½Ã°£
    public float blinkInterval = 0.2f; // ±ôºýÀÌ´Â °£°Ý

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        invalid.SetActive(false);
    }

    public void SetFile(Node node)
    {
        Node = node;
        ResetContent();
    }
    public void Update()
    {
        if (Content.text.Length < 27)
        {
            for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
            {
                if (Input.GetKeyDown(key))
                {
                    Content.text += (key - KeyCode.Alpha0).ToString();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (Content.text.Length > 0)
            {
                Content.text = Content.text.Substring(0, Content.text.Length - 1);
            }
        }
    }

    public void ResetContent()
    {
        Content.text = string.Empty;
    }

    public bool CheckPassword()
    {
        bool result = false;
        if (Node != null)
        {
            if (Node.NodeType == NodeT.TextFile)
                result = (Content.text == (Node as FileNode).Password);
            else if (Node.NodeType == NodeT.ZipFile)
                result = (Content.text == (Node as ZipNode).Password);
        }
        if (!result) ShowInvalid();
        return result;
    }

    public void ShowInvalid()
    {
        StartCoroutine(BlinkCoroutine());
    }

    public void resetNode()
    {
        Node = null;
    }

    private IEnumerator BlinkCoroutine()
    {
        float endTime = Time.time + blinkDuration;
        bool isVisible = true;

        while (Time.time < endTime)
        {
            isVisible = !isVisible;
            invalid.SetActive(isVisible);
            yield return new WaitForSeconds(blinkInterval);
        }

        invalid.SetActive(true);
    }
    
}

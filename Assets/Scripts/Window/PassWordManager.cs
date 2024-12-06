using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PassWordManager : MonoBehaviour
{
    public static PassWordManager Instance { get; private set; }
    public FileNode node=null;
    public ZipNode nodeZip=null;
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
        else
        {
            Destroy(gameObject);
        }
        invalid.SetActive(false);
    }
    public void setFile(Node f)
    {
        if (f.NodeType == NodeT.TextFile)
        {
            node = f as FileNode;
        }
        else if (f.NodeType == NodeT.ZipFile)
        {
            nodeZip = f as ZipNode;
        }
        
        ResetContent();
        invalid.SetActive(false);
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
        if (node != null)
        {
            result = (Content.text == node.Password);
        }
        else if (nodeZip != null)
        {
            result = (Content.text == nodeZip.Password);
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
        node = null;
        nodeZip = null;
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

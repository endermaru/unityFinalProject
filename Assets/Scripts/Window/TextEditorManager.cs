using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEditorManager : MonoBehaviour
{
    public static TextEditorManager Instance { get; private set; }
    private FileNode currentTextFile = null;
    public TMP_Text FileName;
    public TMP_Text Content;
    public GameObject inactiveHeader;

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
        inactiveHeader.GetComponent<Canvas>().enabled = false;

    }
    public void Display()
    {
        if (currentTextFile.NodeType == NodeT.TextFile) 
        {
            FileName.text = currentTextFile.Name;
            Content.text = currentTextFile.Content;

        }
    }
    public void setTextFile(FileNode f)
    {
        currentTextFile = f;
    }

}

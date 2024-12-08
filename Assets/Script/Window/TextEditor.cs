using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEditor : WindowComponent
{
    public static TextEditor Instance { get; private set; }
    public override string ComponentName => "TextEditor";
    public FileNode currentTextFile = null;
    public TMP_Text FileName;
    public TMP_Text Content;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public override void Display()
    {
        FileName.text = currentTextFile.Name;
        Content.text = currentTextFile.Content;
    }
    public void setTextFile(FileNode file)
    {
        currentTextFile = file;
    }
}

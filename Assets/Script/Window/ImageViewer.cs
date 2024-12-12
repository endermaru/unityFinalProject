using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class ImageViewer : WindowComponent
{
    public static ImageViewer Instance { get; private set; }
    public override string ComponentName => "ImageViewer";
    public FileNode ImageNode = null;
    public TMP_Text FileName;
    public GameObject ImagePlaceHolder;

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
        //string relativePath = ImageNode.ImagePath.Replace("Assets/Resources/", "");
        string relativePath = ImageNode.Name;
        if (relativePath.EndsWith(".png") || relativePath.EndsWith(".jpg"))
        {
            relativePath = relativePath.Substring(0, relativePath.LastIndexOf('.')); // Remove extension
        }

        Texture2D texture = Resources.Load<Texture2D>($"Images/{relativePath}");

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        Image uiImage = ImagePlaceHolder.GetComponent<Image>();
        uiImage.sprite = sprite;
    }

    public void SetFile(FileNode file)
    {
        ImageNode = file;
        FileName.text = file.Name;
    }
}

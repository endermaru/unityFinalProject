using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NodeIconManager", menuName = "Scriptable Objects/NodeIconManager")]
public class NodeIconManager : ScriptableObject
{

    public Sprite DefaultIcon;
    public Sprite FolderIcon;
    public Sprite TextFileIcon;
    public Sprite ComputerIcon;
    public Sprite ZipFileIcon;
    public Sprite EmailIcon;
    public Sprite ExeIcon;
    public Sprite ImageIcon;

    public Sprite CursorIcon;
    public Sprite HiddenIcon;

    public Sprite GetIcon(Node node)
    {
        switch (node.NodeType)
        {
            case NodeT.Folder:
                return FolderIcon;
            case NodeT.TextFile:
                return TextFileIcon;
            case NodeT.Computer:
                return ComputerIcon;
            case NodeT.ZipFile:
                return ZipFileIcon;
            case NodeT.Email:
                return EmailIcon;
            case NodeT.Exe:
                return ExeIcon;
            case NodeT.Item:
                if (node.Name == "Ä¿¼­") return CursorIcon;
                if (node.Name == "¼û±è ÇØÁ¦") return HiddenIcon;
                else return DefaultIcon;
            case NodeT.Image:
                return ImageIcon;
            default:
                return DefaultIcon;
        }
    }
}

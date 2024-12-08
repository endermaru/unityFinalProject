using UnityEditor.PackageManager.UI;
using UnityEngine;

public class WindowComponent: MonoBehaviour, IComponent
{
    public Canvas canvas;
    public GameObject InactiveHeader;

    void Start()
    {
        SetWindowActive(false);
    }

    public virtual void Interact()
    {
        if (ComponentName != "Desktop")
        {
            WindowManager.Instance.OpenWindow(canvas);
        }
    }

    public virtual void SetWindowActive(bool isActive)
    {

        if (InactiveHeader != null)
        {
            ;
            InactiveHeader.SetActive(!isActive);
        }
    }

    string IComponent.ComponentType => "Window";

    public virtual string ComponentName => "Window";

    public virtual void Display() { }
}

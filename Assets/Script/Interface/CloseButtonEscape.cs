using UnityEngine;

public class CloseButtonEscape : CloseButton
{
    public GameObject thisWindow;

    public override void Interact()
    {
        if (thisWindow != null)
        {
            thisWindow.SetActive(false);
        }
    }
}

using UnityEngine;

public class CloseButtonExe : CloseButton
{
    public override void Interact()
    {

        if (canvas == WindowManager.Instance.PasswordWindow)
        {
            canvas.GetComponent<PasswordWindow>().ResetWindow();
        }
        if (ExeWindow.Instance.Status == 1)
        {
            ExeWindow.Instance.Cancel();
        }
        ExeWindow.Instance.ResetContent();  
        WindowManager.Instance.CloseWindow(canvas);
    }
}

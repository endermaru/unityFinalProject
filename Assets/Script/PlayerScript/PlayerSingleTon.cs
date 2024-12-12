using UnityEngine;
using TMPro;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance;
    public GameObject InteractMessage;
    public TMP_Text Message;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        InteractMessage.SetActive(false);
    }
    public void ShowMessage(string message)
    {
        Message.text = message;
        InteractMessage.SetActive(true);
    }
    public void HideMessage() 
    {
        InteractMessage.SetActive(false);
    }
}

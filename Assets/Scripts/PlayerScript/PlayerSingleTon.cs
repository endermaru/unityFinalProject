using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    private static PlayerSingleton instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �÷��̾� ������Ʈ ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �÷��̾� ������Ʈ ����
        }
    }
}

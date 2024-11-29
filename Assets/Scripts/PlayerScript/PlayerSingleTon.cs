using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    private static PlayerSingleton instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 플레이어 오브젝트 유지
        }
        else
        {
            Destroy(gameObject); // 중복된 플레이어 오브젝트 제거
        }
    }
}

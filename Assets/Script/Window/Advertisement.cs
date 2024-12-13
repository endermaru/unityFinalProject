using UnityEngine;
using System.Collections;

public class Advertisement : WindowComponent
{
    public static Advertisement Instance {  get; private set; }
    public override string ComponentName => "Advertisement";

    public GameObject Popup;

    public bool showAd = false;
    public float PopupInterval = 5f;

    private Coroutine popupCoroutine;

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
        showAd = true;
        popupCoroutine = StartCoroutine(PopupCycle());
    }

    private IEnumerator PopupCycle()
    {
        while (showAd)
        {
            // 팝업을 활성화 (이미 활성화되어 있어도 문제 없음)
            Popup.SetActive(true);

            // 활성화 주기 간격
            yield return new WaitForSeconds(PopupInterval);
        }
    }

    private void OnDestroy()
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
        }
    }

    void Start()
    {
        Display();
    }
}

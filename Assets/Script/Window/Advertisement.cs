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
            // �˾��� Ȱ��ȭ (�̹� Ȱ��ȭ�Ǿ� �־ ���� ����)
            Popup.SetActive(true);

            // Ȱ��ȭ �ֱ� ����
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

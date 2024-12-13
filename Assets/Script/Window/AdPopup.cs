using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class AdPopup : WindowComponent
{
    public override string ComponentName => "AdPopup";
    public static AdPopup Instance { get; private set; }
    public GameObject ImagePlaceHolder;
    public TMP_Text FileName;

    private float shakeIntensity = 10f;

    public bool showAd = false;
    public float PopupInterval = 10f;
    private Coroutine popupCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void Display()
    {
        ShowAd();
        showAd = true;
        popupCoroutine = StartCoroutine(PopupCycle());
    }

    public void ShowAd()
    {
        // 광고 텍스트와 효과 설정
        StartCoroutine(AnimateTextColor());
        StartCoroutine(ShakeAdPanel());
    }

    private IEnumerator AnimateTextColor()
    {
        FileName.color = new Color(Random.value, Random.value, Random.value);
        yield return new WaitForSeconds(0.1f); // 빠른 색상 변경
    }

    private IEnumerator ShakeAdPanel()
    {
        Vector3 originalPosition = ImagePlaceHolder.GetComponent<RectTransform>().anchoredPosition;

        while (ImagePlaceHolder.activeSelf)
        {
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);

            ImagePlaceHolder.GetComponent<RectTransform>().anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator PopupCycle()
    {
        while (showAd)
        {
            // 팝업을 활성화 (이미 활성화되어 있어도 문제 없음)
            WindowManager.Instance.OpenWindow(canvas);

            // 활성화 주기 간격
            yield return new WaitForSeconds(PopupInterval);
        }
        yield return null;
    }

    private void OnDestroy()
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
        }
    }
}

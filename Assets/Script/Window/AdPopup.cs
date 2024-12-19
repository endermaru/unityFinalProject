using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class AdPopup : WindowComponent
{
    public override string ComponentName => "AdPopup";
    public static AdPopup Instance { get; private set; }
    public GameObject ImagePlaceHolder;
    public Sprite[] AdImages; // 광고 이미지를 담을 배열
    private Image placeholderImage; // ImagePlaceHolder의 Image 컴포넌트
    public TMP_Text FileName;

    private float shakeIntensity = 5f;

    public bool showAd = false;
    public float PopupInterval = 10f;
    private Coroutine popupCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        placeholderImage = ImagePlaceHolder.GetComponent<Image>();
    }

    public override void Display()
    {
        ShowAd();
        showAd = true;
        popupCoroutine = StartCoroutine(PopupCycle());
    }

    public void ShowAd()
    {
        SetRandomImage();
        StartCoroutine(AnimateTextColor());
        StartCoroutine(ShakeAdPanel());
    }

    //private int idx = 0;
    private void SetRandomImage()
    {
        if (AdImages.Length > 0)
        {
            int randomIndex = Random.Range(0, AdImages.Length);
            //idx = (idx + 1) % 3;
            placeholderImage.sprite = AdImages[randomIndex]; // 랜덤 이미지 설정
        }
    }

    private IEnumerator AnimateTextColor()
    {
        while (ImagePlaceHolder.activeSelf) // 광고 패널이 활성화된 동안 실행
        {
            FileName.color = new Color(Random.value, Random.value, Random.value); // 랜덤 색상
            yield return new WaitForSeconds(0.1f); // 빠른 색상 변경 주기
        }
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
            SetRandomImage();
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

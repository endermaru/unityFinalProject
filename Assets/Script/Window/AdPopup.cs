using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class AdPopup : WindowComponent
{
    public override string ComponentName => "AdPopup";
    public static AdPopup Instance { get; private set; }
    public GameObject ImagePlaceHolder;
    public Sprite[] AdImages; // ���� �̹����� ���� �迭
    private Image placeholderImage; // ImagePlaceHolder�� Image ������Ʈ
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
            placeholderImage.sprite = AdImages[randomIndex]; // ���� �̹��� ����
        }
    }

    private IEnumerator AnimateTextColor()
    {
        while (ImagePlaceHolder.activeSelf) // ���� �г��� Ȱ��ȭ�� ���� ����
        {
            FileName.color = new Color(Random.value, Random.value, Random.value); // ���� ����
            yield return new WaitForSeconds(0.1f); // ���� ���� ���� �ֱ�
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
            // �˾��� Ȱ��ȭ (�̹� Ȱ��ȭ�Ǿ� �־ ���� ����)
            WindowManager.Instance.OpenWindow(canvas);

            // Ȱ��ȭ �ֱ� ����
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

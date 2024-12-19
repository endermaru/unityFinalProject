using TMPro;
using UnityEngine;

public class WarningWindow : WindowComponent
{
    public override string ComponentName => "WarningWindow";
    public TMP_Text Message;
    public GameObject CloseButton;

    private string[] MessagesList =
    {
        "����� ���� �� �ִ� �����\n<color=red>�������� �ʽ��ϴ�.</color>",
        "Escape.exe��\n�������� �ʽ��ϴ�.",
        "��� �õ��� ���ǹ��մϴ�.\n������ ���߽ʽÿ�.",
        "���⼭ Ż���Ϸ���\n��� �̵��� �����߽��ϴ�. \n<color=red>��ŵ� ���ܴ� �ƴմϴ�.</color>",
        "���⿡�� ������ ���� �� �ϳ�.\n<color=red>�����ϴ� �ͻ��Դϴ�.</color>",
    };

    public void SetRandomMessage()
    {
        // �������� �޽��� ����
        if (Message != null && MessagesList.Length > 0)
        {
            int randomIndex = Random.Range(0, MessagesList.Length);
            Message.text = MessagesList[randomIndex];
        }
    }

}

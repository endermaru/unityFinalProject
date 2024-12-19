using TMPro;
using UnityEngine;

public class WarningWindow : WindowComponent
{
    public override string ComponentName => "WarningWindow";
    public TMP_Text Message;
    public GameObject CloseButton;

    private string[] MessagesList =
    {
        "당신이 나갈 수 있는 방법은\n<color=red>존재하지 않습니다.</color>",
        "Escape.exe는\n존재하지 않습니다.",
        "모든 시도는 무의미합니다.\n저항을 멈추십시오.",
        "여기서 탈출하려는\n모든 이들은 실패했습니다. \n<color=red>당신도 예외는 아닙니다.</color>",
        "여기에서 나가는 길은 단 하나.\n<color=red>포기하는 것뿐입니다.</color>",
    };

    public void SetRandomMessage()
    {
        // 랜덤으로 메시지 선택
        if (Message != null && MessagesList.Length > 0)
        {
            int randomIndex = Random.Range(0, MessagesList.Length);
            Message.text = MessagesList[randomIndex];
        }
    }

}

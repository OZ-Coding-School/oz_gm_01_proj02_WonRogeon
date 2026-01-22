using UnityEngine;

public class ElectricDoorInteract : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (MessageUI.Instance.IsShowing)
            return;

        // 1번째 메시지
        MessageUI.Instance.Show(
            "문이 잠겨있다.",
            ShowSecondMessage
        );
    }

    private void ShowSecondMessage()
    {
        // 2번째 메시지
        MessageUI.Instance.Show(
            "좌측의 패널과 연결되어있는 듯 하다."
        );
    }
}

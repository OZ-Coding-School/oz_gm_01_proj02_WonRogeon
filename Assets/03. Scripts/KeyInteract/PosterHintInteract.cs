using UnityEngine;

public class PosterHintInteract : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        // 이미 메시지 떠 있으면 닫기
        if (MessageUI.Instance != null && MessageUI.Instance.IsShowing)
        {
            MessageUI.Instance.Hide();
            return;
        }

        // 최초 상호작용 시 힌트 표시
        
            MessageUI.Instance.Show(
                "Find the one who is different from the others."
            );
        
    }
}

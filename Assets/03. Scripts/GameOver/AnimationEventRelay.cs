using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    public void OnBloodEffectFinished()
    {
        GameOverSequenceController.Instance.OnBloodEffectFinished();
    }

    public void OnAyaDeathAnimationFinished()
    {
        GameOverSequenceController.Instance.OnAyaDeathAnimationFinished();
    }
}

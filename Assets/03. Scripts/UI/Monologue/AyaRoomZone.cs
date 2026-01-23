using System.Collections.Generic;
using UnityEngine;

public class AyaRoomZone : Zone
{
    private bool hasPlayedMonologue;

    [Header("Monologue SFX")]
    [SerializeField] private AudioClip firstMonologueSFX;

    public void ResetMonologue()
    {
        hasPlayedMonologue = false;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (hasPlayedMonologue)
            return;

        hasPlayedMonologue = true;

        // 독백 시작 시 1회 사운드 재생
        if (SoundManager.Instance != null && firstMonologueSFX != null)
        {
            SoundManager.Instance.PlayGameSFXAt(
                Player.Instance.transform.position,
                firstMonologueSFX
            );
        }

        MonologueUI.Instance.ShowSequence(
            new List<MonologueLine>
            {
                new MonologueLine
                {
                    character = "Aya",
                    expression = "Smile",
                    message = "집 분위기가 너무 으스스해."
                },
                new MonologueLine
                {
                    character = "Aya",
                    expression = "Smile",
                    message = "밖에 나가서 확인해봐야겠어."
                }
            }
        );
    }
}

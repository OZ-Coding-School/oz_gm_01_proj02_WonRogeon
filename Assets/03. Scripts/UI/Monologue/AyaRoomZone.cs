using System.Collections.Generic;

public class AyaRoomZone : Zone
{
    private bool hasPlayedMonologue;

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

        MonologueUI.Instance.ShowSequence(
            new List<MonologueLine>
            {
                new MonologueLine {
                    character = "Aya",
                    expression = "Default",
                    message = "집 분위기가 너무 으스스해."
                },
                new MonologueLine {
                    character = "Aya",
                    expression = "Default",
                    message = "밖에 나가서 확인해봐야겠어."
                }
            }
        );
    }
}



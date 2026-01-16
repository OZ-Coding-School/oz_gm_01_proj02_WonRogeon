using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyaRoomZone : Zone
{
    private bool hasPlayedMonologue = false;

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
                    message = "This house feels really creepy."
                },
                new MonologueLine {
                    character = "Aya",
                    expression = "Default",
                    message = "Maybe I should go outside for a bit."
                }
            }
        );
    }
}


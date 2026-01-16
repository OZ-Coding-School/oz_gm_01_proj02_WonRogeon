using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "Monologue/Portrait Database",
    fileName = "CharacterPortraitDatabase"
)]
public class CharacterPortraitDatabase : ScriptableObject
{
    [SerializeField] private List<CharacterPortraitData> portraits;

    public Sprite GetPortrait(string character, string expression)
    {
        foreach (var p in portraits)
        {
            if (p.characterName == character &&
                p.expression == expression)
                return p.portrait;
        }

        return null;
    }
}

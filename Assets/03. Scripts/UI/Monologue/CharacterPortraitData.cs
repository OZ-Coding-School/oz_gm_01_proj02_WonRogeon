using UnityEngine;

[CreateAssetMenu(
    menuName = "Monologue/Character Portrait",
    fileName = "CharacterPortraitData"
)]
public class CharacterPortraitData : ScriptableObject
{
    public string characterName;   // Aya
    public string expression;      // Normal, Fear, Sad
    public Sprite portrait;
}

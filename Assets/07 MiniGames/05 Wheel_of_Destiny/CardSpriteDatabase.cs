
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardSprites", menuName = "Card/CardSprites", order = 0)]
public class CardSpriteDatabase : ScriptableObject
{
    public Sprite backSprite;
    public List<Sprite> sprites;
}

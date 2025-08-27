using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardColors", menuName = "Card/CardColors", order = 0)]
public class CardColorDatabase : ScriptableObject
{
    public List<Color> colors;
}

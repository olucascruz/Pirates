using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameOptions", menuName = "CustomSettings", order = 1)]
public class GameOptions : ScriptableObject
{
    public int timeSection = 1;
    public int timeToSpawnEnemies = 10;
}

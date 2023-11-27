using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{

    [SerializeField] private GameOptions gameOptions;
 


    public void SetTimeSection(int value)
    {
        int valueInSeconds = value * 60;
        gameOptions.timeSection = valueInSeconds;
    }

    public void SetTimeToSpawnEnemiews(int value)
    {
        gameOptions.timeToSpawnEnemies = value;
    }

}

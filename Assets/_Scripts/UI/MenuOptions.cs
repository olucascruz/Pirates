using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MenuOptions : MonoBehaviour
{

    [SerializeField] private GameOptions gameOptions;

    private void Start()
    {
        gameOptions.timeSection = 60;

        gameOptions.timeToSpawnEnemies = 10;
    }

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

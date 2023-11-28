using System.Collections;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    [SerializeField] private Transform[] pointsSpawn;
    [SerializeField] private PoolingHandler poolShipChaser;
    [SerializeField] private PoolingHandler poolShipShooter;
    [Header("scriptable object to define in section X seconds to Enemy Spawns")]
    [SerializeField] private GameOptions gameOptions;

    void Start()
    {
        StartCoroutine(Generate());
    }

    /// <summary>
    /// Spawns enemies in the scene
    /// </summary>
    private IEnumerator Generate()
    {
        //Wait while the game is in the starting state
        while (GameManager.Instance.State == GameState.STARTING)
        {
           yield return new WaitForFixedUpdate();
        }
        while (GameManager.Instance.State == GameState.PLAY) {
            //Check if enemy pools are full
            if (poolShipShooter.IsFilled && poolShipChaser.IsFilled) { 
                yield return new WaitForSeconds(gameOptions.timeToSpawnEnemies);
                for (int i = 0; i < 2; i++)
                {
                    GameObject[] enemy = { poolShipShooter.GetPooledObject(),
                                           poolShipChaser.GetPooledObject() };

                    //One of the four spawn locations is chosen at random
                    int pointSpawnIndex = Random.Range(0, 4);

                    //The type of enemy generated is chosen randomly
                    int typeEnemyIndex = Random.Range(0, 2);

                    GameObject enemyObj = enemy[typeEnemyIndex];
                    enemyObj.transform.position = pointsSpawn[pointSpawnIndex].position;
                    enemyObj.transform.rotation = Quaternion.identity;
                    enemyObj.SetActive(true);
                    enemyObj.GetComponent<ShipEnemy>().OnActivate();
                    yield return new WaitForSeconds(1.5f);
                }
            }
            yield return new WaitForSeconds(1f);

        }
    }
}

using System.Collections;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    [SerializeField] private Transform[] pointsSpawn;
    [SerializeField] private PoolingHandler poolShipChaser;
    [SerializeField] private PoolingHandler poolShipShooter;



    void Start()
    {
        StartCoroutine(Generate());
    }

    
    private IEnumerator Generate()
    {
        while (GameManager.Instance.State == StateGame.PLAY) { 
            if(poolShipShooter.IsOk && poolShipChaser.IsOk) { 
                yield return new WaitForSeconds(10f);
                for (int i = 0; i < 2; i++)
                {
                    GameObject[] enemy = { poolShipShooter.GetPooledObject(),
                                           poolShipChaser.GetPooledObject() };

                    int pointSpawnIndex = Random.Range(0, 4);
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

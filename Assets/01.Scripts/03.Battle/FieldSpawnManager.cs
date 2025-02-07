using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarParu;

public class FieldSpawnManager : MonoBehaviour
{
    public bool onSpawn;

    public Transform enemyZone; // 몬스터가 배치될 parents

    private void Start()
    {
        StartCoroutine(SpawnCycle());
    }

    public void SpawnEnemys(string key)
    {
        int ran = Random.Range(1, 2);
        for(int i = 0; i < ran; i++)
        {
            CharacterController enemy = ParuPool.inst.GetPoolItem<CharacterController>(key);
            enemy.ResetCharacter();
            enemy.transform.SetParent(enemyZone);
            enemy.transform.position = new Vector2(Random.Range(-30, 30), Random.Range(-15, 15));
            enemy.gameObject.SetActive(true);
            enemy.OnStart();
        }
    }

    IEnumerator SpawnCycle()
    {
        while (true)
        {
            if (onSpawn)
            {
                SpawnEnemys("2001");
            }
            yield return new WaitForSeconds(.5f);
        }
    }
}

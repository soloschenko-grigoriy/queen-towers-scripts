using System;
using System.Collections;
using UnityEngine;

namespace GS
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField, Range(0, 100)] private int[] amounts = new int[3]{10, 15, 20};
        [SerializeField, Range(0.1f, 5f)]  private float[] spawnDelays = new float[3]{5, 3, 2};
        
        private Enemy[] _enemies = Array.Empty<Enemy>();
        private bool _shouldSpawn = true;
        private int _spawnedAlready;

        private void Awake()
        {
            var group = new GameObject("Enemy Group").transform;

            var totalAmount = 0;
            foreach (var item in amounts)
            {
                totalAmount += item;
            }
            
            _enemies = new Enemy[totalAmount];
            for (var i = 0; i < totalAmount; i++)
            {
                _enemies[i] = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
                _enemies[i].transform.SetParent(group, true);
                _enemies[i].gameObject.SetActive(false);
            }
        }

        public int GetAmountForThisRound(int wave) => amounts[wave];

        public void Reset()
        {
            _shouldSpawn = false;
            Array.ForEach(_enemies, enemy => enemy.Cleanup());
        }

        public void StartSpawning(Flag nearestFlag, int wave)
        {
            _shouldSpawn = true;
            _spawnedAlready = 0;
            
            StartCoroutine(Spawn(nearestFlag, wave));
        }

        private IEnumerator Spawn(Flag nearestFlag, int wave)
        {
            while (_spawnedAlready < amounts[wave])
            {
                yield return new WaitForSeconds(spawnDelays[wave]);
                // in case level is over while we are spawning
                if (!_shouldSpawn)
                {
                    break;
                }

                _enemies[_spawnedAlready++].Spawn(transform.position, nearestFlag);
            }
        }
    }
}
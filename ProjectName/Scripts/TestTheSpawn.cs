using System.Collections.Generic;
using CompanyNamespace.Spawner;
using UnityEngine;

namespace ProjectName
{
    public class TestTheSpawn : MonoBehaviour
    {
        private readonly Stack<Enemy> EnemyStack = new Stack<Enemy>();
        private IPoolSpawner<Enemy> EnemySpawnerRef;

        private void Start()
         {
             EnemySpawnerRef = FindObjectOfType<EnemySpawner>(); //TODO: replace with IFind DI service finding instead of Unity Find Object 
         }

        private void Update()
        {
            //Simple input as this is an example
            if (Input.GetKeyDown(KeyCode.F))
            {
                Enemy e = EnemySpawnerRef.Spawn();
                EnemyStack.Push(e);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                //Remove last unit created
                if (EnemyStack.Count == 0)
                {
                    Debug.LogWarning("Attempting to remove an Enemy when none exist in the stack.");
                    return;
                }
                Enemy e = EnemyStack.Pop();
                EnemySpawnerRef.ReturnToPool(e);
            }
        }
    }
}
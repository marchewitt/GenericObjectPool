using CompanyNamespace.Spawner;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProjectName
{
    public class EnemySpawner : SpawnerBase<Enemy>
    {
        [SerializeField] private Transform SpawnTransform;

#if UNITY_EDITOR
        protected new void OnValidate()
        {
            base.OnValidate();
            Assert.IsNotNull(SpawnTransform);
        }
#endif

        /// <summary>
        /// Use as if MonoBehavior Awake
        /// </summary>
        protected override void EndOfBaseAwake()
        {
            if (SpawnTransform == null)
            {
                Debug.LogWarning("SpawnTransform should be pre-set in editor inspector but came up null during runtime. Using Spawner's transform to avoid null ref exception.");
                SpawnTransform = transform;
            }
        }

        /// <summary>
        /// Use as if MonoBehavior Start
        /// </summary>
        protected override void EndOfBaseStart()
        {
        }
        
        
        /// <summary>
        /// Simply instantiates the object, will spawn at Spawner's Transform unless over-ride to work differently
        /// </summary>
        /// <returns></returns>
        protected override Enemy CreateNewPooledObject()
        {
            
            return Instantiate(PrefabToSpawn, SpawnTransform.position, SpawnTransform.rotation, SpawnHierarchyParent);
        }
    }
}
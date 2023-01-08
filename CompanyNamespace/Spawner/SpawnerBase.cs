using System;
using System.Collections;
using System.Collections.Generic;
using ProjectName;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace CompanyNamespace.Spawner
{
    
    /// <summary>
    /// The Spawner uses a Unity3D ObjectPool which uses a stack to hold a collection of object instances for reuse
    /// and is not thread-safe. To use simply inherit and apply your prefab class to the generic.
    ///
    /// By default spawns at it's set transform
    ///
    /// From there you can over-ride key functions like Create or
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SpawnerBase<T> : MonoBehaviour, IPoolSpawner<T> where T : MonoBehaviour 
    {
        [Header("Object Spawning Details")]
        [SerializeField] protected T PrefabToSpawn;
        [Tooltip("Optionally assign a parent object in the hierarchy for where this object should spawn into")]
        [SerializeField] protected Transform SpawnHierarchyParent;

        //////
        [Header("Object Pool Settings")]

        [Min(0)]
        [Tooltip("Un-editable after launching so set to upper limit, objects will still be created ")]
        [SerializeField] protected int MaxPoolCount = 100;

        [Min(0)]
        [Tooltip("Sets the default array size of the Pool's Stack, this value should be the expected value used in most scenes to maximize memory savings")]
        [SerializeField] protected int DefaultStackCapacity = 20;
        
        [Min(0)]
        [Tooltip("Set to enforce the amount of objects to initialize in the pool during awake, this should be less then DefaultStackCapacity")]
        [SerializeField] protected int PreInitializedPoolCount = 10;

        [Tooltip("If true the Pool's stack will do a .Contains(element) on releasing to ensure you never add an object to the pool twice. If you can gurantee this never happens you can save a little speed having this as false")]
        [SerializeField] protected bool CollectionCheck = false;
        

        /// <summary>Unity3D ObjectPool, private to enforce leveraging interfaces</summary>
        private ObjectPool<T> Pool;

        public event Func<T> OnCreate; 
        public event Action<T> OnPoolGet;
        public event Action<T> OnPoolRelease;
        public event Action<T> OnPoolDestroy;
#if UNITY_EDITOR
        protected void OnValidate()
        {
            Assert.IsTrue(PreInitializedPoolCount <= DefaultStackCapacity, "Enforced pre-initialization capacity is greater then set DefaultStackCapacity, this is causing an inefficient re-size of the stack array");
        }
#endif

        private void Awake()
        {
            PrePoolCreationEventWiringOnAwake();
            Pool = CreatePoolOnAwake();
            if (PreInitializedPoolCount != 0) {InitializePoolWithObjects(Pool, PreInitializedPoolCount);}
            EndOfBaseAwake();
        }
        
        
        /// <summary>Used to configure the events handed off to the Unity ObjectPool, rarely will you want to over-ride this but left option open</summary>
        protected virtual void PrePoolCreationEventWiringOnAwake()
        {
            if (PrefabToSpawn == null)
            {
                Debug.LogError($"No prefab setup in Spawner, expecting PrefabToSpawn to contain an object of type:{typeof(T)}");
            }
            if (SpawnHierarchyParent == null)
            {
                Debug.LogWarning("SpawnHierarchyParent is null, setting it so objects will spawn under this spawner's transform");
                SpawnHierarchyParent = this.transform;
            };
            
            //Ties the protected functions to our public events,
            //this way we can have events publicly listened to,
            //but functionality not publicly called
            OnCreate += CreateNewPooledObject;
            OnPoolGet += SetupPooledObject;
            OnPoolDestroy += DestroyPooledObject;
            OnPoolRelease += CleanUpPooledObject;
        }

        /// <summary>Creates a new pool and configures the public Actions into Unity's Pool</summary>
        protected virtual ObjectPool<T> CreatePoolOnAwake()
        {
            return new ObjectPool<T>(
                OnCreate,
                OnPoolGet,
                OnPoolRelease,
                OnPoolDestroy,
                CollectionCheck,
                DefaultStackCapacity,
                MaxPoolCount
            );
        }

        /// <summary>
        /// Initializes the pool
        /// </summary>
        /// <param name="pool"></param>
        /// <typeparam name="T"></typeparam>
        protected void InitializePoolWithObjects<T>(ObjectPool<T> pool, int count) where T : MonoBehaviour
        {
            
            
            T[] obj_array = new T[count];
            //Create all pre-initialized objects through the pool, caching their reference
            for (int i = 0; i < count; i++)
            {
                obj_array[i] = pool.Get();
            }

            //Return to pool to cleanup/disable them
            for (int i = (count-1); i >= 0; i--) //Marc: Doing in reverse for editor hierarchy visualization sake due to being a stack 
            {
                pool.Release(obj_array[i]);
            }
        }

        /// <summary>Called on last line of Awake, use as if normal Awake</summary>
        protected abstract void EndOfBaseAwake();

        private void Start()
        {
            EndOfBaseStart();
        }
        /// <summary> Called at the end of the base Start() call allowing inherited mono-behaviors to override this to treat it like Start</summary>
        protected abstract void EndOfBaseStart();


        int IPoolSpawner<T>.GetCount() => Pool.CountActive;
        int IPoolSpawner<T>.GetPoolInactiveCount() => Pool.CountInactive;
        int IPoolSpawner<T>.GetPoolTotalCount() => Pool.CountAll;
        T IPoolSpawner<T>.Spawn() { return Pool.Get(); }
        void IPoolSpawner<T>.ReturnToPool(T objectToReturnToPool) => Pool.Release(objectToReturnToPool);
        void IPoolSpawner<T>.DestroyPool() => Pool.Clear();

        
        /// <summary>
        /// Simply instantiates the object, will spawn at Spawner's Transform unless over-ride to work differently
        /// </summary>
        /// <returns></returns>
        protected virtual T CreateNewPooledObject()
        {
            return Instantiate(PrefabToSpawn, transform.position, transform.rotation, SpawnHierarchyParent);
        }

        /// <summary>
        /// Simply enables the object
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void SetupPooledObject(T obj) => obj.gameObject.SetActive(true);

        /// <summary>
        /// Simply disables the object
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void CleanUpPooledObject(T obj) => obj.gameObject.SetActive(false);
        

        /// <summary>
        /// Simply calls Destroy on the objet
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void DestroyPooledObject(T obj) => Destroy(obj);
        
    }
}

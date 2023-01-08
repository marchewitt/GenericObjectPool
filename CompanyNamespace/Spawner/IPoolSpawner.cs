using UnityEngine;

namespace CompanyNamespace.Spawner
{
    public interface IPoolSpawner<T> where T : MonoBehaviour
    {
        
        /// <summary>Gets an object from teh pool or Instantiates if needed, then runs a setup</summary>
        T Spawn();
        /// <summary>Releases an object to the pool, triggering a cleanup </summary>
        /// <param name="objectToReturnToPool"></param>
        void ReturnToPool(T objectToReturnToPool);
        
        /// <summary>Returns count of all actively spawned entities from the pool</summary>
        int GetCount();
        /// <summary>Returns count of how many objects remain in the pool</summary>
        int GetPoolInactiveCount();
        /// <summary>Returns count of combined active and inactive entities in pool</summary>
        int GetPoolTotalCount();
        /// <summary>Calls Destroy() on all pool game objects</summary>
        void DestroyPool();
    }
}
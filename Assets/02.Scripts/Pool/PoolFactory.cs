using System.Collections.Generic;
using UnityEngine;

public class PoolFactory : MonoBehaviour
{
    private sealed class Pool
    {
        public readonly Stack<GameObject> Inactive = new();
    }

    private readonly Dictionary<GameObject, Pool> _pools = new();
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new();
    private readonly HashSet<GameObject> _activeInstances = new();

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError($"[{name}] 생성할 프리팹이 비어 있습니다.");
            return null;
        }

        if (!_pools.TryGetValue(prefab, out Pool pool))
        {
            pool = new Pool();
            _pools.Add(prefab, pool);
        }

        GameObject instance = pool.Inactive.Count > 0
            ? pool.Inactive.Pop()
            : Instantiate(prefab);

        instance.transform.SetPositionAndRotation(position, rotation);
        instance.transform.SetParent(transform);
        instance.SetActive(true);

        _instanceToPrefab[instance] = prefab;
        _activeInstances.Add(instance);

        IPoolable poolable = FindPoolable(instance);
        if (poolable != null)
        {
            poolable.Spawn();
        }

        return instance;
    }

    public T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        return Get(prefab.gameObject, position, rotation).GetComponent<T>();
    }

    public void Release(GameObject instance)
    {
        if (instance == null)
        {
            return;
        }

        if (!_instanceToPrefab.TryGetValue(instance, out GameObject prefab) ||
            !_pools.TryGetValue(prefab, out Pool pool))
        {
            _instanceToPrefab.Remove(instance);
            _activeInstances.Remove(instance);
            Destroy(instance);
            return;
        }

        IPoolable poolable = FindPoolable(instance);
        if (poolable != null)
        {
            poolable.Despawn();
        }

        _activeInstances.Remove(instance);
        instance.SetActive(false);
        instance.transform.SetParent(transform);
        pool.Inactive.Push(instance);
    }

    public void Release(IPoolable poolable)
    {
        if (poolable is Component component)
        {
            Release(component.gameObject);
        }
    }
    
    public void Clear(GameObject prefab)
    {
        if (prefab == null || !_pools.Remove(prefab, out Pool pool))
        {
            return;
        }

        while (pool.Inactive.Count > 0)
        {
            GameObject instance = pool.Inactive.Pop();
            _instanceToPrefab.Remove(instance);
            Destroy(instance);
        }
    }

    public void ClearUnusedPools()
    {
        List<GameObject> unusedPrefabs = new();

        foreach (KeyValuePair<GameObject, Pool> pair in _pools)
        {
            bool hasActiveInstance = false;

            foreach (KeyValuePair<GameObject, GameObject> instance in _instanceToPrefab)
            {
                if (instance.Value == pair.Key && _activeInstances.Contains(instance.Key))
                {
                    hasActiveInstance = true;
                    break;
                }
            }

            if (!hasActiveInstance)
            {
                unusedPrefabs.Add(pair.Key);
            }
        }

        foreach (GameObject prefab in unusedPrefabs)
        {
            Clear(prefab);
        }
    }

    public void ClearAll()
    {
        List<GameObject> prefabs = new(_pools.Keys);

        foreach (GameObject prefab in prefabs)
        {
            Clear(prefab);
        }
    }

    private static IPoolable FindPoolable(GameObject instance)
    {
        foreach (MonoBehaviour component in instance.GetComponents<MonoBehaviour>())
        {
            if (component is IPoolable poolable)
            {
                return poolable;
            }
        }

        return null;
    }
}

using System;
using UnityEngine;
using UnityEngine.Pool;

abstract public class Spawner : MonoBehaviour
{
    [SerializeField] private TemporaryObject _prefab;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<TemporaryObject> _pool;
    private int _objectsSpawnedCount = 0;
    private int _objectsCreatedCount = 0;
    private int _objectsActiveCount = 0;

    protected Vector3 SpawnPosition = Vector3.zero;

    public event Action<TemporaryObject> Expired;
    public event Action<int, int, int> UpdatedStats;

    private void Awake()
    {
        _pool = new ObjectPool<TemporaryObject>(
            createFunc: () => CreateObject(),
            actionOnGet: (temporaryObject) => ActivateObject(temporaryObject),
            actionOnRelease: (temporaryObject) => DeactivateObject(temporaryObject),
            actionOnDestroy: (temporaryObject) => DestroyObject(temporaryObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    virtual protected void GetObject()
    {
        _pool.Get();
    }

    private void OnExpired(TemporaryObject temporaryObject)
    {
        _pool.Release(temporaryObject);
        Expired?.Invoke(temporaryObject);
    }

    private void ActivateObject(TemporaryObject temporaryObject)
    {
        UpdatedStats?.Invoke(++_objectsSpawnedCount, ++_objectsActiveCount, _objectsCreatedCount);

        temporaryObject.transform.position = SpawnPosition;
        temporaryObject.gameObject.SetActive(true);
        temporaryObject.Expired += OnExpired;
    }

    private void DeactivateObject(TemporaryObject temporaryObject)
    {
        UpdatedStats?.Invoke(_objectsSpawnedCount, --_objectsActiveCount, _objectsCreatedCount);

        temporaryObject.Expired -= OnExpired;
        temporaryObject.gameObject.SetActive(false);
    }

    private TemporaryObject CreateObject()
    {
        UpdatedStats?.Invoke(_objectsSpawnedCount, _objectsActiveCount, ++_objectsCreatedCount);

        return Instantiate(_prefab);
    }

    private void DestroyObject(TemporaryObject temporaryObject)
    {
        UpdatedStats?.Invoke(_objectsSpawnedCount, --_objectsActiveCount, _objectsCreatedCount);

        Destroy(temporaryObject);
    }
}

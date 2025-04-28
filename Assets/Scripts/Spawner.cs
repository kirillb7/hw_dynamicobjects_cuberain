using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private float _spawnY = 20f;
    [SerializeField] private float _minSpawnX = -10f;
    [SerializeField] private float _maxSpawnX = 10f;
    [SerializeField] private float _minSpawnZ = -10f;
    [SerializeField] private float _maxSpawnZ = 10f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<Cube> _pool;
    private List<Cube> _subscribedCubes = new List<Cube>();

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => ActionOnRelease(cube),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void OnDisable()
    {
        for (int i = _subscribedCubes.Count; i > 0; i--)
        {
            UnsubscribeFromCube(_subscribedCubes[i - 1]);
        }
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = new Vector3(Random.Range(_minSpawnX, _maxSpawnX), _spawnY, Random.Range(_minSpawnZ, _maxSpawnZ));
        cube.Rigidbody.velocity = Vector3.zero;
        cube.gameObject.SetActive(true);
        cube.ResetExpiration();

        SubscribeToCube(cube);
    }

    private void ActionOnRelease(Cube cube)
    {
        UnsubscribeFromCube(cube);
        cube.gameObject.SetActive(false);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void SubscribeToCube(Cube cube)
    {
        cube.Expired += _pool.Release;
        _subscribedCubes.Add(cube);
    }

    private void UnsubscribeFromCube(Cube cube)
    {
        cube.Expired -= _pool.Release;
        _subscribedCubes.Remove(cube);
    }
}

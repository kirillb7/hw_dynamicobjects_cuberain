using System.Collections;
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
    private bool _isRaining = true;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => ActivateCube(cube),
            actionOnRelease: (cube) => DeactivateCube(cube),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(RainCubes());
    }

    private void ActivateCube(Cube cube)
    {
        cube.transform.position = new Vector3(Random.Range(_minSpawnX, _maxSpawnX), _spawnY, Random.Range(_minSpawnZ, _maxSpawnZ));
        cube.Rigidbody.velocity = Vector3.zero;
        cube.gameObject.SetActive(true);
        cube.ResetExpiration();
        cube.Expired += _pool.Release;
    }

    private void DeactivateCube(Cube cube)
    {
        cube.Expired -= _pool.Release;
        cube.gameObject.SetActive(false);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private IEnumerator RainCubes()
    {
        float elapsedTime = 0;

        while (_isRaining)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= _repeatRate)
            {
                elapsedTime = 0;
                GetCube();
            }

            yield return null;
        }
    }
}

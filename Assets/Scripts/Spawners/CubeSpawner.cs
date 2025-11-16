using System.Collections;
using UnityEngine;

public class CubeSpawner : Spawner
{
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private float _minSpawnX = -10f;
    [SerializeField] private float _maxSpawnX = 10f;
    [SerializeField] private float _minSpawnY = 20f;
    [SerializeField] private float _maxSpawnY = 25f;
    [SerializeField] private float _minSpawnZ = -10f;
    [SerializeField] private float _maxSpawnZ = 10f;

    private bool _isRaining = true;

    private void Start()
    {
        StartCoroutine(Rain());
    }

    protected override void GetObject()
    {
        SpawnPosition.x = Random.Range(_minSpawnX, _maxSpawnX);
        SpawnPosition.y = Random.Range(_minSpawnY, _maxSpawnY);
        SpawnPosition.z = Random.Range(_minSpawnZ, _maxSpawnZ);

        base.GetObject();
    }

    private IEnumerator Rain()
    {
        float elapsedTime = 0;

        while (_isRaining)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= _repeatRate)
            {
                elapsedTime = 0;
                GetObject();
            }

            yield return null;
        }
    }
}

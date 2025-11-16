using UnityEngine;

public class BombSpawner : Spawner
{
    [SerializeField] private Spawner _otherSpawner;

    private void OnEnable()
    {
        _otherSpawner.Expired += OnExpired;
    }

    private void OnDisable()
    {
        _otherSpawner.Expired -= OnExpired;
    }

    private void OnExpired(TemporaryObject temporaryObject)
    {
        SpawnPosition = temporaryObject.transform.position;
        GetObject();
    }
}

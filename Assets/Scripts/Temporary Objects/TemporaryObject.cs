using System;
using System.Collections;
using UnityEngine;

abstract public class TemporaryObject : MonoBehaviour
{
    [SerializeField] protected float MinExpirationTime = 2;
    [SerializeField] protected float MaxExpirationTime = 5;

    protected float ExpirationProgress = 0;
    protected float ExpirationTime;
    protected bool IsExpiring = false;
    protected Coroutine Coroutine;

    public event Action<TemporaryObject> Expired;

    virtual protected void ResetExpiration()
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }

        ExpirationProgress = 0;
        IsExpiring = false;
    }

    virtual protected void StartExpiration()
    {
        ExpirationTime = UnityEngine.Random.Range(MinExpirationTime, MaxExpirationTime);

        ResetExpiration();
        IsExpiring = true;
        Coroutine = StartCoroutine(Expire());
    }

    virtual protected void FinishExpiration()
    {
        Expired?.Invoke(this);
    }

    private IEnumerator Expire()
    {
        while (ExpirationProgress < ExpirationTime)
        {
            yield return null;

            ExpirationProgress += Time.deltaTime;
            Tick();
        }

        FinishExpiration();
    }

    abstract protected void Tick();
}

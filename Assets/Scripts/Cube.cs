using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private float _minExpirationTime = 2;
    [SerializeField] private float _maxExpirationTime = 5;
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _expiringColor = Color.red;

    private bool _isExpiring = false;
    private Renderer _renderer;

    public Rigidbody Rigidbody { get; private set; }

    public event Action<Cube> Expired;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isExpiring == false)
        {
            if (collision.gameObject.GetComponent<Platform>())
            {
                _isExpiring = true;
                UpdateColor();
                StartCoroutine(Expire());
            }
        }
    }

    private IEnumerator Expire()
    {
        float expirationTime = UnityEngine.Random.Range(_minExpirationTime, _maxExpirationTime);
        float elapsedTime = 0;

        while (elapsedTime < expirationTime)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Expired?.Invoke(this);
    }

    private void UpdateColor()
    {
        if (_isExpiring)
        {
            _renderer.material.color = _expiringColor;
        }
        else
        {
            _renderer.material.color = _defaultColor;
        }
    }

    public void ResetExpiration()
    {
        _isExpiring = false;
        UpdateColor();
    }
}

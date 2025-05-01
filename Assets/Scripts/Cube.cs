using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _minExpirationTime = 2;
    [SerializeField] private float _maxExpirationTime = 5;
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _expiringColor = Color.red;

    private bool _isExpiring = false;
    private Renderer _renderer;

    public event Action<Cube> Expired;

    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isExpiring == false)
        {
            if (collision.gameObject.TryGetComponent<Platform>(out _))
            {
                _isExpiring = true;
                UpdateColor();
                Invoke(nameof(Expire), UnityEngine.Random.Range(_minExpirationTime, _maxExpirationTime));
            }
        }
    }

    public void ResetExpiration()
    {
        _isExpiring = false;
        UpdateColor();
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

    private void Expire()
    {
        Expired?.Invoke(this);
    }
}

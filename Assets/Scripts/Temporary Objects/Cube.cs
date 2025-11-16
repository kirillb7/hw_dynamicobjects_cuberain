using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Cube : TemporaryObject
{
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _expiringColor = Color.red;

    private Renderer _renderer;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        ResetExpiration();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsExpiring == false)
        {
            if (collision.gameObject.TryGetComponent<Platform>(out _))
            {
                StartExpiration();
            }
        }
    }

    protected override void ResetExpiration()
    {
        base.ResetExpiration();
        UpdateColor();
        _rigidbody.velocity = Vector3.zero;
    }

    protected override void StartExpiration()
    {
        base.StartExpiration();
        UpdateColor();
    }

    protected override void Tick()
    {
        
    }

    private void UpdateColor()
    {
        if (IsExpiring)
        {
            _renderer.material.color = _expiringColor;
        }
        else
        {
            _renderer.material.color = _defaultColor;
        }
    }
}

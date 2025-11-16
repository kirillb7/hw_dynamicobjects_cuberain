using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Exploder))]
public class Bomb : TemporaryObject
{
    private Color _color;
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Exploder _exploder;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _exploder = GetComponent<Exploder>();
    }

    private void OnEnable()
    {
        _color = _renderer.material.color;
        StartExpiration();
    }

    protected override void ResetExpiration()
    {
        base.ResetExpiration();
        _color.a = 1;
        _rigidbody.velocity = Vector3.zero;
    }

    protected override void FinishExpiration()
    {
        _exploder.Explode();
        base.FinishExpiration();
    }

    protected override void Tick()
    {
        _color.a = Mathf.Lerp(1, 0, ExpirationProgress / ExpirationTime);
        _renderer.material.color = _color;
    }
}

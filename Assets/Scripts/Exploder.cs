using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _explosionForce = 300;
    [SerializeField] private float _explosionRadius = 15;

    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in hits)
        {
            Rigidbody body = hit.attachedRigidbody;

            if (body != null)
            {
                body.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
    }
}

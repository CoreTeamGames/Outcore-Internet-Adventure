using UnityEngine;

public class Damager : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable _damagable))
            _damagable.ApplyDamage(1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSplash : MonoBehaviour
{
    public float Speed;
    private Transform _transform;
    private Collider2D _trigger;
    private List<Collider2D> _collisions;
    private ContactFilter2D _filter;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _trigger = GetComponent<Collider2D>();
        _collisions = new List<Collider2D>();

        _filter = new ContactFilter2D();
        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    private void Update()
    {
        _transform.Translate(Vector3.up * Time.deltaTime * Speed);
    }

    private void FixedUpdate()
    {
        _trigger.OverlapCollider(_filter, _collisions);
        foreach(Collider2D collider in _collisions)
        {
            if (!collider.GetComponent<BossFight>())
            {
                PlayerHealth player = collider.GetComponent<PlayerHealth>();
                if (player)
                {
                    player.Hurt(5);
                }

                Destroy(gameObject);
            }
        }
    }
}

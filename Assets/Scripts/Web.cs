using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    public float Speed;
    private Transform _transform;
    private BoxCollider2D _collider;
    private ContactFilter2D _filter;
    private List<Collider2D> _colliders;
    private bool _notReached = true;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<BoxCollider2D>();
        _filter = new ContactFilter2D();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _colliders = new List<Collider2D>();
    }

    void Update()
    {
        if (_notReached)
        {
            Vector3 newScale = _transform.localScale;
            newScale.y += Speed * Time.deltaTime;
            _transform.localScale = newScale;
            _collider.size = newScale;
        }

        _colliders.Clear();
        _collider.OverlapCollider(_filter, _colliders);
        foreach (Collider2D collider in _colliders)
        {
            if (!collider.GetComponent<BossFight>())
            {
                _notReached = false;
            }
        }
    }
}

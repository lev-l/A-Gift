using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    public float Speed;
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;
    private ContactFilter2D _filter;
    private List<Collider2D> _colliders;
    private bool _notReached = true;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _filter = new ContactFilter2D();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _colliders = new List<Collider2D>();

        Invoke(nameof(DestroyIt), 3);
    }

    private void Update()
    {
        if (_notReached)
        {
            Vector3 newScale = _renderer.size;
            newScale.y += Speed * Time.deltaTime;
            _renderer.size = newScale;
            _collider.size = newScale;
            _collider.offset = new Vector2(0, newScale.y / 2f);
        }

        _colliders.Clear();
        _collider.OverlapCollider(_filter, _colliders);
        foreach (Collider2D collider in _colliders)
        {
            if (!collider.GetComponent<BossFight>())
            {
                _notReached = false;
                PlayerMovement player = collider.GetComponent<PlayerMovement>();

                if (player)
                {
                    player.Freeze();
                }
            }
        }
    }

    private void DestroyIt()
    {
        Destroy(gameObject);
    }
}

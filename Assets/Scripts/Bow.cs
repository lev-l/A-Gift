using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BowAnimations))]
public class Bow : MonoBehaviour
{
    [SerializeField, Range(10, 100)] private float _maxEnergy;
    private float _currentEnergy = 0;
    private bool _charging = false;
    private int _currentArrows;
    private Transform _transform;
    private ContactFilter2D _filter;
    private List<RaycastHit2D> _hits;
    private BowAnimations _animations;

    private void Start()
    {
        _currentArrows = 5;

        _transform = GetComponent<Transform>();
        _filter = new ContactFilter2D();
        _hits = new List<RaycastHit2D>(1);
        _animations = GetComponent<BowAnimations>();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && _currentArrows > 0)
        {
            _charging = true;
        }
        if (_charging
            && Input.GetMouseButtonUp(0))
        {
            _charging = false;
            Fire();
        }

        if (_charging)
        {
            _currentEnergy += 10 * Time.deltaTime;
            _animations.UpdateBowTense(_currentEnergy);
            if (_currentEnergy >= _maxEnergy)
            {
                _currentEnergy = _maxEnergy;
                _charging = false;
                Fire();
            }
        }
    }

    private void Fire()
    {
        _currentArrows -= 1;
        _animations.UpdateArrowsCount(_currentArrows);
        _animations.StartArrow(_currentEnergy / 1.5f);
        Physics2D.Raycast(origin: _transform.position,
                        direction: _transform.TransformDirection(Vector3.up),
                        distance: _currentEnergy / 1.5f,
                        contactFilter: _filter,
                        results: _hits);

        foreach(RaycastHit2D hit in _hits)
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy)
            {
                enemy.Hurt(_currentEnergy);
            }

            break;
        }
        _currentEnergy = 0;
    }

    public void AddArrows()
    {
        _currentArrows += 5;
        _animations.UpdateArrowsCount(_currentArrows);
    }
}

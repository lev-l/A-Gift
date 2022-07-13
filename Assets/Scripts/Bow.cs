using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BowAnimations))]
public class Bow : MonoBehaviour
{
    [SerializeField, Range(10, 100)] private float _maxEnergy;
    private float _currentEnergy = 0;
    private bool _charging = false;
    private Transform _transform;
    private int _layerMask;
    private BowAnimations _animations;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _layerMask = Physics2D.GetLayerCollisionMask(_transform.parent.gameObject.layer);
        _animations = GetComponent<BowAnimations>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
            if(_currentEnergy >= _maxEnergy)
            {
                _currentEnergy = _maxEnergy;
                _charging = false;
                Fire();
            }
        }
    }

    private void Fire()
    {
        _animations.StartArrow(_currentEnergy / 1.5f);
        RaycastHit2D hit = Physics2D.Raycast(origin: _transform.position,
                                            direction: _transform.TransformDirection(Vector3.up),
                                            distance: _currentEnergy / 1.5f,
                                            layerMask: _layerMask);

        if (hit)
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy)
            {
                enemy.Hurt(_currentEnergy);
            }
        }
        _currentEnergy = 0;
    }
}

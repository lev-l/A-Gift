using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField, Range(10, 100)] private float _maxEnergy;
    private float _currentEnergy = 0;
    private bool _charging = false;
    private Transform _transform;
    private int _layerMask;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _layerMask = Physics2D.GetLayerCollisionMask(_transform.parent.gameObject.layer);
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
            _currentEnergy += 5 * Time.deltaTime;
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
        print("fire!");
        _currentEnergy = 0;
    }
}

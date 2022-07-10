using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 7)] private float Speed;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private float _radius;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _radius = GetComponent<CircleCollider2D>().radius;
    }

    void FixedUpdate()
    {
        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"),
                                            Input.GetAxis("Vertical"));
        movementVector *= Time.deltaTime * Speed;
        float raycastDistance = movementVector.magnitude + _radius;

        RaycastHit2D hit = Physics2D.Raycast(origin: _transform.position,
                                            direction: movementVector,
                                            distance: raycastDistance,
                                            layerMask: Physics2D.GetLayerCollisionMask(gameObject.layer));
        if (hit)
        {
            float movementMaxDistance = Vector2.Distance(_transform.position, hit.point) - _radius;
            movementVector = Vector2.ClampMagnitude(movementVector, movementMaxDistance);
        }

        _transform.Translate(movementVector);
    }
}

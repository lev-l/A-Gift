using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 7)] private float Speed;
    private Transform _transform;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"),
                                            Input.GetAxis("Vertical"));
        movementVector *= Time.deltaTime;
        float raycastDistance = movementVector.magnitude + (_transform.localScale.x / 2f);

        Physics2D.Raycast(origin: _transform.position,
                            direction: movementVector,
                            distance: raycastDistance,
                            layerMask: Physics2D.GetLayerCollisionMask(gameObject.layer));
    }
}

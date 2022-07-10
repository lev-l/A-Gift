using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 7)] private float Speed;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private RaycastHit2D[] _hitsBuffer;
    private List<RaycastHit2D> _hits;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _hitsBuffer = new RaycastHit2D[3];
        _hits = new List<RaycastHit2D>(3);
    }

    void FixedUpdate()
    {
        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"),
                                            Input.GetAxis("Vertical"));
        float movementDistance = movementVector.magnitude * Speed * Time.deltaTime;

        if(_rigidbody.Cast(movementVector, _hitsBuffer, movementDistance) > 0)
        {
            _hits.Clear();
            foreach(RaycastHit2D hit in _hitsBuffer)
            {
                if (hit)
                {
                    _hits.Add(hit);
                }
            }

            for(int i = 0; i < _hits.Count; i++)
            {
                float projection = Vector2.Dot(movementVector, _hits[i].normal);
                if(projection < 0)
                {
                    movementVector -= _hits[i].normal * projection;
                }

                movementDistance = _hits[i].distance - 0.1f < movementDistance ?
                                                _hits[i].distance - 0.1f : movementDistance;
            }
        }

        _transform.Translate(movementVector * movementDistance);
    }
}

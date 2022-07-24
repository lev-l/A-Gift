using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 7)] private float Speed;
    private Transform _transform;
    public bool _unfreezed;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _unfreezed = true;
    }

    void FixedUpdate()
    {
        if (_unfreezed)
        {
            Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"),
                                                Input.GetAxis("Vertical")).normalized;
            float movementDistance = movementVector.magnitude * Speed * Time.deltaTime;
            _transform.Translate(movementVector * movementDistance);
        }
    }

    public void Freeze()
    {
        _unfreezed = false;
        Invoke(nameof(Unfreeze), 2);
    }

    private void Unfreeze()
    {
        _unfreezed = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RotationToMouse))]
public class BowAnimations : MonoBehaviour
{
    private Transform _transform;
    private Transform _arrow;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _arrow = _transform.GetChild(0);
    }

    public void UpdateBowTense(float strength)
    {
        _transform.localPosition = _transform.TransformDirection(Vector3.down * strength / 30);
    }

    public void StartArrow(float strength)
    {
        UpdateBowTense(0);
        _arrow.SetParent(_transform);
        _arrow.localPosition = Vector3.zero;
        _arrow.localRotation = Quaternion.Euler(Vector3.zero);

        StopCoroutine(nameof(Arrow));
        StartCoroutine(Arrow(strength));
    }

    private IEnumerator Arrow(float strength)
    {
        _arrow.localPosition = Vector3.zero;
        _arrow.SetParent(null);
        float distanceRemain = strength;

        while(distanceRemain > 0.1f)
        {
            Vector3 localMove = GetNextLocalMove(distanceRemain);
            if(localMove.x != 0)
            {
                throw new System.Exception("local move isn't straight");
            }
            distanceRemain -= localMove.y;

            _arrow.position = _arrow.TransformPoint(localMove);
            yield return new WaitForEndOfFrame();
        }

        _arrow.SetParent(_transform);
        _arrow.localRotation = Quaternion.Euler(Vector3.zero);
        _arrow.localPosition = Vector3.zero;
    }

    private Vector3 GetNextLocalMove(float distance)
    {
        return Vector3.Lerp(Vector2.zero, Vector2.up * distance / _arrow.localScale.y, 5f) * Time.deltaTime * 10;
    }
}

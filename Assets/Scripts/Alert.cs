using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{
    private ArrowsContainer _arrowsSystem;
    private Collider2D _trigger;
    private ContactFilter2D _filter;
    private List<Collider2D> _overlapResults;

    private void Awake()
    {
        _arrowsSystem = GetComponentInParent<ArrowsContainer>();
        _trigger = GetComponent<Collider2D>();
        _overlapResults = new List<Collider2D>(5);

        _filter = new ContactFilter2D();
        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    private void OnEnable()
    {
        StartCoroutine(Checking());
    }

    private IEnumerator Checking()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            if(_trigger.OverlapCollider(_filter, _overlapResults) > 0)
            {
                foreach(Collider2D collider in _overlapResults)
                {
                    if (collider.GetComponent<PlayerHealth>())
                    {
                        _arrowsSystem.PlayerHasCome();
                        gameObject.SetActive(false);
                        Invoke(nameof(AwakeSamurai), 60);
                    }
                }
            }
        }
    }

    private void AwakeSamurai()
    {
        gameObject.SetActive(true);
    }
}

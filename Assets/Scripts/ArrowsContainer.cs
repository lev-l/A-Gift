using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsContainer : MonoBehaviour
{
    private GameObject _arrows;
    private Bow _bow;

    private void Start()
    {
        _arrows = transform.GetChild(0).gameObject;
        _bow = FindObjectOfType<Bow>();
    }

    public void PlayerHasCome()
    {
        _bow.AddArrows();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthPresenter : MonoBehaviour
{
    private Transform _image;
    private float _maxHealth;

    private void Start()
    {
        _image = FindObjectOfType<Canvas>().transform.GetChild(2);
        _maxHealth = FindObjectOfType<PlayerHealth>().GetHealthParams().max;
    }

    public void UpdateHP(float current)
    {
        _image.localScale = new Vector3(current / _maxHealth, 1, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAnimations : MonoBehaviour
{
    private Transform _image;
    private float _maxHP;

    private void Start()
    {
        _image = FindObjectOfType<Canvas>().transform.GetChild(0);
        _maxHP = FindObjectOfType<EnemyHealth>().GetHealthParams().max;
    }

    public void UpdateHP(float currentHP)
    {
        _image.localScale = new Vector3(currentHP / _maxHP, 1, 1);
    }
}

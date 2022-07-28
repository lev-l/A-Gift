using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAnimations : MonoBehaviour
{
    private Animator _animator;
    private Transform _image;
    private float _maxHP;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _image = FindObjectOfType<Canvas>().transform.GetChild(0);
        _maxHP = FindObjectOfType<EnemyHealth>().GetHealthParams().max;
    }

    public void UpdateHP(float currentHP)
    {
        _image.localScale = new Vector3(currentHP / _maxHP, 1, 1);
    }

    public void StartMoving()
    {
        _animator.SetBool("Move", true);
    }

    public void StopMoving()
    {
        _animator.SetBool("Move", false);
    }
}

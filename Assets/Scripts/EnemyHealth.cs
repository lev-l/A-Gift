using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    protected float _currentHealth;
    private BossFight _boss;

    public virtual (float current, float max) GetHealthParams()
    {
        return (_currentHealth, _maxHealth);
    }

    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
        _boss = GetComponent<BossFight>();
    }

    public virtual void Hurt(float damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        _boss.Damaged(_currentHealth);
    }

    public virtual void Heal(float restorePoints)
    {
        _currentHealth += restorePoints;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }
}

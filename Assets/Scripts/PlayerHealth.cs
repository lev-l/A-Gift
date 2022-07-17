using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;
    protected float _currentHealth;

    public virtual (float current, float max) GetHealthParams()
    {
        return (_currentHealth, _maxHealth);
    }

    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
    }

    public virtual void Hurt(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            SceneManager.LoadScene(0);
        }
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

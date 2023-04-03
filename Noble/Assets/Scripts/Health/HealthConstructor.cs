using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles the health of the player
public class HealthConstructor : MonoBehaviour
{
    private int _currentMaxHealth;
    private int _currentHealth;

    void Start()
    {
        _currentMaxHealth = 100;
        _currentHealth = _currentMaxHealth;
    }

    void Update()
    {
        Debug.Log("Here is the current health of the player: " + _currentHealth);
        if (_currentHealth <= 0)
        {
            Debug.Log("The player has ran out of health .. and the application is quitting");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    //Properties
    public int Health
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }

    public int maxHealth
    {
        get { return _currentMaxHealth; }
        set { _currentMaxHealth = value; }
    }

    //Constructor
    public void unitHealth(int health, int maxHealth)
    {
        _currentHealth = health;
        _currentMaxHealth = maxHealth;
    }

    //Methods
    public void dmgUnit(int dmgAmount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= dmgAmount;
        }
    }

    public void healUnit(int healAmount)
    {
        if (_currentHealth > 0 && _currentHealth < _currentMaxHealth)
        {
            _currentHealth += healAmount;
        }
        if (_currentHealth > _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            _currentHealth -= 25;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            _currentHealth -= 10;
        }
    }
}

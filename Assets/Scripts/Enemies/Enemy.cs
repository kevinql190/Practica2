using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IStealFoodType
{
    [Header("Enemy")]
    public FoodScriptableObject EnemyType;
    public int CurrentHealth { get; set; }
    [Header("Attack")]
    public int damage = 1;
    public GameObject target; 
    public float attackRange = 5f;
    public float detectionDistance = 10f;
    public float prepareAttackTime = 0.5f;
    public float cooldownAttack = 1f;
    public float stoppingDistanceAttack = 1f; //rang en el que es para el navagent abans de preparar-se per atacar


    private void Start()
    {
        CurrentHealth = EnemyType.enemyHealth;
    }
    public virtual void Attack() { }
    public virtual void FacePlayer() { }
    public virtual void Damager() 
    {
        target.GetComponent<IDamageable>().TakeDamage(-damage);
    }
    public bool InRange()
    {
        return Vector3.Distance(target.transform.position, transform.position) <= attackRange;
    }
    public void TakeDamage(int damage)
    {
        if (CurrentHealth > 1) //Damage
        {
            CurrentHealth += damage;
            //Efecte damage i animaci�
        }
        else //Mort
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    public void StealFoodType(PanController panController)
    {
        panController.ChangeFoodType(EnemyType.FoodType);
    }

}

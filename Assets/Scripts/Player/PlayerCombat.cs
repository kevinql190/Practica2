using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private PanController _panController;
    [Header("Attack Info")]
    [SerializeField] private int playerDamage;
    [SerializeField] private Cooldown attackCookdown;
    [Header("Attack Layers")]
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _breakableLayer;
    [Header("Gizmos")]
    [SerializeField] private bool attackGizmo;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackRadius;
    private Vector3 AttackPoint => transform.position + transform.forward * attackDistance;
    private void Start()
    {
        _panController = GetComponent<PanController>();
    }
    private void Update()
    {
        HandleAttack();
    }
    private void HandleAttack()
    {
        if (!PlayerInputHandler.AttackJustPressed || _panController.currentFoodType != FoodType.Default || attackCookdown.IsCoolingDown) return;
        AudioManager.Instance.PlaySFXOnce("atac");

        Collider[] breakableColliders = Physics.OverlapSphere(AttackPoint, attackRadius, _breakableLayer);
        if(breakableColliders.Length != 0)
        {
            foreach (Collider breakable in breakableColliders) breakable.GetComponent<IDamageable>().TakeDamage(-playerDamage);
        }
        Collider[] enemyColliders = Physics.OverlapSphere(AttackPoint, attackRadius, _enemyLayer);
        if(enemyColliders.Length != 0) DamageClosestEnemy(enemyColliders);
        attackCookdown.StartCooldown();
    }
    private void DamageClosestEnemy(Collider[] enemies)
    {
        float _closestDistance = Mathf.Infinity;
        Collider closestEnemy = new();
        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < _closestDistance)
            {
                closestEnemy = enemy;
                _closestDistance = distanceToEnemy;
            }
        }
        if (closestEnemy != null)
        {
            closestEnemy.GetComponent<IDamageable>().TakeDamage(-playerDamage);
            closestEnemy.GetComponent<IStealFoodType>()?.StealFoodType(_panController);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (!attackGizmo) return;
        //Per veure el rang d'atac a l'editor.
        Gizmos.DrawSphere(AttackPoint, attackRadius);
    }
}

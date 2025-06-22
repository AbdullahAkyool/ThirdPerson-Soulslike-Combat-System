using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    private EnemyController enemyController;

    public int MaxHealth = 100;
    public int CurrentHealth = 100;

    [SerializeField] private EnemyHealthBarController healthBarController;
    public EnemyHealthBarController HealthBarController => healthBarController;

    [SerializeField] private ParticleSystem hitParticle;
    public ParticleSystem HitParticle => hitParticle;
    [SerializeField] private ParticleSystem bloodParticle;
    public ParticleSystem BloodParticle => bloodParticle;

    void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    private void Start()
    {
        ResetHealthSystem();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        healthBarController.UpdateHealthBar(CurrentHealth / (float)MaxHealth);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;

            enemyController.stateMachine.ChangeState(new EnemyDieState(enemyController));
        }

        if (!(enemyController.stateMachine.CurrentState is EnemyGetHitState))
        {
            enemyController.stateMachine.ChangeState(new EnemyGetHitState(enemyController));
        }
    }
    public void ResetHealthSystem()
    {
        healthBarController.fillBar.gameObject.SetActive(true);
        CurrentHealth = MaxHealth;
        healthBarController.UpdateHealthBar(1f);
    }

    public void OnEnemyGetHitEnd() // animation event
    {
        if (enemyController.stateMachine.CurrentState is EnemyGetHitState)
        {
            enemyController.stateMachine.ChangeState(new EnemyIdleState(enemyController));
        }
    }
}

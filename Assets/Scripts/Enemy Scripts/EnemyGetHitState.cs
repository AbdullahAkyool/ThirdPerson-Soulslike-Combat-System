using UnityEngine;

public class EnemyGetHitState : IState
{
    private EnemyController enemy;
    private int hitAnimCount = 5;

    public EnemyGetHitState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void StateEnter()
    {
        EnemyGetHitAnimation();
    }
    public void StateUpdate() { }

    public void StateExit() { }

    public void EnemyGetHitAnimation()
    {
        int randomIndex = Random.Range(0, hitAnimCount);
        enemy.Animator.SetInteger("HitIndex", randomIndex);
        enemy.Animator.SetTrigger("GetHit");

        enemy.EnemyHealthController.HitParticle.Play();

        if (!enemy.EnemyHealthController.BloodParticle.isPlaying)
        {
            enemy.EnemyHealthController.BloodParticle.Play();
        }
    }
}

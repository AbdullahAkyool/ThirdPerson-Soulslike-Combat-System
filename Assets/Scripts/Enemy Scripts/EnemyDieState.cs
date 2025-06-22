using System.Collections;
using UnityEngine;

public class EnemyDieState : IState
{
    private EnemyController enemy;

    private float dissolveDuration = 3f;
    private float beforeDissolve = 3f;
    private float postDissolveDelay = 5f;

    private Vector3 currentEnemyPosition;

    public EnemyDieState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void StateEnter()
    {
        currentEnemyPosition = enemy.transform.position;
        currentEnemyPosition.y += .5f;
        enemy.transform.position = currentEnemyPosition;

        enemy.Coll.enabled = false;
        enemy.EnableRagdoll();  
        enemy.Animator.enabled = false;

        enemy.EnemyHealthController.HealthBarController.CloseHealthBar();

        enemy.StartCoroutine(DissolveAndRespawn());
    }
    public void StateUpdate() { }

    public void StateExit() { }

    private IEnumerator DissolveAndRespawn()
    {
        yield return new WaitForSeconds(beforeDissolve);

        Material mat = enemy.MeshRenderer.materials[0];
        mat.SetFloat("_Dissolve", 0f);
        float elapsed = 0f;
        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            mat.SetFloat("_Dissolve", Mathf.Lerp(0f, 1f, elapsed / dissolveDuration));
            yield return null;
        }

        enemy.EnemyHealthController.HitParticle.Stop();
        enemy.EnemyHealthController.BloodParticle.Stop();

        yield return new WaitForSeconds(postDissolveDelay);

        currentEnemyPosition.y -= .5f;
        enemy.transform.position = currentEnemyPosition;

        mat.SetFloat("_Dissolve", 0f);

        enemy.Coll.enabled = true;

        enemy.Animator.Rebind();
        enemy.Animator.Update(0f);
        enemy.Animator.enabled = true;

        enemy.EnemyHealthController.ResetHealthSystem();
        enemy.stateMachine.ChangeState(new EnemyIdleState(enemy));
    }

}

using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : IState
{
    private PlayerController player;
    private int attackAnimCount = 5;

    private int currentCombo = 0;
    private float necessaryInputTime = 0.7f;
    private float timer = 0f;
    private bool comboQueued = false;
    private bool animationFinished = false;

    private ProgressBarController progressBarController;

    public PlayerAttackState(PlayerController player)
    {
        this.player = player;
        progressBarController = UIManager.Instance.ProgressBarController;
    }

    public void StateEnter()
    {
        PlayAttackAnimation();
    }

    public void StateUpdate()
    {
        timer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && timer <= necessaryInputTime)
        {
            comboQueued = true;
        }

        if (animationFinished)
        {
            if (comboQueued && currentCombo < 2)
            {
                currentCombo++;
                comboQueued = false;
                animationFinished = false;
                timer = 0f;
                PlayAttackAnimation();
            }
            else
            {
                player.stateMachine.ChangeState(new PlayerIdleState(player));
            }
        }
    }

    public void StateExit()
    {
        comboQueued = false;
        animationFinished = false;
        currentCombo = 0;
    }

    private void PlayAttackAnimation()
    {
        player.Sword.SwordParticle.Play();

        int randomIndex = Random.Range(0, attackAnimCount);
        player.Animator.SetInteger("AttackIndex", randomIndex);
        player.Animator.SetTrigger("Attack");

        if (player.AttackZoneController.EnemiesOnTarget.Count > 0)
        {
            // Combo UI
            if (currentCombo == 1)
            {
                ActionManager.OnShowAttackMessage("x2 COMBO!");
            }
            else if (currentCombo == 2)
            {
                ActionManager.OnShowAttackMessage("x3 COMBO!\nCRITICAL HIT!");
            }

            progressBarController.ShowProgressBar(necessaryInputTime);
        }
    }

    public void OnAttackAnimationEnd()
    {
        animationFinished = true;

        progressBarController.HideProgressBar();

        player.Sword.SwordParticle.Stop();
    }

    public void OnAttackAnimationHit(List<EnemyHealthController> enemiesOnTarget)
    {
        for (int i = enemiesOnTarget.Count - 1; i >= 0; i--)
        {
            var enemy = enemiesOnTarget[i];

            if (currentCombo < 2)
            {
                enemy.TakeDamage(player.Sword.SwordPower);
            }
            else if (currentCombo >= 2)
            {
                enemy.TakeDamage(player.Sword.SwordPower * 2);
            }

            if (enemy.CurrentHealth <= 0)
            {
                enemiesOnTarget.RemoveAt(i);
            }
        }
    }
}



using System.Collections;
using TMPro;
using UnityEngine;
public enum BattleState
{
    START,
    PLAYERTURN,
    ATTACKING,
    ENEMYTURN,
    WON,
    LOST
}

public class BattleSystem : MonoBehaviour
{
    private bool notInBattle = true;

    public BattleState state;
    public Transform fightCheck;
    public LayerMask enemyLayer;

    public GameObject player;
    public GameObject enemy;

    public GameObject UI;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    public TMP_Text gameStateText;
    public TMP_Text dialogue;
    public GameObject attackButton;

    public QuestionManager questionManager;


    private Unit playerUnit;
    private Rigidbody2D playerBody;

    private Unit enemyUnit;
    private Rigidbody2D enemyBody;

    private void Update()
    {
        if (InCombatDistance() && notInBattle)
        {
            notInBattle = false;
            state = BattleState.START;
            gameStateText.text = "Start";

            StartCoroutine(SetupBattle());
        }
    }

    private bool InCombatDistance()
    {
        return Physics2D.OverlapCircle(fightCheck.position, 5f, enemyLayer);
    }

    IEnumerator SetupBattle()
    {
        playerUnit = player.GetComponent<Unit>();
        enemyUnit = enemy.GetComponent<Unit>();

        playerBody = player.GetComponent<Rigidbody2D>();
        enemyBody = enemy.GetComponent<Rigidbody2D>();

        playerBody.constraints = RigidbodyConstraints2D.FreezeAll;
        enemyBody.constraints = RigidbodyConstraints2D.FreezeAll;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        UI.SetActive(true);

        dialogue.text = "Engaged in combat with " + enemyUnit.unitName + "!";

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        gameStateText.text = "Player Turn";

        PlayerTurn();
    }

    private void PlayerTurn()
    {
        attackButton.SetActive(true);
        state = BattleState.PLAYERTURN;
        dialogue.text = "Choose an action:";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        state = BattleState.ATTACKING;
        attackButton.SetActive(false);
        questionManager.AskQuestion();
    }

    public IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        dialogue.text = "The attack is successful!";

        enemyHUD.SetHP(enemyUnit.currentHp);

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            gameStateText.text = "Won";
            dialogue.text = "You won the battle!";
            StartCoroutine(EndBattle());
        }
        else
        {
            dialogue.text = "Enemy is stunned!";
            yield return new WaitForSeconds(1f);
            PlayerTurn();
        }
    }
    public void StartEnemyTurn()
    {
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        state = BattleState.ENEMYTURN;
        gameStateText.text = "Enemy Turn";
        dialogue.text = "Thinking...";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        dialogue.text = "The attack is successful!";

        playerHUD.SetHP(playerUnit.currentHp);

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.LOST;
            gameStateText.text = "Lost";
            dialogue.text = "You have been defeated...";
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            gameStateText.text = "Player Turn";
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            Destroy(enemy);
            Destroy(enemyHUD);

            playerBody.constraints = RigidbodyConstraints2D.None;
            playerUnit.currentHp = playerUnit.maxHp;

            notInBattle = true;
        }
        else
        {
            Destroy(player);
            Destroy(playerHUD);
            yield return new WaitForSeconds(2f);

            Application.Quit();
        }
        yield return new WaitForSeconds(2f);

        UI.SetActive(false);
    }
}

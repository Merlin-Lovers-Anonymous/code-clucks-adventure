using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Slider playerHealth;
    public TMP_Text nameText;
    public TMP_Text levelText;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl: " + unit.unitLevel;
        playerHealth.maxValue = unit.maxHp;
        playerHealth.value = unit.currentHp;
    }

    public void SetHP(int hp)
    {
        playerHealth.value = hp;
    }
}

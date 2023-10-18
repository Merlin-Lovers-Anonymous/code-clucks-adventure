using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHp;
    public int currentHp;

    public bool TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

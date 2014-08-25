using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LDInventory {

    public Dictionary<int, LDCthulu> cthuluParts;
    public LDWeapon weapon;
    public LDPotion potion;
    public int potionsCount = 0;
    public int activeWeapon;
    public int potionHealth = 25;
    public bool hasKey = false;
	// Use this for initialization

	public LDInventory () {
        cthuluParts = new Dictionary<int, LDCthulu>();
        activeWeapon = -1;
    }

    public void setWeapon(LDWeapon w)
    {
        activeWeapon = 1;
        weapon = w;
    }

    public bool AddCthuluPart(LDCthulu cthuluPart)
    {
        int i = 0;
        while (i <= 9)
        {
            if (!cthuluParts.ContainsKey(i))
            {
                cthuluParts.Add(i, cthuluPart);
                return true;
            }
            i++;

            if (i == 10)
            {
                hasKey = true;
            }
        }
        return false;
    }

    public void AddPotion()
    {
        potionsCount++;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().potionCount++;
    }

    public void Use()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().health += potionHealth;

    }
}

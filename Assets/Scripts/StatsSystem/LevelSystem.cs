using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LevelSystem
{
    public string levelSystemId;
    public int levelMax = 100;
    public float currentXp;
    public int currentLevel = 1;
    public float requiredXp;
    public float totalXp;
    public float additionMultiplier = 300;
    public float powerMultiplier = 2;
    public float divisionMultiplier = 7;
    public static Action<string, float, float, int> onXpLvlGained;
    public static Action<string> onLvlUp;

    public LevelSystem(string levelSystemId, float additionMultiplier = 300f, float powerMultiplier = 2f, float divisionMultiplier = 7f)
    {
        this.levelSystemId = levelSystemId;
        this.additionMultiplier = additionMultiplier;
        this.powerMultiplier = powerMultiplier;
        this.divisionMultiplier = divisionMultiplier;
    }
    public void GainExperience(float xp) {
        Debug.Log("levelSystem");
        if (currentLevel >= levelMax)
            return;
        // Si requiredXp n'a pas encore été calculé, on le fait
        if (requiredXp == 0)
            requiredXp = CalculateRequiredXp();

        currentXp += xp;

        // Si l'xp actuelle est supérieure à l'xp requise pour lvl up
        if (currentXp >= requiredXp)
        {
            //On appel la méthode levelUp avec comme paramètre currentXp - requiredXp, ce qui représente l'xp qui en trop que le joueur a gagné
            LevelUp(currentXp - requiredXp);
        }
        //On appel l'event onXpLvlGained avec : l'id de ce LevelSystem (exemple : levelJoueur), l'xp actuel, le lvl actuel
        onXpLvlGained?.Invoke(levelSystemId, currentXp, requiredXp, currentLevel);
    }
    private void LevelUp(float remainningXp)
    {
        // On augmente le level actuel
        currentLevel++;
        // On calcute l'xp total du joueur
        totalXp += currentXp - remainningXp;
        // On set l'xp actuelle = a l'xp restante, c'est à dire l'xp "en trop" après le lvl up
        currentXp = 0;
        requiredXp = CalculateRequiredXp();
        onLvlUp?.Invoke(levelSystemId);

        GainExperience(remainningXp);

    }

    private int CalculateRequiredXp()
    {
        int calculatedRequiredXp = 0;
        for (int i = 0; i < currentLevel; i++)
        {
            calculatedRequiredXp += (int)Mathf.Floor(i + additionMultiplier * Mathf.Pow(powerMultiplier, i / divisionMultiplier));
        }
        return calculatedRequiredXp / 4;
    }

}

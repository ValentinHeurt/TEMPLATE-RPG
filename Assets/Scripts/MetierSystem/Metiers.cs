using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Metiers : MonoBehaviour
{

    /*WORKFLOW
    JOUEUR APPROCHE RESSOURCE RECOLTABLE -> INPUT INTERACTION
    -> CHECK LVL (V�rifie que le lvl du m�tier correspondant du joueur est >= au lvl requis de la ressource)
    -> SI CHECK LVL RETURN TRUE -> On raise l'event OnMetierBehaviorTrigger du m�tier correspondant 
    avec en entr�e un HarvestableItem.
    */

    //Liste des m�tiers
    public List<Metier> metiers;

    private void Start()
    {
        metiers.ForEach(m => m.levelSystem = new LevelSystem(m.Name));
    }

    public bool CheckLevel(string metierID, int lvl)
    {
        return metiers.First(metier => metier.ID == metierID).levelSystem.currentLevel >= lvl;
    }

    public void GiveXp(string metierID, int xp)
    {
        metiers.First(metier => metier.ID == metierID).levelSystem.GainExperience(xp);
    }

    //Start : On va r�cup�rer dynamiquement tout les m�tiers et les ajouter � la liste des m�tiers

    //CheckLevel input : Le lvl requis de la ressource, le m�tier vis�


}

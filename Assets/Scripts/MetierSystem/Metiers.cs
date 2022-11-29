using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Metiers : MonoBehaviour
{

    /*WORKFLOW
    JOUEUR APPROCHE RESSOURCE RECOLTABLE -> INPUT INTERACTION
    -> CHECK LVL (Vérifie que le lvl du métier correspondant du joueur est >= au lvl requis de la ressource)
    -> SI CHECK LVL RETURN TRUE -> On raise l'event OnMetierBehaviorTrigger du métier correspondant 
    avec en entrée un HarvestableItem.
    */

    //Liste des métiers
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

    //Start : On va récupérer dynamiquement tout les métiers et les ajouter à la liste des métiers

    //CheckLevel input : Le lvl requis de la ressource, le métier visé


}

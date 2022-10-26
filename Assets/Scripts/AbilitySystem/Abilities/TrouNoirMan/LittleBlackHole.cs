using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleBlackHole : AbilityBase
{
    [Header("LittleBlackHole")]
    [SerializeField] GameObject blackHole;
    // Ajouter un serializeField offsetedPosition qui sera la position de spawn
    public override void Ability()
    {
        Vector3 temp = new Vector3(0.5f + transform.position.x, 0.5f + transform.position.y, 0.5f + transform.position.z);
        GameObject blackHolePrefab = Instantiate(blackHole,temp, Quaternion.identity); 
    }

}

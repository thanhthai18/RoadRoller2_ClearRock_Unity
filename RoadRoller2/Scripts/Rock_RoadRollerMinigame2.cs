using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_RoadRollerMinigame2 : MonoBehaviour
{    
    public float speedRock = 4;



    private void Update()
    {
        if (!GameController_RoadRollerMinigame2.instance.isLose && !GameController_RoadRollerMinigame2.instance.isWin && gameObject != null)
        {
            transform.Translate(Vector3.up * speedRock * Time.deltaTime);
        }
    }
}

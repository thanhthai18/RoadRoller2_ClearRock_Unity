using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBackGround_RoadRollerMinigame2 : MonoBehaviour
{
    private Vector3 startPos;
    public float speedBG = 4;

    private void Start()
    {
        startPos = transform.position;
    }



    private void Update()
    {
        if (GameController_RoadRollerMinigame2.instance.isBegin && !GameController_RoadRollerMinigame2.instance.isLose && !GameController_RoadRollerMinigame2.instance.isWin)
        {
            transform.Translate(Vector3.up * speedBG * Time.deltaTime);
            if (transform.position.y > 15.68906f)
            {
                transform.position = startPos;
            }
        }
    }
}

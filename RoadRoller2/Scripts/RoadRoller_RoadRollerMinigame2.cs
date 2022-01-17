using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadRoller_RoadRollerMinigame2 : MonoBehaviour
{
    public Vector2 lastPos;
    public bool isHoldMouse = false;
    public GameObject VFX;
    public bool isStartSpawnOnGame;


    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_Run;

    private void Start()
    {
        isStartSpawnOnGame = true;
        lastPos = transform.position;
        anim.state.Complete += AnimComplete;
        PlayAnim(anim, anim_Run, true);
    }

    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == anim_Run)
        {
            PlayAnim(anim, anim_Run, true);
        }
        if (trackEntry.Animation.Name == anim_Run && GameController_RoadRollerMinigame2.instance.isLose)
        {
            anim.state.TimeScale = 0;
        }
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }

    private void OnMouseDown()
    {
        isHoldMouse = true;
    }

    private void OnMouseUp()
    {
        isHoldMouse = false;
    }

    private void LateUpdate()
    {
        if (isHoldMouse)
        {
            if (transform.position.x > lastPos.x)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            if (transform.position.x < lastPos.x)
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }
        }
        lastPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            var tmpVFX = Instantiate(VFX, collision.transform.position, Quaternion.identity);
            tmpVFX.transform.localScale = Vector3.one;
            tmpVFX.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() => { Destroy(tmpVFX); });
            //var index = GameController_RoadRollerMinigame2.instance.listRock.IndexOf(collision.GetComponent<Rock_RoadRollerMinigame2>());
            Destroy(collision.gameObject);

            if (!GameController_RoadRollerMinigame2.instance.tutorial.activeSelf)
            {
                GameController_RoadRollerMinigame2.instance.isTutorial = false;
            }
            
            //GameController_RoadRollerMinigame2.instance.listRock.RemoveAt(index);
            
            //if(GameController_RoadRollerMinigame2.instance.listRock.Count == 0)
            //{
            //    GameController_RoadRollerMinigame2.instance.SpawnRock();
            //}

            GameController_RoadRollerMinigame2.instance.rockCount++;
            GameController_RoadRollerMinigame2.instance.SetTextRock();
            if (isStartSpawnOnGame)
            {
                isStartSpawnOnGame = false;
                GameController_RoadRollerMinigame2.instance.SpawnCars();
                GameController_RoadRollerMinigame2.instance.SpawnRock();
            }
            if(GameController_RoadRollerMinigame2.instance.rockCount == 20)
            {
                GameController_RoadRollerMinigame2.instance.Win();
            }
        }

        if (collision.gameObject.CompareTag("Path"))
        {
            GameController_RoadRollerMinigame2.instance.Lose();
            collision.transform.DOKill();
        }
    }
}


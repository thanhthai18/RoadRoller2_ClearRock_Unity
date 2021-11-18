using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cars_RoadRollerMinigame2 : MonoBehaviour
{
    private void Start()
    {
        transform.DOMoveY(transform.position.y - 15, 7).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
            GameController_RoadRollerMinigame2.instance.Respawn();
        });
        transform.DOScale(1, 4).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            GameController_RoadRollerMinigame2.instance.Lose();
            gameObject.transform.DOKill();
        }
    }
}

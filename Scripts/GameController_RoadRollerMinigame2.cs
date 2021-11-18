using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_RoadRollerMinigame2 : MonoBehaviour
{
    public static GameController_RoadRollerMinigame2 instance;

    public Camera mainCamera;
    public RoadRoller_RoadRollerMinigame2 RoadRollerObj;
    public List<Cars_RoadRollerMinigame2> carsPrefab = new List<Cars_RoadRollerMinigame2>();
    public List<Cars_RoadRollerMinigame2> listCarObj = new List<Cars_RoadRollerMinigame2>();
    public Rock_RoadRollerMinigame2 rockPrefab;
    public List<Rock_RoadRollerMinigame2> listRock = new List<Rock_RoadRollerMinigame2>();
    public List<Transform> spawnCarPos = new List<Transform>();
    public List<Transform> spawnRockPos = new List<Transform>();
    public bool isWin, isLose, isBegin, isIntro;
    public Vector2 mousePos;
    public RaycastHit2D[] hit;
    public bool isHoldRoadRoller;
    public Vector2 tmpPos_RoadRoller;
    private float f2;
    public Text txtRock;
    public int rockCount;
    private List<int> listCheckSame1 = new List<int>();
    private List<int> listCheckSame2 = new List<int>();
    private int ran1, ran2;
    public int countCar;
    public GameObject tutorial;
    public bool isTutorial, isCanHoldRoadRoller;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        isWin = false;
        isLose = false;
        isBegin = false;
        isIntro = true;
        isHoldRoadRoller = false;
        isTutorial = true;
        isCanHoldRoadRoller = false;
        rockCount = 0;
        tutorial.SetActive(false);
        SetTextRock();


    }

    private void Start()
    {
        SetSizeCamera();
        Intro();
    }

    void SetSizeCamera()
    {
        float f1;
        f1 = 16.0f / 9;
        f2 = Screen.width * 1.0f / Screen.height;
        mainCamera.orthographicSize *= f1 / f2;
    }

    void Intro()
    {
        SpawnCars();
    }

    void ShowTutorial()
    {
        if (isTutorial)
        {
            isCanHoldRoadRoller = true;
            tutorial.transform.position = RoadRollerObj.transform.position;
            tutorial.SetActive(true);
            Time.timeScale = 0;
            LoopTutorial();
        }
    }

    void LoopTutorial()
    {
        tutorial.transform.position = RoadRollerObj.transform.position;
        for (int i = 0; i < listRock.Count; i++)
        {
            if (listRock[i] != null)
            {
                tutorial.transform.DOMove(listRock[i].transform.position, 2).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                {
                    if (tutorial.activeSelf)
                    {
                        LoopTutorial();
                    }
                });
            }
        }
    }

    public void DelayShowTutorial()
    {
        Invoke(nameof(ShowTutorial), 1);
    }

    public void SetTextRock()
    {
        if (rockCount > 9)
        {
            txtRock.text = rockCount.ToString() + "/20";
        }
        else
        {
            txtRock.text = "0" + rockCount.ToString() + "/20";
        }
    }

    void OnSpawningRock(int maxRock)
    {
        for (int i = 0; i <= Random.Range(0, maxRock); i++)
        {
            ran1 = Random.Range(0, listCheckSame1.Count);
            if (!isIntro)
            {
                listRock.Add(Instantiate(rockPrefab, new Vector2(spawnRockPos[listCheckSame1[ran1]].position.x, spawnRockPos[listCheckSame1[ran1]].position.y - Random.Range(0, 7)), Quaternion.identity));
            }
            else
            {
                listRock.Add(Instantiate(rockPrefab, new Vector2(spawnRockPos[listCheckSame1[ran1]].position.x, spawnRockPos[listCheckSame1[ran1]].position.y ), Quaternion.identity));
            }
            listCheckSame1.RemoveAt(ran1);

        }
    }

    public void SpawnRock()
    {
        listRock.Clear();
        listCheckSame1.Clear();
        listCheckSame1.AddRange(new int[] { 0, 1, 2, 3 });

        if (isIntro)
        {
            OnSpawningRock(1);
        }
        else if (rockCount <= 16)
        {
            OnSpawningRock(4);
        }
        else if (rockCount == 17)
        {
            OnSpawningRock(3);
        }
        else if (rockCount == 18)
        {
            OnSpawningRock(2);
        }
        else if (rockCount == 19)
        {
            OnSpawningRock(1);
        }
    }

    void OnSpawningCar(int carQuantity)
    {
        for (int i = 0; i < carQuantity; i++)
        {
            ran2 = Random.Range(0, listCheckSame2.Count);
            listCarObj.Add(Instantiate(carsPrefab[Random.Range(0, 3)], spawnCarPos[listCheckSame2[ran2]].position, Quaternion.identity));
            listCheckSame2.RemoveAt(ran2);
        }
    }

    public void SpawnCars()
    {
        countCar = 0;
        listCarObj.Clear();
        listCheckSame2.Clear();
        listCheckSame2.AddRange(new int[] { 0, 1, 2, 3 });

        if (isIntro)
        {
            OnSpawningCar(4);
        }
        else if (rockCount < 10)
        {
            OnSpawningCar(1);
        }
        else if (rockCount >= 10 && rockCount <= 14)
        {
            OnSpawningCar(2);
        }
        else if (rockCount >= 15)
        {
            OnSpawningCar(3);
        }
    }
    public void Respawn()
    {
        countCar++;
        if (countCar == listCarObj.Count && !isWin && !isLose)
        {
            if (!isIntro)
            {
                SpawnCars();
                SpawnRock();
            }
            else
            {
                RoadRollerObj.transform.DOMoveX(0, 3).SetEase(Ease.Linear).OnComplete(() =>
                {
                    SpawnRock();
                    isBegin = true;
                    isIntro = false;
                    DelayShowTutorial();
                });
            }
        }
    }

    public void Win()
    {
        isWin = true;
        Debug.Log("Win");
        isHoldRoadRoller = false;
        RoadRollerObj.GetComponent<PolygonCollider2D>().enabled = false;
        RoadRollerObj.transform.DOMoveX(RoadRollerObj.transform.position.x + 30, 1).SetEase(Ease.Linear);
    }
    public void Lose()
    {
        isLose = true;
        Debug.Log("Thua");
        isHoldRoadRoller = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isBegin && !isLose && !isWin && isCanHoldRoadRoller)
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.RaycastAll(mousePos, Vector2.zero);
            if (hit.Length != 0)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider.gameObject.CompareTag("Player"))
                    {
                        isHoldRoadRoller = true;
                        tmpPos_RoadRoller = mousePos - (Vector2)RoadRollerObj.transform.position;
                        if (tutorial.activeSelf)
                        {
                            tutorial.SetActive(false);
                            tutorial.transform.DOKill();
                            Time.timeScale = 1;
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHoldRoadRoller = false;
        }

        if (isHoldRoadRoller)
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            mousePos = new Vector2(Mathf.Clamp(mousePos.x, -mainCamera.orthographicSize * f2 + 1.3f + tmpPos_RoadRoller.x, mainCamera.orthographicSize * f2 - 1.3f + tmpPos_RoadRoller.x), Mathf.Clamp(mousePos.y, -mainCamera.orthographicSize + 0.5f + tmpPos_RoadRoller.y, mainCamera.orthographicSize - 3f + tmpPos_RoadRoller.y));
            RoadRollerObj.transform.position = new Vector2(mousePos.x - tmpPos_RoadRoller.x, mousePos.y - tmpPos_RoadRoller.y);
            RoadRollerObj.transform.position = new Vector2(mousePos.x - tmpPos_RoadRoller.x, mousePos.y - tmpPos_RoadRoller.y);
        }
    }

}

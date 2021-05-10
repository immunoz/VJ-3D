using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject recipe, preparingStep, player, delivery;
    public Canvas canvas;          
    public Sprite goSprite;
    public float showRecipeTime;
    public float offset;
    public float spawnTime;
    public GameObject[] levelDeliveries;

    private GameSteps state;
    private float timer;
    private Vector2 lastPosition;
    private List<GameObject> deliveries;

    enum GameSteps
    {
        RUNNING, SHOWING_RECIPE, PREPARING
    };

    void Start()
    {
        if (recipe != null) state = GameSteps.SHOWING_RECIPE;
        else state = GameSteps.PREPARING;
        timer = 0f;
        deliveries = new List<GameObject>();
        initSpawnPoint();
    }

    private void initSpawnPoint()
    {
        Vector3[] v = new Vector3[4];
        canvas.GetComponent<RectTransform>().GetLocalCorners(v);

        Vector3 finalPosition = v[1];
        GameObject temp = Instantiate(delivery) as GameObject;
        finalPosition.x -= temp.GetComponent<RectTransform>().sizeDelta.x / 2;
        finalPosition.y -= temp.GetComponent<RectTransform>().sizeDelta.y / 2;
        lastPosition = finalPosition;

        Destroy(temp);
    }
    // Update is called once per frame
    void Update()
    {
        bool spaceB = Input.GetKey(KeyCode.Space);
        switch (state) {
            case GameSteps.SHOWING_RECIPE:
                if (spaceB)
                {
                    recipe.SetActive(false);
                    preparingStep.SetActive(true);
                    state = GameSteps.PREPARING;
                    timer = 0;
                }
                break;
            case GameSteps.RUNNING:
                if (timer > 0) timer -= Time.deltaTime;
                else {
                    timer = spawnTime;
                    generateDelivery();
                }
                
                
                break;
            case GameSteps.PREPARING:
                timer += Time.deltaTime;
                if (timer > showRecipeTime)
                {
                    state = GameSteps.RUNNING;
                    preparingStep.SetActive(false);
                    timer = 0;
                }
                if (showRecipeTime * 2 / 3 < timer) preparingStep.GetComponent<Image>().sprite = goSprite;
                break;
        }
    }

    private void generateDelivery() {
        GameObject temp = Instantiate(levelDeliveries[Random.Range(0, levelDeliveries.Length)]) as GameObject;
        temp.transform.SetParent(canvas.transform);
        lastPosition.x += temp.GetComponent<RectTransform>().sizeDelta.x + offset;
        temp.GetComponent<RectTransform>().localPosition = lastPosition;
        deliveries.Add(temp);
    }
}

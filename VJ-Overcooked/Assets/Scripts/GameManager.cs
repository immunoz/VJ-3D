using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public GameObject recipe, preparingStep, player, delivery, plateSpawner;
    public Canvas canvas;          
    public Sprite goSprite;
    public float showRecipeTime;
    public float offset;
    public float spawnTime, spawnPlateTime;
    public GameObject[] levelDeliveries;
    

    private GameSteps state;
    private float timer;
    private Vector2 lastPosition;
    private List<GameObject> deliveries;
    private List<GameObject> plates;
    private List<float> plateCooldown;

    public void increaseScore()
    {
        Debug.Log("increase score!");
    }

    public void AddPlateTimer(GameObject obj)
    {
        plates.Add(obj);
        plateCooldown.Add(spawnPlateTime);
    }

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
        plates = new List<GameObject>();
        plateCooldown = new List<float>();
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
                if (plates.Count != 0) updatePlateTimers();


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
                    player.GetComponent<Player>().setPlay(true);
                }
                if (showRecipeTime * 2 / 3 < timer) preparingStep.GetComponent<Image>().sprite = goSprite;
                break;
        }
    }

    private void updatePlateTimers()
    {
        for (int i = 0; i < plates.Count; ++i) {
            if (plateCooldown[i] > 0) plateCooldown[i] -= Time.deltaTime;
            else
            {
                plates[i].SetActive(true);
                plates[i].GetComponent<Plate>().SetDirty();
                plateSpawner.GetComponent<SpawnPlateLocation>().setObject(plates[i]);
                plates.Remove(plates[i]);
                plateCooldown.RemoveAt(i);
            }
        }
    }

    private void generateDelivery() {
        GameObject temp = Instantiate(levelDeliveries[Random.Range(0, levelDeliveries.Length)]) as GameObject;
        temp.transform.SetParent(canvas.transform,false);
        lastPosition.x += temp.GetComponent<RectTransform>().sizeDelta.x + offset;
        temp.GetComponent<RectTransform>().localPosition = lastPosition;
        deliveries.Add(temp);
    }

    /*public bool deliverPlate(GameObject plate) {
        if (deliveries.Count == 0) return false;
        
        Plate plateScript = plate.GetComponent<Plate>();
        foreach (GameObject delivery in deliveries) {
            Recipe recipeScript = delivery.GetComponent<Recipe>();
            if (recipeScript != null && recipeScript.checkRecipe(plateScript.getIngredients()))
            {
                Destroy(delivery);
                return true;
            }
        }
        return false;
    }*/
    public bool deliverPlate(GameObject plate)
    {
        if (deliveries.Count == 0) return false;

        Plate plateScript = plate.GetComponent<Plate>();
        for (int i = 0; i < deliveries.Count; ++i)
        {
            DeliveryScript d = deliveries[i].GetComponent<DeliveryScript>();
            if (d.dish == plateScript.getPreparedDish())
            {
                Destroy(deliveries[i]);
                deliveries.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
}

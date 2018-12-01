using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null; // Singleton

    public Transform sunMaster;
    public GameObject[] positionsLeft; // Holds position obj's on the left
    public GameObject[] positionsRight; // Holds position obj's on the right
    public GameObject altar;
    public GameObject releasePosition;
    public GameObject citizen;
    public GameObject citizenWoman;
    public GameObject citizenChild;


    public GameObject sacrificeCitizen;
    public GameObject lighting;

    public int citizenAmount;
    public int manAmount;
    public int childAmount;
    public int womanAmount;

    [SerializeField]
    List<GameObject> citizenListLeft= new List<GameObject>();
    [SerializeField]
    List<GameObject> citizenListRight = new List<GameObject>();
    public List<GameObject> citizenList = new List<GameObject>();

    public float curseTime = 2f;
    public float spawnTime = 2f;
    [SerializeField]
    float curseTimer;
    [SerializeField]
    float spawnTimerLeft;
    [SerializeField]
    float spawnTimerRight;
    [SerializeField]
    float spawnTimerNeutral;
    [SerializeField]
    float sunPower;
    void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    void Start () {
        spawnTimerLeft = spawnTime;
        spawnTimerRight = spawnTime;
    }
    void SpawnCurse()
    {
        if (citizenList.Count < 0)
        { 
            int rnd = Random.Range(1, citizenList.Count);
            Instantiate(lighting, citizenList[rnd].transform.position,Quaternion.identity);
            Destroy(citizenList[rnd]);
            curseTimer = curseTime;
        }
    }
	void Update () {

        Debug.Log(citizenList.Count);
        UpdateUI();

        if (sunPower <= 0)
            curseTimer -= Time.deltaTime;
        if (curseTimer <= 0)
            SpawnCurse();

        if (spawnTimerLeft > 0)
            spawnTimerLeft -= Time.deltaTime;
        if (spawnTimerRight > 0)
            spawnTimerRight -= Time.deltaTime;

        // Spawn neutral citizen
        if (spawnTimerNeutral > 0 && citizenList.Count < citizenAmount)
            spawnTimerNeutral -= Time.deltaTime;
        if (spawnTimerNeutral <= 0 && citizenList.Count < citizenAmount) // Active citizen vs max citizen
            SpawnRandomCitizen();

        // Keep 4 citizen spawned on both sides
        if (citizenListLeft.Count < 4 && spawnTimerLeft <= 0 && citizenAmount > 0)
            SpawnCitizenLeft();
        if (citizenListRight.Count < 4 && spawnTimerRight <= 0 && citizenAmount > 0)
            SpawnCitizenRight();

        // Pause
        if (Input.GetKeyDown("space"))
            TogglePause();
        if (Input.GetKeyDown("a") && sacrificeCitizen == null)  //(Input.GetAxis("Horizontal") < 0) 
            SelectLeftCitizen();
        if (Input.GetKeyDown("d") && sacrificeCitizen == null)  //(Input.GetAxis("Horizontal") > 0) 
            SelectRightCitizen();
        if (Input.GetKeyDown("w") && sacrificeCitizen != null)
            Sacrifice();
        if (Input.GetKeyDown("s") && sacrificeCitizen != null)
            ReleaseCitizen();
    }
    public void CheckCitizenList()
    {
        for (int i = 0; i < citizenList.Count; i++)
        {
            // Remove empty indexes if exceeding global citizen amount
            if (citizenList[i] == null)
            {
                Debug.Log("Removing " + i);
                citizenList.RemoveAt(i);
            // Fill empty citizenslots with new citizen
            //if (citizenList[i] == null)
            //   citizenList.Insert(i, RandomCitizenType());
            }
        }
 
    }
    void UpdateUI()
    {
        if (sacrificeCitizen)
            UIManager.instance.ShowButtonsVertical();
        else
            UIManager.instance.HideButtonsVertical();
        if (positionsLeft[4].GetComponent<Position>().CheckStatus())
            UIManager.instance.ShowLeftButton();
        if (positionsRight[4].GetComponent<Position>().CheckStatus())
            UIManager.instance.ShowRightButton();
    }
    void Sacrifice()
    {
        sunPower += sacrificeCitizen.GetComponent<Citizen>().power;
        citizenAmount -= 1;
        AudioManager.instance.Play();
        UpdateSun();
        Demand.
        Destroy(sacrificeCitizen);
    }
    void UpdateSun()
    {
        // Increase sun size
        sunMaster.localScale += new Vector3(.1f,.1f);
        sunMaster.position += new Vector3(0f, .2f);
    }
    void SelectLeftCitizen()
    {
        // If the position next to altar is taken, move citizen to altar
        if (positionsLeft[4].GetComponent<Position>().CheckStatus() && citizenListLeft[0].GetComponent<Citizen>().atTheFinalPosition)
        {
            citizenListLeft[0].GetComponent<Citizen>().MoveToAltar();
            citizenListLeft.RemoveAt(0); // Clean empty index from the list
        }
    }
    void SelectRightCitizen()
    {
        if (positionsRight[4].GetComponent<Position>().CheckStatus() && citizenListRight[0].GetComponent<Citizen>().atTheFinalPosition)
        {
            citizenListRight[0].GetComponent<Citizen>().MoveToAltar();
            citizenListRight.RemoveAt(0); // Clean empty index from the list
        }
    }

    void ReleaseCitizen()
    {
        sacrificeCitizen.GetComponent<Citizen>().MoveToReleasePosition();
        sacrificeCitizen.GetComponent<Citizen>().sr.sortingOrder = 1;
        sacrificeCitizen = null;
    }

    GameObject RandomCitizenType()
    {
        int rnd = Random.Range(1, 4);
        if (rnd == 1)
            return citizen;
        if (rnd == 2)
            return citizenWoman;
        if (rnd == 3)
            return citizenChild;
        // Default
        return null;
    }
    void SpawnRandomCitizen()
    {
        int rnd = Random.Range(1, 3);
        if (rnd == 1)
        {
            GameObject newCitizen = Instantiate(RandomCitizenType(), positionsLeft[0].transform.position, Quaternion.identity);
            citizenList.Add(newCitizen);
            newCitizen.GetComponent<Citizen>().side = Citizen.Side.left;
            newCitizen.GetComponent<Citizen>().myCitizenIndex = citizenList.IndexOf(newCitizen); // Store index locally
            newCitizen.GetComponent<Citizen>().name = newCitizen.GetComponent<Citizen>().name + citizenList.IndexOf(newCitizen); 
            newCitizen.GetComponent<Citizen>().MoveOffScreenRight();
            newCitizen.GetComponent<Citizen>().sr.sortingOrder = 1;
            spawnTimerNeutral = spawnTime;
        }
        else
        {
            GameObject newCitizen = Instantiate(RandomCitizenType(), positionsRight[0].transform.position, Quaternion.identity);
            citizenList.Add(newCitizen);
            newCitizen.GetComponent<Citizen>().side = Citizen.Side.right;
            newCitizen.GetComponent<Citizen>().myCitizenIndex = citizenList.IndexOf(newCitizen); // Store index locally
            newCitizen.GetComponent<Citizen>().name = newCitizen.GetComponent<Citizen>().name + citizenList.IndexOf(newCitizen);
            newCitizen.GetComponent<Citizen>().MoveOffScreenLeft();
            newCitizen.GetComponent<Citizen>().sr.sortingOrder = 1;
            spawnTimerNeutral = spawnTime;
        }
    }

    void SpawnCitizenLeft()
    {
        // Reset spawn timer
        spawnTimerLeft = spawnTime;
        // Spawn
        GameObject newCitizen = Instantiate(RandomCitizenType(), positionsLeft[0].transform.position, Quaternion.identity);
        // Keep track of all the citizen
        citizenListLeft.Add(newCitizen);
        citizenList.Add(newCitizen);
        // Assign side and position to move in
        newCitizen.GetComponent<Citizen>().side = Citizen.Side.left;
        newCitizen.GetComponent<Citizen>().myCitizenIndex = citizenList.IndexOf(newCitizen); // Store index locally
        newCitizen.GetComponent<Citizen>().name = newCitizen.GetComponent<Citizen>().name + citizenList.IndexOf(newCitizen);
        newCitizen.GetComponent<Citizen>().AssignPosition(1); 
    }
    void SpawnCitizenRight()
    {
        // Reset spawn timer
        spawnTimerRight = spawnTime;
        // Spawn
        GameObject newCitizen = Instantiate(RandomCitizenType(), positionsRight[0].transform.position, Quaternion.identity);
        // Keep track of all the citizen
        citizenListRight.Add(newCitizen);
        citizenList.Add(newCitizen);
        // Assign side and position to move in
        newCitizen.GetComponent<Citizen>().side = Citizen.Side.right;
        newCitizen.GetComponent<Citizen>().myCitizenIndex = citizenList.IndexOf(newCitizen); // Store index locally
        newCitizen.GetComponent<Citizen>().name = newCitizen.GetComponent<Citizen>().name + citizenList.IndexOf(newCitizen);
        newCitizen.GetComponent<Citizen>().AssignPosition(1); // int index = citizenListLeft.IndexOf(newCitizen);

    }
    void TogglePause()
    {
        if (Time.timeScale != 0)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}

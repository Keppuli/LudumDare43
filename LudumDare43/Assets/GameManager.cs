using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject demands;
    public GameObject godRays;

    bool gameEnded;

    public GameObject sacrificeCitizen;
    public GameObject lighting;
    public GameObject fireSoul;

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
    public float sunPower = 1f;
    public float sunPowerOld = 1f; // Start x1

    void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
    }
    void Start () {
        spawnTimerLeft = spawnTime;
        spawnTimerRight = spawnTime;
    }
    public void EndGame()
    {
        gameEnded = true;
        demands.SetActive(false);
        UIManager.instance.HideButtonsVertical();
        UIManager.instance.HideSelectButtons();
        UIManager.instance.buttonLeftMaster.SetActive(false);
        godRays.SetActive(true);
    }
    void Update () {
        UpdateUI();
        UpdateSun();

        if (spawnTimerLeft > 0)
            spawnTimerLeft -= Time.deltaTime;
        if (spawnTimerRight > 0)
            spawnTimerRight -= Time.deltaTime;

        // Spawn neutral citizen
        if (spawnTimerNeutral > 0 && citizenList.Count < citizenAmount)
            spawnTimerNeutral -= Time.deltaTime;
        if (spawnTimerNeutral <= 0) // Active citizen vs max citizen
            SpawnRandomCitizen();

        // Keep 4 citizen spawned on both sides
        if (citizenListLeft.Count < 4 && spawnTimerLeft <= 0 && citizenAmount > 0)
            SpawnCitizenLeft();
        if (citizenListRight.Count < 4 && spawnTimerRight <= 0 && citizenAmount > 0)
            SpawnCitizenRight();

        if (!gameEnded)
        {
           
            if (Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow))  //(Input.GetAxis("Horizontal") < 0) 
            {
                if (sacrificeCitizen == null) SelectLeftCitizen();
            }
            if (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow))  //(Input.GetAxis("Horizontal") > 0) 
            {
                if (sacrificeCitizen == null) SelectRightCitizen();
            }
            if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (sacrificeCitizen != null) Sacrifice();
            }
            if (Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow) && sacrificeCitizen != null)
            {
                if (sacrificeCitizen != null) ReleaseCitizen();
            }
            if (Input.GetKeyDown(KeyCode.F1))
                RestartGame();
        }
    }
 
    public void SpawnCurse()
    {
        int rndLR = Random.Range(1, 3);
        if (rndLR == 1 && citizenListLeft.Count > 0)
        {
            CameraManager.instance.GetComponent<CameraManager>().shakeDuration = 0.4f;
            CameraManager.instance.GetComponent<CameraManager>().shakeAmount = 20f;
            AudioManager.instance.PlayLighting();
            int rnd = Random.Range(1, citizenListLeft.Count);
            Instantiate(lighting, citizenListLeft[rnd].transform.position, Quaternion.identity);
            GameObject target = citizenListLeft[rnd];
            citizenListLeft.RemoveAt(rnd); // Clean the list
            Instantiate(GameManager.instance.fireSoul, target.transform.position, Quaternion.identity);
            target.GetComponent<Citizen>().ReleasePosition();
            Destroy(target);
        }
        else if (rndLR == 2 && citizenListRight.Count > 0)
        {
            CameraManager.instance.GetComponent<CameraManager>().shakeDuration = 0.4f;
            CameraManager.instance.GetComponent<CameraManager>().shakeAmount = 20f;
            AudioManager.instance.PlayLighting();
            int rnd = Random.Range(1, citizenListRight.Count);
            Instantiate(lighting, citizenListRight[rnd].transform.position, Quaternion.identity);
            GameObject target = citizenListRight[rnd];
            citizenListRight.RemoveAt(rnd); // Clean the list
            Instantiate(GameManager.instance.fireSoul, target.transform.position, Quaternion.identity);
            target.GetComponent<Citizen>().ReleasePosition();
            Destroy(target);
        }
    }
    public void CheckCitizenList()
    {
        for (int i = 0; i < citizenList.Count; i++)
        {
            // Remove empty indexes if exceeding global citizen amount
            if (citizenList[i] == null)
            {
                //citizenList.RemoveAt(i);
                citizenList.RemoveAll(x => x == null);
                // Fill empty citizenslots with new citizen
                //if (citizenList[i] == null)
                //   citizenList.Insert(i, RandomCitizenType());
            }
        }
    }
    
    void UpdateUI()
    {
        if (sacrificeCitizen)
        {
            UIManager.instance.ShowButtonsVertical();
            UIManager.instance.HideSelectButtons();
        }
        else
        {
            UIManager.instance.HideButtonsVertical();
            if (positionsLeft[4].GetComponent<Position>().CheckStatus())
                UIManager.instance.ShowLeftButton();
            if (positionsRight[4].GetComponent<Position>().CheckStatus())
                UIManager.instance.ShowRightButton(); 
        }
      
    }
    void Sacrifice()
    {
        if (Demand.instance.CheckDemand(sacrificeCitizen))
            // Do something bad if sacrifice is wrong
        sacrificeCitizen.GetComponent<Citizen>().ReleasePosition();
        citizenAmount -= 1;
        AudioManager.instance.PlaySacrifice();
        Instantiate(GameManager.instance.fireSoul, sacrificeCitizen.transform.position, Quaternion.identity);
        Destroy(sacrificeCitizen);
    }
    public void AddSunPower(int amount)
    {
        sunPowerOld = sunPower;
        sunPower += amount;
        PyramidLight.instance.UpdateSprite();
        Priest.instance.CheckPoints();
    }
    void UpdateSun()
    {
        if (sunPower > 50)
        {
            CameraManager.instance.GetComponent<CameraManager>().shakeDuration = 2f;
            CameraManager.instance.GetComponent<CameraManager>().shakeAmount = 1f+ (sunPower/100);
        }
        float step = 100f;
        Vector3 scaleOld = sunMaster.localScale;
        float increase = 1f +sunPowerOld/30f;
        Vector3 scaleNew = new Vector3(sunMaster.transform.position.x + increase, sunMaster.transform.position.y + increase, 0f);
        // Increase sun size
        sunMaster.localScale = Vector3.Lerp(scaleNew, scaleOld, Time.deltaTime * step);
        //sunMaster.localScale += new Vector3(.1f,.1f);
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
        for (int i = 0; i < citizenList.Count; i++)
        {
            if (citizenList[i] == sacrificeCitizen)
                citizenList.RemoveAt(i);
        }
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

    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

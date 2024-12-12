using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class RacerValues
{
    public List<float> raceValueList;

}

public class RaceManager : MonoBehaviour
{

    public static RaceManager instance = null;     //Singleton

    #region Main
    public GameObject[] racers;     //Arrays of players at runtime
    public GameObject[] firstInitPlayers;
    public List<RacerValues> racerValues = new List<RacerValues>();
    public CameraFollow myCamera;

    #endregion

    #region Enums
    public phases phaseSelect;
    public enum phases { first, second, third, fourth, fifth, sixth };

    #endregion

    #region Game Mode
    //[SerializeField] private Image windSymbol;
    //public string[] gameMode = { "WET", "DRY", "SNOW", "MUD" };
    //public int GameModeIndex;                                   // randomly selected before the game
    [SerializeField] private int playerCount; // Will be adjusted from events
    public GameObject startCounter;
    private float startTime = 10f;
    // public string[] windDirection = { "FRONT", "BACK" };                 // proportional to the Rigidbody's weight
    //public int windDirectionIndex;
    // public string[] windStrength = { "LOW", "MEDIUM", "HIGH" };
    //    public int windStrengthIndex;
    public float currentRaceDistance;
    //  public TextMeshProUGUI windStrengthText;
    //  public TextMeshProUGUI windDirectionText;
    // public TextMeshProUGUI gameModeText;

    #endregion

    #region Level 
    public GameObject carSignsGO;
    // public GameObject[] marqueeObjects;
    // public GameObject[] marqueeObjects2;
    public GameObject finishObject;
    public List<TextMeshProUGUI> positionText; // Player's rank
    public List<Vector2> signPositions;
    public GameObject rmArrow;

    #endregion

    #region DivideNumber
    public List<int> userValues;
    #endregion

    #region RaceStarter
    public List<float> totals;
    public TextMeshProUGUI raceTime;
    [Range(0, 1000)] public float raceTotalTime;
    private int pointDivider = 100;
    public int pointCount;
    #endregion

    #region FinishHandle
    public List<TextMeshProUGUI> topPlacementsTime;
    public List<TextMeshProUGUI> finishNames;
    [SerializeField] private TMP_Text eventNameText;
    [SerializeField] private TMP_Text eventPropText;
    [SerializeField] private TMP_Text winnerNameText;
    [SerializeField] private TMP_Text winnerEngineText;
    [SerializeField] private TMP_Text winnerWheelText;
    [SerializeField] private TMP_Text winnerWeightText;
    [SerializeField] private TMP_Text winnerPowertrainText;
    [SerializeField] private TMP_Text winnerTurboText;

    [HideInInspector] public GameObject firstPlaceGO;       //1st racer's GO
    [HideInInspector] public float finishAnimTimer;
    public float finishTimer;
    #endregion

    #region Canvases
    public GameObject startUI;
    public GameObject raceUI;
    [SerializeField] private GameObject skipToRace;

    #endregion

    #region carProperties
    public TextMeshProUGUI targetID;
    public TextMeshProUGUI targetEngine;
    public TextMeshProUGUI targetDT;
    public TextMeshProUGUI targetTurbo;
    public TextMeshProUGUI targetWeight;
    public TextMeshProUGUI targetWheel;
    public TextMeshProUGUI carNumberText;
    #endregion

    #region PreRace
    private int startIncreaser = 0;
    #endregion

    #region Bools 
    [Header("                   BOOLS   ")]
    public bool raceStarted = false;
    public bool preRaceProcess = false;
    public bool lobbyBool = false;
    private bool isFirstPlaceFinded = false;

    public bool isStartUiActive = false;
    bool check = false;
    #endregion
    [SerializeField] private int minRandom, maxRandom;      //nft power will be deleted
    private float sum;      //formula sum
    [SerializeField] private GameObject[] finishVfx;        //finish vfx Havai fisek

    public GameObject lobbyPanel;
    public List<GameObject> placements;     //Final sliding results

    public GameObject winnerPanel;      //winner panel before the lobby panel
    public GameObject startLogo;        //Start presentatipn logo

    [SerializeField]
    private GameObject smokesGO;        //Start smokes GO

    [HideInInspector] public float startPresentationTimer = 0;

    [Header("                                      GET NFT TO RACE SCENE VARIABLES")]
    NetworkManager _networkManager;
    public Participants allParticipantsInGame;
    public int raceID;
    public RaceInfosFromServer raceInfos;
    public GetRoomDetailServerResponse joinedRoomDetail;

    void Awake()
    {

        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
        QualitySettings.vSyncCount = 1;

        ////////Get Network Results
        _networkManager = NetworkManager.Instance;


        // Game Mode Initialize & Adjust
        // GameModeIndex = ((int)Random.Range(0, gameMode.Length));
        // windDirectionIndex = ((int)Random.Range(0, windDirection.Length));
        // windStrengthIndex = ((int)Random.Range(0, windStrength.Length));
        // currentRaceDistance = raceDistances[(int)Random.Range(0, 3.45f)];

        playerCount = 10;

        // Game Mode Texts
        // windStrengthText.text = windStrength[windStrengthIndex].ToString();
        // windDirectionText.text = windDirection[windDirectionIndex].ToString();
        // gameModeText.text = gameMode[GameModeIndex].ToString();


    }
    void Start()
    {
        GetRaceResultsFromServer();
    }

    private void GenerateUserValues()
    {
        //Set the finish object position
        finishObject.transform.position = new Vector3(currentRaceDistance, .23f, -18.28f);

        //CarSigns
        for (int i = 0; i < racers.Length; i++)
        {

            StartCoroutine(ChangeCarSign(i));

        }

        //  NFT's power from abbas will be deleted
        for (int m = 0; m < playerCount; m++)
        {
            racers[m].GetComponent<CarBase>().racerResult.result = racers[m].GetComponent<CarBase>().racerResult.result * 100;
            print((int)racers[m].GetComponent<CarBase>().racerResult.result);
            userValues.Add(((int)racers[m].GetComponent<CarBase>().racerResult.result) / 100);
        }

        pointCount = ((int)(currentRaceDistance / pointDivider));

        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < pointCount * 2; j++)
            {

                racerValues[i].raceValueList.Add(currentRaceDistance / pointCount - 20);

            }
        }

        // Add and Subs values based on the racers values to make placement right.
        for (int i = 0; i < playerCount; i++)
        {

            for (int g = 0; g < pointCount; g++)
            {
                racerValues[i].raceValueList[g] += racers[i].GetComponent<CarBase>().racerResult.len[g];
                sum += racers[i].GetComponent<CarBase>().racerResult.len[g];

            }

            //formula to make values close each other
            if (sum != 0)
            {
                int g = racerValues[i].raceValueList.Count - 1;
                if (sum < 0)
                {
                    while (sum != 0)
                    {
                        racerValues[i].raceValueList[g]++;
                        g--;
                        sum++;

                        if (g == 0)
                        {
                            g = racerValues[i].raceValueList.Count - 1;
                        }
                    }
                }
                else
                {
                    while (sum != 0)
                    {
                        racerValues[i].raceValueList[g]--;
                        g--;
                        sum--;

                        if (g == 0)
                        {
                            g = racerValues[i].raceValueList.Count - 1;
                        }
                    }
                }
            }
            print(sum);

            racerValues[i].raceValueList.Sort();

            for (int j = ((int)(pointCount * .85f)); j < ((int)(pointCount)); j++)
            {

                racerValues[i].raceValueList[j] += userValues[i] / 30;

            }

            for (int j = ((int)(pointCount)); j < ((int)(pointCount) * 2); j++)
            {

                racerValues[i].raceValueList[j] += userValues[i] / 10;

            }

        }


        //car names wrt powers will be deleted
        for (int j = 0; j < 10; j++)
        {
            // change the name of the cars w.r.t car powers
            racers[j].gameObject.name = "AVAXCARS " + userValues[j];

        }


        // totals for analyze will be deleted
        for (int i = 0; i < playerCount; i++)
        {
            float ali = 0;

            firstInitPlayers[i].GetComponent<CarScript>().racerValues = racerValues[i].raceValueList;
            for (int j = 0; j < pointCount; j++)
            {

                ali += racerValues[i].raceValueList[j];

            }

            totals.Add(ali);
        }
    }
    void OnEnable()
    {
        timeScaleIncrease.onClick.AddListener(TimeScaleIncreaser);
        timeScaleDecrease.onClick.AddListener(TimeScaleDecreaser);

    }
    [SerializeField] private TextMeshProUGUI timeScaleText;

    private float deltaTime;
    public TextMeshProUGUI fpsText;
    void Update()
    {



        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();

        timeScaleText.text = "TimeScale: " + Time.timeScale;
        //Total race time
        if (raceStarted == true)
        {
            raceTotalTime += Time.deltaTime;
            raceTime.text = "" + ((int)raceTotalTime);
        }

        switch (phaseSelect)
        {
            case (phases.first):

                //start presentation timer
                startPresentationTimer += Time.deltaTime;
                if (startPresentationTimer > 7)
                {

                    InvokeRepeating("RaceStartManagement", 0, 5);
                    startLogo.gameObject.SetActive(false);
                    phaseSelect = phases.second;

                }

                break;
            case (phases.second):

                //Ui actions
                if (isStartUiActive == false)
                {

                    startUI.SetActive(true);
                    isStartUiActive = true;

                }

                for (int i = 0; i < playerCount; i++)
                    firstInitPlayers[i].GetComponent<Rigidbody>().velocity = Vector3.right * 20;

                startUI.gameObject.transform.position = Vector3.Lerp(startUI.gameObject.transform.position, new Vector3(-43, startUI.gameObject.transform.position.y, startUI.gameObject.transform.position.z), Time.deltaTime * 2);
                skipToRace.gameObject.transform.localPosition = Vector3.Lerp(skipToRace.gameObject.transform.localPosition, new Vector3(820, skipToRace.gameObject.transform.localPosition.y, skipToRace.gameObject.transform.localPosition.z), Time.deltaTime);

                // Pre race camera operation
                PreRaceProcessCamera();

                //prerace ui and invoke actions
                if (preRaceProcess)
                {
                    skipToRace.SetActive(false);
                    CancelInvoke("RaceStartManagement");
                    smokesGO.gameObject.SetActive(false);
                    InvokeRepeating("RacePosition", 0, .1f);
                    phaseSelect = phases.third;
                }

                break;

            case (phases.third):

                Starter();

                if (raceStarted)
                {
                    for (int i = 0; i < playerCount; i++)
                    {
                        firstInitPlayers[i].GetComponent<Rigidbody>().isKinematic = false;

                    }
                    phaseSelect = phases.fourth;

                }

                break;

            case (phases.fourth):

                //Havai fisek activator
                if (finishObject.GetComponent<FinishHandler>().finishIndex == 1)
                {

                    for (int i = 0; i < finishVfx.Length; i++)
                    {
                        finishVfx[i].gameObject.SetActive(true);
                    }

                }
                if (finishObject.GetComponent<FinishHandler>().finishIndex == 9)
                {
                    CancelInvoke("RacePoisiton");
                    phaseSelect = phases.fifth;

                }
                break;

            case (phases.fifth):
                //finish operation
                FinishPanelOpen();
                break;
        }

        //Driving until the 1st racer finished
        if (isFirstPlaceFinded == false)
            Driving();

    }

    //Numerator executor
    private void RacePosition()
    {
        for (int i = 0; i < playerCount; i++)
        {

            StartCoroutine(ChangeRank(racers[i].GetComponent<CarScript>().carSign, signPositions[i]));

        }
    }

    //Change car ranks
    IEnumerator ChangeRank(GameObject racer, Vector3 signPosition)
    {
        float elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / 1f);
            racer.transform.position = Vector2.Lerp(racer.transform.position, signPosition, progress);
            yield return null;
        }

    }

    //Change the car signs
    IEnumerator ChangeCarSign(int i)
    {

        yield return new WaitForSeconds(0.05f);
        positionText[i].text = racers[i].GetComponent<CarScript>().name;

    }

    void Starter()
    {

        // Kinematic cars and ui activations
        if (isStartUiActive == true)
        {
            raceUI.SetActive(true);
            carSignsGO.SetActive(true);
            for (int i = 0; i < playerCount; i++)
            {

                racers[i].transform.position = new Vector3(0, 1.16f, racers[i].transform.position.z);
                firstInitPlayers[i].GetComponent<Rigidbody>().isKinematic = true;

            }
            isStartUiActive = false;

        }

        // Start countdown
        startTime -= Time.deltaTime;
        startCounter.GetComponentInChildren<TextMeshProUGUI>().text = "Race starts in " + ((int)startTime) + " seconds";

        if (startTime >= 0.1f)
        {
            raceStarted = false;
            startCounter.SetActive(true);

        }
        else if (preRaceProcess == true)    // inactivate countdown and start the race if the start time >= 0.1f
        {

            raceStarted = true;
            startCounter.SetActive(false);

        }

        // Race
        raceUI.gameObject.transform.localScale = Vector3.Lerp(raceUI.gameObject.transform.localScale, Vector3.one, Time.deltaTime * 10);
        startUI.gameObject.transform.position = Vector3.Lerp(startUI.gameObject.transform.position, new Vector3(-1000, startUI.gameObject.transform.position.y, startUI.gameObject.transform.position.z), Time.deltaTime * 2);
        skipToRace.gameObject.transform.localPosition = Vector3.Lerp(skipToRace.gameObject.transform.localPosition, new Vector3(2000, skipToRace.gameObject.transform.localPosition.y, skipToRace.gameObject.transform.localPosition.z), Time.deltaTime);

        if (startUI.gameObject.transform.position.x < -950)
        {

            startUI.SetActive(false);

        }

    }

    private void Driving()
    {
        //Activate the driving actions when the race started
        if (raceStarted)
        {
            for (int g = 0; g < firstInitPlayers.Length; g++)
            {

                firstInitPlayers[g].GetComponent<Rigidbody>().velocity = Vector3.Lerp(firstInitPlayers[g].GetComponent<Rigidbody>().velocity,
                new Vector3(racerValues[g].raceValueList[((int)raceTotalTime)], 0, 0), Time.deltaTime * 2);

            }
        }
    }
    [SerializeField] private Button timeScaleIncrease;
    [SerializeField] private Button timeScaleDecrease;


    void TimeScaleIncreaser()
    {
        Time.timeScale += 1;
    }
    void TimeScaleDecreaser()
    {
        Time.timeScale -= 1;
    }

    [SerializeField] private AudioSource startTransitionSound;
    void FixedUpdate()
    {

        startTransitionSound.volume += 0.006f;
        racers = racers.OrderBy(x => x.transform.position.x).ToArray();
        //  Marquee();

    }
    /*
        //Slide side panels
        void Marquee()
        {
            foreach (GameObject marq in marqueeObjects)
            {

                marq.transform.position -= new Vector3(Time.deltaTime * 10, 0, 0);

            }
            foreach (GameObject marq in marqueeObjects2)
            {

                marq.transform.position -= new Vector3(Time.deltaTime * 10, 0, 0);

            }
        }*/

    //Finish events
    void FinishPanelOpen()
    {

        finishTimer += Time.deltaTime;

        //Stop race actions and move object outside of the view
        if (finishTimer > 5 && finishTimer < 6f)
        {
            for (int i = 0; i < playerCount; i++)
            {

                StopCoroutine(ChangeRank(racers[i], signPositions[i]));

            }

            carSignsGO.transform.localPosition = new Vector3(Mathf.Lerp(carSignsGO.transform.localPosition.x, -1400, Time.deltaTime),
            carSignsGO.transform.localPosition.y,
            carSignsGO.transform.localPosition.z);


            raceUI.gameObject.transform.localScale = Vector3.Lerp(raceUI.gameObject.transform.localScale, Vector3.one * 2, Time.deltaTime * 10);

        }

        //Activate winner panel and session finished events
        if (finishTimer > 6 && finishTimer < 18 && check == false)
        {
            winnerEngineText.text=""+racers[9].GetComponent<CarBase>().carEngine;
            winnerNameText.text=""+racers[9].GetComponent<CarBase>().ownerUsername;
            winnerPowertrainText.text=""+racers[9].GetComponent<CarBase>().carDriveTrain;
            winnerTurboText.text=""+racers[9].GetComponent<CarBase>().carTurbo;
            winnerWeightText.text=""+racers[9].GetComponent<CarBase>().carWeight;
            winnerWheelText.text=""+racers[9].GetComponent<CarBase>().carWheelType;
            eventNameText.text=""+joinedRoomDetail.data.room_name;
            eventPropText.text=joinedRoomDetail.data.distance + " - "+ joinedRoomDetail.data.category_type;
            
            SessionCompleted();
            if (finishTimer > 8)
                check = true;

        }

        //Lobby panel activate
        if (finishTimer > 18)
        {
            if (lobbyBool == false)
            {

                lobbyBool = true;
                lobbyPanel.SetActive(true);

            }


        }

        // Finish car representation angles and adjustments
        if (winnerPanel.activeSelf == true && finishAnimTimer < 3)
        {

            //Get first placement gameobject reference and teleport to the presentation location
            if (isFirstPlaceFinded == false)
            {

                firstPlaceGO = racers[9].gameObject;
                firstPlaceGO.transform.position = new Vector3(currentRaceDistance + 100, firstPlaceGO.transform.position.y, -30);
                isFirstPlaceFinded = true;

            }

            // first place Gameobject's rigidbody and transform actions
            firstPlaceGO.GetComponent<Rigidbody>().velocity = Vector3.zero;
            firstPlaceGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            firstPlaceGO.GetComponent<Rigidbody>().useGravity = true;

            firstPlaceGO.transform.Translate(new Vector3(50 * Time.deltaTime / (10 * finishAnimTimer + 1), 0, 0), Space.World);
            firstPlaceGO.transform.eulerAngles += new Vector3(0, 20 * Time.deltaTime, 0);

            firstPlaceGO.transform.GetChild(1).transform.GetChild(0).transform.eulerAngles += new Vector3(0, Time.deltaTime * 10, 0);
            firstPlaceGO.transform.GetChild(1).transform.GetChild(1).transform.eulerAngles += new Vector3(0, Time.deltaTime * 10, 0);

            //The time when the rigid body will stop final presentation transformations.
            finishAnimTimer += Time.deltaTime;
            if (finishAnimTimer > 3)
                firstPlaceGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        }

    }

    //Race start presentation properties adjustment.
    public void RaceStartManagement()
    {

        if (preRaceProcess == false && startIncreaser < 10)
        {

            startIncreaser += 1;
            carNumberText.text = "# " + (startIncreaser);
            targetID.text = "" + firstInitPlayers[startIncreaser - 1].name;
            targetEngine.text = "" + firstInitPlayers[startIncreaser - 1].GetComponent<CarBase>().carEngine.ToString();
            targetDT.text = "" + firstInitPlayers[startIncreaser - 1].GetComponent<CarBase>().carDriveTrain.ToString();
            targetTurbo.text = "" + firstInitPlayers[startIncreaser - 1].GetComponent<CarBase>().carTurbo.ToString();
            targetWeight.text = "" + firstInitPlayers[startIncreaser - 1].GetComponent<CarBase>().carWeight.ToString();
            targetWheel.text = "" + firstInitPlayers[startIncreaser - 1].GetComponent<CarBase>().carWheelType.ToString();
            PreRaceProcessCamera();

            // Switch between the cars until the final car
            if (startIncreaser > 9)
            {
                StartCoroutine(Delay());
            }

        }

    }

    //Pre race process camera movements and led pingpong
    private void PreRaceProcessCamera()
    {

        myCamera.transform.position = new Vector3(Mathf.Lerp(myCamera.transform.position.x, firstInitPlayers[startIncreaser - 1].transform.position.x + 30, Time.deltaTime * 1.5f), Mathf.Lerp(myCamera.transform.position.y, 9f, Time.deltaTime * 1.5f), Mathf.Lerp(myCamera.transform.position.z, firstInitPlayers[startIncreaser - 1].transform.position.z, Time.deltaTime * 1.5f));
        myCamera.transform.eulerAngles = new Vector3(Mathf.Lerp(myCamera.transform.eulerAngles.x, 31.6f, Time.deltaTime * 1f), Mathf.Lerp(myCamera.transform.eulerAngles.y, 270f, Time.deltaTime * 1), Mathf.Lerp(myCamera.transform.eulerAngles.z, 0, Time.deltaTime * 1));
        firstInitPlayers[startIncreaser - 1].GetComponent<CarScript>().PingPongLed();

    }

    IEnumerator Delay()
    {

        yield return new WaitForSeconds(5f);
        CancelInvoke("RaceStartManagement");
        yield return new WaitForSeconds(.5f);
        preRaceProcess = true;


    }

    //Skip to the race start view
    public void SkipPreRace()
    {

        preRaceProcess = true;
        startIncreaser = 10;

    }

    void SessionCompleted()
    {

        winnerPanel.SetActive(true);
        raceUI.SetActive(false);
        carSignsGO.SetActive(false);
        startUI.SetActive(false);
        WinnerPlacements();

    }

    void WinnerPlacements()
    {

        //Slide racers final sign with time
        placements[0].transform.localPosition = Vector3.Lerp(placements[0].transform.localPosition, new Vector3(-585.6f, placements[0].transform.localPosition.y, 0), 3 * Time.deltaTime);
        placements[1].transform.localPosition = Vector3.Lerp(placements[1].transform.localPosition, new Vector3(-601.1f, placements[1].transform.localPosition.y, 0), 3 * Time.deltaTime);
        placements[2].transform.localPosition = Vector3.Lerp(placements[2].transform.localPosition, new Vector3(-616.6f, placements[2].transform.localPosition.y, 0), 3 * Time.deltaTime);

        for (int j = 0; j < 10; j++)
        {

            placements[j].transform.localPosition = Vector3.Lerp(placements[j].transform.localPosition, new Vector3(-623f, placements[j].transform.localPosition.y, 0), 3 * Time.deltaTime);

        }

    }

    /////////////////////////////////////////////////////////////////GET NFT TO RACE SCENE///////////////////////////////////////////////////////////////////
    void AdjustCarsAccordingToNFT()
    {


        allParticipantsInGame = _networkManager._eventManager.allParticipants;

        for (int i = 0; i < allParticipantsInGame.users.Count; i++)
        {
            racers[i].GetComponent<CarBase>().ownerUsername = allParticipantsInGame.users[i].username;
            racers[i].GetComponent<CarBase>().carUuid = allParticipantsInGame.users[i].uuid;
            racers[i].GetComponent<CarBase>().carName = allParticipantsInGame.users[i].name;
            racers[i].GetComponent<CarBase>().joinedRoomName = allParticipantsInGame.users[i].room_name;
            racers[i].GetComponent<CarBase>().carColor = allParticipantsInGame.users[i].attributes[0].value.ToString();
            racers[i].GetComponent<CarBase>().carRarity = allParticipantsInGame.users[i].attributes[1].value.ToString();
            racers[i].GetComponent<CarBase>().carLedColor = allParticipantsInGame.users[i].attributes[2].value.ToString();
            racers[i].GetComponent<CarBase>().carEngine = allParticipantsInGame.users[i].attributes[3].value.ToString();
            racers[i].GetComponent<CarBase>().carWeight = allParticipantsInGame.users[i].attributes[4].value.ToString();
            racers[i].GetComponent<CarBase>().carDriveTrain = allParticipantsInGame.users[i].attributes[5].value.ToString();
            racers[i].GetComponent<CarBase>().carTurbo = allParticipantsInGame.users[i].attributes[6].value.ToString();
            racers[i].GetComponent<CarBase>().carWheelType = allParticipantsInGame.users[i].attributes[7].value.ToString();
            racers[i].GetComponent<CarBase>().car_id = allParticipantsInGame.users[i].car_id;

            racers[i].GetComponent<CarBase>().ChangeAllCarsToNFT();
            for (int j = 0; j < raceInfos.data.Count; j++)
            {
                if (raceInfos.data[j].car_id == racers[i].GetComponent<CarBase>().car_id)
                {
                    racers[i].GetComponent<CarBase>().racerResult = raceInfos.data[j];
                }
            }
        }

    }
    void GetRoomDetailServer()
    {
        _networkManager.GetRoomDetailFromServer(_networkManager._eventManager.selectedRoomID, FailCallback, GetRoomDetailServerCallback);
    }

    private void GetRoomDetailServerCallback(string data)
    {
        joinedRoomDetail = JsonUtility.FromJson<GetRoomDetailServerResponse>(data);
        currentRaceDistance = joinedRoomDetail.data.distance;

        GenerateUserValues();
        for (int i = 0; i < racers.Length; i++)
        {
            racers[i].GetComponent<CarScript>().GenerateCarSpec();
        }
    }
    void GetRaceResultsFromServer()
    {
        _networkManager.StartRaceGetRaceInfoServer(_networkManager._eventManager.selectedRoomID, FailCallback, GetRaceResultsFromServerCallback);

    }

    private void GetRaceResultsFromServerCallback(string data)
    {
        raceInfos = JsonUtility.FromJson<RaceInfosFromServer>(data);
        AdjustCarsAccordingToNFT();
        GetRoomDetailServer();

    }

    void FailCallback(string error)
    {
        print(error);
    }
}

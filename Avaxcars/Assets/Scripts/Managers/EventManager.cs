using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using FirebaseWebGL.Examples.Database;




public class EventManager : MonoBehaviour
{

    private static EventManager instance = null;
    private NetworkManager _networkManager;
    public UIManager _uiManager;

    [Header("                              Event Variables")]
    private int currentPageIndex = 1;
    private int totalPage = 1;
    private int currentCategory = 2, currentRequiredFuel = 3, remainingEventObject;
    private bool isRightButton = false;
    public List<EventPage> allEvents = new List<EventPage>();
    public GetAllCategory getAllCategory;
    public Participants allParticipants;
    public GameObject[] allEventGameobjects;
    public GameObject[] allParticipantGameobjects;
    public string selectedRoomID;
    private int remainingTime = 10;


    [Header("                              Event UI Variables")]
    public Button getEventRoomButton, joinRoomButton, previousPageButton, nextPageButton, leaveFromRoomButton;
    public TMP_Text currentPageIndexText, remainingTimeText, roomNameText;
    public GameObject joinLeaveGO;
    public bool isInEvent = false;
    public bool isIn;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("EventManager").AddComponent<EventManager>();
            }

            return instance;
        }
    }
    private void OnEnable()
    {
        instance = this;
    }

    void Start()
    {
        _networkManager = NetworkManager.Instance;
        _uiManager = UIManager.Instance;
        DontDestroyOnLoad(this.gameObject);
        AddButtonListeners();

    }


    public void AddButtonListeners()
    {
        getEventRoomButton.onClick.AddListener(() => StartEventLoad());
        joinRoomButton.onClick.AddListener(() => JoinRoomServer());
        previousPageButton.onClick.AddListener(() => PreviousPageSelectedRank());
        nextPageButton.onClick.AddListener(() => NextPageSelectedRank());
        leaveFromRoomButton.onClick.AddListener(() => LeaveRoomServer());

    }

    void StartEventLoad()
    {
        GetAllCategoriesServer();

    }
    public void GetRequestedPageServer()
    {
        _networkManager.GetRequestedPageFromServer(currentCategory, currentPageIndex, FailCallback, GetRequestedPageServerCallback);
    }

    private void GetRequestedPageServerCallback(string data)
    {
        var response = JsonUtility.FromJson<AllRoomResponse>(data);
        print(data);
        print(response.data.Count);
        print((response.meta.totalCount % 10));
        print(totalPage = (response.meta.totalCount / 10) + 1);
        if ((response.meta.totalCount % 10) != 0)
        {
            totalPage = (response.meta.totalCount / 10) + 1;
        }

        for (int i = 0; i < response.data.Count; i++)
        {
            EventPage tempPage = new EventPage();
            //Assign Variables
            tempPage.categoryType = response.data[i].category_type;
            tempPage.eventName = response.data[i].room_name;
            tempPage.participantCount = response.data[i].participantCount;
            tempPage.raceDistance = response.data[i].distance;
            tempPage.roomID = response.data[i].room_id;
            tempPage.requiredFuel = currentRequiredFuel;
            tempPage.axpPoints = response.data[i].axp_points;

            //Assign UI
            tempPage.eventLinePrefab = allEventGameobjects[i];


            tempPage.eventNameText = tempPage.eventLinePrefab.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
            tempPage.requiredFuelText = tempPage.eventLinePrefab.transform.GetChild(2).gameObject.GetComponent<TMP_Text>();
            tempPage.raceDistanceText = tempPage.eventLinePrefab.transform.GetChild(3).gameObject.GetComponent<TMP_Text>();
            tempPage.participantCountText = tempPage.eventLinePrefab.transform.GetChild(4).gameObject.GetComponent<TMP_Text>();
            tempPage.eventStarterButton = tempPage.eventLinePrefab.transform.GetChild(5).gameObject.GetComponent<Button>();
            tempPage.axpPointsText = tempPage.eventLinePrefab.transform.GetChild(6).gameObject.GetComponent<TMP_Text>();
            //Print room spects
            tempPage.eventNameText.text = tempPage.eventName;
            tempPage.participantCountText.text = tempPage.participantCount.ToString() + "/10";
            tempPage.raceDistanceText.text = tempPage.raceDistance.ToString();
            tempPage.requiredFuelText.text = tempPage.requiredFuel.ToString();
            tempPage.axpPointsText.text = tempPage.axpPoints.ToString();

            //Add Button Listeners 
            tempPage.eventStarterButton.onClick.RemoveAllListeners();
            tempPage.eventStarterButton.onClick.AddListener(delegate { EnterGarageWithRoomID(tempPage.roomID, tempPage.participantCount); });

            //Add Instantiates To List
            allEventGameobjects[i] = tempPage.eventLinePrefab;
            allEventGameobjects[i].SetActive(true);

            //Add Event To List
            allEvents.Add(tempPage);

            //GoEventScreen
            ViewManager.Show<EventView>();

        }


        remainingEventObject = response.data.Count % 10;
        if (remainingEventObject != 0)
        {
            for (int i = remainingEventObject; i < 10; i++)
            {
                allEventGameobjects[i].SetActive(false);
            }
        }

        CheckButtonStatus();
    }

    public void EnterGarageWithRoomID(string roomID, int roomParticipantCount)
    {

        selectedRoomID = roomID;
        CancelRoomsRefresh();
        ViewManager.Show<GarageView>();
        //        DatabaseHandler.Instance.EventMessagesButton.interactable = true;
        print(roomID);

    }

    void JoinRoomServer()
    {
        _networkManager.JoinToRoomServer(selectedRoomID, _uiManager.CarsInstantiates[_uiManager.currentIndex].GetComponent<CarBase>().carUuid, FailCallback, JoinRoomServerCallback);
    }

    private void JoinRoomServerCallback(string data)
    {
        var json = JsonUtility.FromJson<RegisterRoomResponse>(data);
        print(json.is_full);

        joinRoomButton.gameObject.SetActive(false);
        leaveFromRoomButton.gameObject.SetActive(true);

        _networkManager.IsRoomFullSocketConnection(json.is_full);
    }

    void LeaveRoomServer()
    {
        _networkManager.LeaveFromServer(selectedRoomID, _uiManager.CarsInstantiates[_uiManager.currentIndex].GetComponent<CarBase>().carUuid, FailCallback, LeaveRoomServerCallback);
    }

    private void LeaveRoomServerCallback(string data)
    {
        print(data);

        joinRoomButton.gameObject.SetActive(true);
        leaveFromRoomButton.gameObject.SetActive(false);
    }


    void GetAllCategoriesServer()
    {
        _networkManager.GetAllCategoriesFromServer(FailCallback, GetAllCategoriesServerCallback);
    }

    public void GetAllCategoriesServerCallback(string data)
    {
        var json = JsonUtility.FromJson<GetAllCategory>(data);
        getAllCategory = json;
        print(json.success);
        InvokeRepeating("GetRequestedPageServer", 0f, 30f);
    }

    void GetParticipantOnRoomServer()
    {
        _networkManager.GetParticipantOnRoomFromServer(selectedRoomID, FailCallback, GetParticipantOnRoomServerCallback);
    }

    public void GetParticipantOnRoomServerCallback(string data)
    {
        allParticipants = JsonUtility.FromJson<Participants>(data);

        for (int i = 0; i < allParticipants.users.Count; i++)
        {
            /////Username
            allParticipantGameobjects[i].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text = allParticipants.users[i].username;
            /* IPFS*/
            string[] uriHttps = allParticipants.users[i].image.Split("//"[1]);
            string uri = "https://ipfs.io/ipfs/" + uriHttps[2] + "/" + uriHttps[3];
            //
            StartCoroutine(DownloadImage(uri, allParticipantGameobjects[i]));
            /////Attributes
            allParticipantGameobjects[i].transform.GetChild(1).transform.GetChild(2).GetComponent<TMP_Text>().text = allParticipants.users[i].attributes[3].value.ToString();
            allParticipantGameobjects[i].transform.GetChild(1).transform.GetChild(3).GetComponent<TMP_Text>().text = allParticipants.users[i].attributes[5].value.ToString();
            allParticipantGameobjects[i].transform.GetChild(1).transform.GetChild(4).GetComponent<TMP_Text>().text = allParticipants.users[i].attributes[6].value.ToString();
            allParticipantGameobjects[i].transform.GetChild(1).transform.GetChild(5).GetComponent<TMP_Text>().text = allParticipants.users[i].attributes[3].value.ToString();
            allParticipantGameobjects[i].transform.GetChild(1).transform.GetChild(6).GetComponent<TMP_Text>().text = allParticipants.users[i].attributes[4].value.ToString();
            allParticipantGameobjects[i].transform.GetChild(1).transform.GetChild(7).GetComponent<TMP_Text>().text = allParticipants.users[i].attributes[7].value.ToString();


        }

        roomNameText.text = allParticipants.users[0].room_name;
    }

    IEnumerator DownloadImage(string MediaUrl, GameObject carTumbnail)
    {
        if (isIn == false)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                Debug.Log(request.error);
            else
            if (isIn == false)
            {
                carTumbnail.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<RawImage>().texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }
        }

    }



    void FailCallback(string error)
    {
        print(error);
    }

    public void DeleteAllRoomObjects()
    {
        for (int i = 0; i < allEventGameobjects.Length; i++)
        {
            allEventGameobjects[i].SetActive(false);
        }
    }

    void NextPageSelectedRank()
    {

        print(totalPage);
        print(currentPageIndex);
        if (currentPageIndex < totalPage)
        {
            isRightButton = true;
            currentPageIndex++;
            DeleteAllRoomObjects();
            GetRequestedPageServer();
        }
    }

    void PreviousPageSelectedRank()
    {

        print(currentPageIndex);
        print(totalPage);
        if (currentPageIndex > 1)
        {
            isRightButton = false;
            currentPageIndex--;
            DeleteAllRoomObjects();
            GetRequestedPageServer();
        }
    }

    void CheckButtonStatus()
    {
        if (totalPage == 1)
        {
            previousPageButton.interactable = false;
            nextPageButton.interactable = false;
        }
        else
        {
            if (isRightButton)
            {
                if (currentPageIndex == totalPage)
                {
                    nextPageButton.interactable = false;
                    previousPageButton.interactable = true;
                }
                else
                {
                    previousPageButton.interactable = true;
                }
            }
            else
            {
                if (currentPageIndex == 1)
                {
                    previousPageButton.interactable = false;
                    nextPageButton.interactable = true;
                }
                else
                {
                    nextPageButton.interactable = true;
                }
            }
        }

        currentPageIndexText.text = "" + currentPageIndex;
    }

    public void ChangeSelectedCategory(string selectedCategory)
    {
        currentPageIndex = 1;
        isRightButton = false;
        DeleteAllRoomObjects();

        for (int i = 0; i < getAllCategory.data.Count; i++)
        {
            if (selectedCategory == getAllCategory.data[i].title)
            {
                currentCategory = getAllCategory.data[i].id;
                currentRequiredFuel = getAllCategory.data[i].entry_fuel_fee;
            }
        }

        GetRequestedPageServer();
    }


    public void CancelRoomsRefresh()
    {
        LeaveRoomServer();
        CancelInvoke("GetRequestedPageServer");
    }

    public void EnterWaitingArea()
    {

        ViewManager.Show<WaitingScreenView>();
        GetParticipantOnRoomServer();
        StartCoroutine(Countdown(remainingTime));

    }

    void EnterRaceScene()
    {
        isIn = true;
        SceneManager.LoadScene(Strings.RaceScene);
    }

    IEnumerator Countdown(int seconds)
    {
        int counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
            remainingTimeText.text = "Remaining Time : " + counter;

        }
        EnterRaceScene();
    }

}




[Serializable]
public class EventPage
{
    public string eventName;
    public int requiredFuel;
    public int raceDistance;
    public int axpPoints;
    public int participantCount;
    public string categoryType;
    public string roomID;
    public GameObject eventLinePrefab;
    public Button eventStarterButton;
    public TMP_Text requiredFuelText;
    public TMP_Text axpPointsText;
    public TMP_Text raceDistanceText;
    public TMP_Text participantCountText;
    public TMP_Text eventNameText;

}

[Serializable]
public class CategoryInfo
{
    public int id;
    public string title;
    public int entry_fuel_fee;
    public int min_xp_amount;
    public int max_xp_amount;
}

[Serializable]
public class GetAllCategory
{
    public bool success;
    public List<CategoryInfo> data;
}

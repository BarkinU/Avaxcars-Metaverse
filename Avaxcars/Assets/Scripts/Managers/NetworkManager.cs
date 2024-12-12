using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Socket.Quobject.SocketIoClientDotNet.Client;
//using Firesplash.UnityAssets.SocketIO;
using UnityEngine.UI;

#if HAS_JSON_NET
//If Json.Net is installed, this is required for Example 6. See documentation for information on how to install Json.NET
//Please note that most recent unity versions bring Json.Net with them by default, you would only need to enable the compiler flag as documented.
using Newtonsoft.Json;
#endif

public class NetworkManager : MonoBehaviour
{
    [Header("User Register Inputs")]
    private static NetworkManager instance = null;
    public delegate void NetworkCallback(string text);
    private NetworkCallback _registerSuccessCallback;
    private NetworkCallback _registerFailCallback;
    private GameManager _gameManager;
    public EventManager _eventManager;

    private string BaseURL = "https://uat-api.avaxcars.com/api-v1";
    private QSocket socket;
    //private string BaseURL = "http://192.168.1.28:5000/api-v1";
    private RoomID response = new RoomID();


    // [Header("SocketIO Settings")]
    //public SocketIOCommunicator sioCom;

    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("NetworkManager").AddComponent<NetworkManager>();
            }

            return instance;
        }
    }

    private void OnEnable()
    {
        instance = this;

    }

    void OnDestroy()
    {
        // sioCom.Instance.On("disconnect", (string payload) =>
        // {
        //     if (payload.Equals("io server disconnect"))
        //     {
        //         Debug.Log("Disconnected from server.");
        //     }
        //     else
        //     {
        //         Debug.LogWarning("We have been unexpecteldy disconnected. This will cause an automatic reconnect. Reason: " + payload);
        //     }
        // });
    }

    void Start()
    {

        _gameManager = GameManager.Instance;
        _eventManager = EventManager.Instance;
        DontDestroyOnLoad(this.gameObject);

    }

    void Update()
    {
        if (response.is_full == true)
        {
            if (_eventManager.isIn == false)
            {
                _eventManager.EnterWaitingArea();
                response.is_full = false;
            }
        }
    }

    public void Authenticate(bool isLoginOrRegister, NetworkCallback failCallback, NetworkCallback successCallback)
    {

        if (PlayerPrefs.GetString(PrefKeys._sessionTokenPref) != "")
        {
            var sessionToken = PlayerPrefs.GetString(PrefKeys._sessionTokenPref);
            Login(failCallback, successCallback);
        }
        else
        {
            if (isLoginOrRegister)
            {
                Login(failCallback, successCallback);
            }
            else
            {
                GeoIP(failCallback, successCallback);

            }
        }
    }

    public void Login(NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("email", _gameManager.loginEmail.text);
        formData.AddField("password", _gameManager.loginPassword.text);
        StartCoroutine(_PostRequest(BaseURL + "/auth/login", formData, failCallback, successCallback));
    }


    public void GeoIP(NetworkCallback failCallback, NetworkCallback successCallback)
    {
        _registerSuccessCallback = successCallback;
        _registerFailCallback = failCallback;
        StartCoroutine(_GetRequest("https://freegeoip.app/json/", failCallback, Register));
    }

    public void GetUserCountry(NetworkCallback failCallback, NetworkCallback successCallback)
    {
        StartCoroutine(_GetRequest("https://freegeoip.app/json/", failCallback, successCallback));
    }


    void Register(string data)
    {
        print(data);
        Debug.Log("Registering");
        DataGeoIP response = JsonUtility.FromJson<DataGeoIP>(data);
        WWWForm formData = new WWWForm();

        print(_gameManager.registerEmail.text);
        formData.AddField("ip", response.ip);
        formData.AddField("country_code", response.country_code);
        formData.AddField("country_name", response.country_name);
        formData.AddField("region_code", response.region_code);
        formData.AddField("region_name", response.region_name);
        formData.AddField("city", response.city);
        formData.AddField("zip_code", response.zip_code);
        formData.AddField("time_zone", response.time_zone);
        formData.AddField("latitude", response.latitude);
        formData.AddField("longitude", response.longitude);
        formData.AddField("email", _gameManager.registerEmail.text);
        formData.AddField("password", _gameManager.registerPassword.text);
        formData.AddField("passwordRepeat", _gameManager.registerVerifyPassword.text);
        formData.AddField("first_name", _gameManager.firstName.text);
        formData.AddField("last_name", _gameManager.lastName.text);
        formData.AddField("country", _gameManager.countryText.text);
        formData.AddField("birth_day", _gameManager.birthDay.text);
        formData.AddField("birth_month", _gameManager.birthMonth.text);
        formData.AddField("birth_year", _gameManager.birthYear.text);

        StartCoroutine(
            _PostRequest(BaseURL + "/auth/register", formData, _registerFailCallback, _registerSuccessCallback));
    }

    public void CreateUsername(string username, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("username", username);
        StartCoroutine(_PostRequest(BaseURL + "/auth/addUsername", formData, failCallback, successCallback));
    }


    public void AddWalletIDToServer(string data, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();

        formData.AddField("walletId", data);
        StartCoroutine(_PostRequest(BaseURL + "/wallet/addWalletId", formData, failCallback, successCallback));
    }

    public void GetAllNFTFromServer(NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();

        formData.AddField("", "");
        StartCoroutine(_PostRequest(BaseURL + "/wallet/nft/getAllNfts", formData, failCallback, successCallback));
    }


    public void GetNftByIndexFromServer(string data, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("index", data);
        StartCoroutine(_PostRequest(BaseURL + "/wallet/nft/getNftOneByIndex", formData, failCallback, successCallback));
    }

    public void GetNftByUuidFromServer(string data, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("uuid", data);
        StartCoroutine(_PostRequest(BaseURL + "/wallet/nft/getNftOneByUuid", formData, failCallback, successCallback));
    }

    public void GetUserFromServer(NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        StartCoroutine(_PostRequest(BaseURL + "/auth/getUser", formData, failCallback, successCallback));
    }

    public void UpdatePassword(string oldPassword, string newPassword, string newPasswordVerify, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("oldPassword", oldPassword);
        formData.AddField("newPassword", newPassword);
        formData.AddField("newPasswordAgain", newPasswordVerify);

        StartCoroutine(_PostRequest(BaseURL + "/auth/updatePassword", formData, failCallback, successCallback));
    }

    public void SendResetLinkFromServer(string mailToChangePassword, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("email", mailToChangePassword);

        StartCoroutine(_PostRequest(BaseURL + "/auth/forgotPassword/sendEmail", formData, failCallback, successCallback));
    }

    public void UpdateCarSpec(string carUuid, string specType, bool upgradable, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("car_uuid", carUuid);
        formData.AddField("spec_name", specType);
        formData.AddField("bool", upgradable.ToString());

        StartCoroutine(_PostRequest(BaseURL + "/cars/specs/upgradeToSpec", formData, failCallback, successCallback));
    }

    public void GetRequestedPageFromServer(int selectedCategoryID, int pageIndex, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("page", pageIndex);
        formData.AddField("category_id", selectedCategoryID);

        StartCoroutine(_PostRequest(BaseURL + "/event/getAllRoomByPagination", formData, failCallback, successCallback));
    }

    public void JoinToRoomServer(string roomID, string carUuid, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("room_id", roomID);
        formData.AddField("car_id", carUuid);
        StartCoroutine(_PostRequest(BaseURL + "/event/joinRoom", formData, failCallback, successCallback));
    }


    public void LeaveFromServer(string roomID, string carUuid, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("room_id", roomID);
        formData.AddField("car_id", carUuid);
        StartCoroutine(_PostRequest(BaseURL + "/event/leaveRoom", formData, failCallback, successCallback));
    }


    public void GetAllCategoriesFromServer(NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("", "");
        StartCoroutine(_PostRequest(BaseURL + "/event/category/getAllCategory", formData, failCallback, successCallback));
    }

    public void GetParticipantOnRoomFromServer(string roomID, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("room_id", roomID);
        StartCoroutine(_PostRequest(BaseURL + "/event/getParticipantsOnRoom", formData, failCallback, successCallback));
    }

    public void GetAllMissionFromServer(NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("", "");
        StartCoroutine(_PostRequest(BaseURL + "/mission/getMissionGrouping", formData, failCallback, successCallback));
    }

    public void GetMissionDetailFromServer(string missionID, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("engine_id", missionID);
        StartCoroutine(_PostRequest(BaseURL + "/mission/getMissionDetail", formData, failCallback, successCallback));
    }

    public void GotoMissionFromServer(string missionID, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("id", missionID);
        StartCoroutine(_PostRequest(BaseURL + "/mission/goToMission", formData, failCallback, successCallback));
    }

    public void AddRewardMissionFromServer(string missionID, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("engine_id", missionID);
        StartCoroutine(_PostRequest(BaseURL + "/mission/fuel/addReward", formData, failCallback, successCallback));
    }

    public void GetUserFuelFromServer(NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("", "");
        StartCoroutine(_PostRequest(BaseURL + "/mission/fuel/getFuels", formData, failCallback, successCallback));
    }

    public void UpgradeEngineRequestFromServer(string carUuid, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("car_uuid", carUuid);
        StartCoroutine(_PostRequest(BaseURL + "/spec/upgradeEngine", formData, failCallback, successCallback));
    }

    public void UpgradeDriveTrainRequestFromServer(string carUuid, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("car_uuid", carUuid);
        StartCoroutine(_PostRequest(BaseURL + "/spec/upgradeDrivetrain", formData, failCallback, successCallback));
    }

    public void UpgradeTurboRequestFromServer(string carUuid, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("car_uuid", carUuid);
        StartCoroutine(_PostRequest(BaseURL + "/spec/upgradeTurbo", formData, failCallback, successCallback));
    }

    public void StartRaceGetRaceInfoServer(string roomID, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("room_id", roomID);
        StartCoroutine(_PostRequest(BaseURL + "/race/raceNow", formData, failCallback, successCallback));
    }

    public void GetRoomDetailFromServer(string roomID, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("room_id", roomID);
        StartCoroutine(_PostRequest(BaseURL + "/event/getRoomDetail", formData, failCallback, successCallback));
    }

    public void GetNFTSpecsByUuid(string carUuid, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("uuid", carUuid);
        StartCoroutine(_PostRequest(BaseURL + "/wallet/nft/getSpecsByUuid", formData, failCallback, successCallback));
    }


    public void IsRoomFullSocketConnection(bool is_full)
    {
        socket = IO.Socket("https://uat-api.avaxcars.com");
        //  socket = IO.Socket("http://192.168.1.28:5000");
        RoomID roomInfo = new RoomID();

        roomInfo.room_id = _eventManager.selectedRoomID;
        roomInfo.is_full = is_full;

        string json = JsonUtility.ToJson(roomInfo);

        socket.On(QSocket.EVENT_CONNECT, () =>
        {
            Debug.Log("Connected");
            socket.Emit("roomIsFull", json);
        });

        socket.On("roomIsFull", data =>
        {
            response = JsonUtility.FromJson<RoomID>(data.ToString());
            print(response.is_full);
            Debug.LogWarning("data : " + data);
        });

        // RoomID roomInfo = new RoomID();
        // roomInfo.room_id = _eventManager.selectedRoomID;
        // roomInfo.is_full = is_full;
        // string json = JsonUtility.ToJson(roomInfo);

        // sioCom.Instance.On("connect", (string data) =>
        // {
        //     Debug.Log("LOCAL: Hey, we are connected!");
        //     Debug.Log("Socket.IO Connected. Doing work...");

        //     //NOTE: All those emitted and received events (except connect and disconnect) are made to showcase how this asset works. The technical handshake is done automatically.



        //     //First of all we knock at the servers door
        //     //EXAMPLE 1: Sending an event without payload data
        //     sioCom.Instance.Emit("roomIsFull", json);
        // });

        // //EXAMPLE 5: Listening for an event with JSON Object payload
        // sioCom.Instance.On("roomIsFull", (string payload) =>
        // {
        //     RoomID response = JsonUtility.FromJson<RoomID>(payload);
        //     Debug.Log("Received the POD name from the server. Upadting UI. Oh! It's  by the way.");
        //     Debug.Log("I talked to " + roomInfo);

        //     //Let's ask for random numbers (example 6 below)
        //     // sioCom.Instance.Emit("SendNumbers");
        // });
    }




    #region HTTP Requests
    IEnumerator _GetRequest(string BaseURL, NetworkCallback FailCallback, NetworkCallback SuccessCallback)
    {
        // https://www.google.com?region=tr&lan=en
        using (UnityWebRequest webRequest = UnityWebRequest.Get(BaseURL))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = BaseURL.Split('/');
            int page = pages.Length - 1;

            var error = webRequest.error;
            var response = webRequest.downloadHandler.text;
            var webRequestResult = webRequest.result;
            print(webRequestResult);
            webRequest.Dispose();

            switch (webRequestResult)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + error);
                    FailCallback(response);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + error);
                    Debug.Log("Error message: " + response);
                    FailCallback(response);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + response);
                    print(response);
                    print(pages[page]);
                    SuccessCallback?.Invoke(response);
                    break;
            }
        }
    }

    IEnumerator _PostRequest(string BaseURL, WWWForm formData, NetworkCallback FailCallback, NetworkCallback SuccessCallback)
    {
        Debug.Log("Posting: " + BaseURL);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(BaseURL, formData))
        {
            webRequest.SetRequestHeader("AUTHORIZATION", PlayerPrefs.GetString(PrefKeys._sessionTokenPref));
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = BaseURL.Split('/');
            int page = pages.Length - 1;

            var error = webRequest.error;
            var response = webRequest.downloadHandler.text;
            var webRequestResult = webRequest.result;
            print(webRequestResult);
            webRequest.Dispose();



            switch (webRequestResult)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + error);
                    FailCallback?.Invoke(response);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + error);
                    Debug.Log("Error message: " + response);
                    FailCallback?.Invoke(response);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + response);
                    SuccessCallback?.Invoke(response);
                    break;
            }
        }
    }

    /* IEnumerator _PutRequest(string BaseURL, string raw, NetworkCallback FailCallback, NetworkCallback SuccessCallback)
     {
         using (UnityWebRequest webRequest = UnityWebRequest.Put(BaseURL, raw))
         {
             // Request and wait for the desired page.
             yield return webRequest.SendWebRequest();

             string[] pages = BaseURL.Split('/');
             int page = pages.Length - 1;

             var error = webRequest.error;
             var response = webRequest.downloadHandler.text;
             var webRequestResult = webRequest.result;
             webRequest.Dispose();

             switch (webRequestResult)
             {
                 case UnityWebRequest.Result.ConnectionError:
                 case UnityWebRequest.Result.DataProcessingError:
                     Debug.LogError(pages[page] + ": Error: " + error);
                     FailCallback(response);
                     break;
                 case UnityWebRequest.Result.ProtocolError:
                     Debug.LogError(pages[page] + ": HTTP Error: " + error);
                     FailCallback(response);
                     break;
                 case UnityWebRequest.Result.Success:
                     Debug.Log(pages[page] + ":\nReceived: " + response);
                     SuccessCallback(response);
                     break;
             }
         }
     }*/

    public IEnumerator FetchNFTToServer(CarSpectIngame data, NetworkCallback failCallback, NetworkCallback successCallback)
    {
        Debug.Log("Fething All NFTs");
        string output = JsonConvert.SerializeObject(data);
        print(output);


        var webRequest = new UnityWebRequest(BaseURL + "/wallet/nft/fetchNfts", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(output);
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("AUTHORIZATION", PlayerPrefs.GetString(PrefKeys._sessionTokenPref));
        yield return webRequest.SendWebRequest();

        string[] pages = BaseURL.Split('/');
        int page = pages.Length - 1;

        var error = webRequest.error;
        var response = webRequest.downloadHandler.text;
        var webRequestResult = webRequest.result;
        print(webRequestResult);
        webRequest.Dispose();

        switch (webRequestResult)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + error);
                failCallback(response);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + error);
                Debug.Log("Error message: " + response);
                failCallback(response);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(pages[page] + ":\nReceived: " + response);
                print(pages[page]);
                successCallback?.Invoke(response);
                break;
        }

    }
    #endregion


}
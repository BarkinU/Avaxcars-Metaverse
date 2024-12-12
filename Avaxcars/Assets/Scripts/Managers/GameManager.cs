using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using FirebaseWebGL.Examples.Database;
public class GameManager : MonoBehaviour
{
    [Header("                           Singleton Variables")]
    private static GameManager instance = null;
    private NetworkManager _networkManager;
    private UIManager _uiManager;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            }

            return instance;
        }
    }


    [Header("                           Register Variables")]
    public TMP_InputField registerEmail;
    public TMP_InputField registerPassword;
    public TMP_InputField registerVerifyPassword;
    public Button registerButton;
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField countryText;
    public TMP_InputField birthDay;
    public TMP_InputField birthMonth;
    public TMP_InputField birthYear;

    [Header("                           Login Variables")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;
    public Button loginButton;
    public Button forgotMyPassword;
    public bool isLoginOrRegister = false;

    [Header("                           Username Variables")]
    public TMP_InputField createUsername;
    public Button createUsernameButton;

    [Header("                           Change Password Variables")]
    public TMP_InputField oldPassword;
    public TMP_InputField newPassword;
    public TMP_InputField newPasswordVerify;
    public Button changePasswordButton;

    [Header("                           Change Password Variables")]
    public TMP_InputField mailToChangePassword;
    public Button sendResetPasswordButton;
    public TMP_Text resetLinkServerResponse;

    [Header("                           Fuel Variables")]
    [SerializeField]
    private TMP_Text fuelText; //,hydrogenFuelCountText, solarFuelCountText, ionFuelCountText, nuclearFuelCountText;
    [SerializeField]
    private int hydrogenCount = 0, solarCount = 0, ionCount = 0, nuclearCount = 0;
    [SerializeField]
    private double hydrogenRemainingTime = 0, solarRemainingTime = 0, ionRemainingTime = 0, nuclearRemainingTime = 0;

    [Header("                           Menu Car Variables")]
    public CarSpectIngame carSpect;

    public AllNFTServerResponse allCar;
    [Header("                           Camera Variables")]
    public MoveCameraBetweenPositions camTransition;

    [Header("                           Random Other Class Variables")]
    public static bool isMenuCameraFree = false;
    [Header("                           Missions Variables")]
    [SerializeField] private float popUpDuration = 0.5f;
    private bool isPopUpDone = false;
    private int selectedMissionID = 0, currentFuelId = 0;
    [SerializeField] private GameObject hydrogenStartMission, hydrogenGetReward, hydrogenNotification, hydrogenRemainingTimeGO, solarStartMission, solarGetReward, solarNotification, solarRemainingTimeGO, ionStartMission, ionGetReward, ionNotification, ionRemainingTimeGO, nuclearStartMission, nuclearGetReward, nuclearNotification, nuclearRemainingTimeGO;
    [SerializeField] private Button[] hydrogenMissionButtons;
    [SerializeField] private Button[] solarMissionButtons;

    [SerializeField] private Button[] ionMissionButtons;

    [SerializeField] private Button[] nuclearMissionButtons;
    public string userCountry = "TR";
    public Root missionGroups;

    [Header("                           Error Variables")]
    public GameObject errorMessageGO, chatPanel;
    [Header("                           Top Panel Variables")]
    [SerializeField] private TMP_Text usernameText;
    [Header("                           User Variables")]
    public GetUserResponse userInformation = new GetUserResponse();
    public void AddButtonListeners()
    {
        registerButton.onClick.AddListener(() => Authenticate());
        loginButton.onClick.AddListener(() => Authenticate());
        forgotMyPassword.onClick.AddListener(() => ForgotMyPasswordOpen());
        createUsernameButton.onClick.AddListener(() => CreateUsernameServer());
        changePasswordButton.onClick.AddListener(() => UpdatePasswordServer());
        sendResetPasswordButton.onClick.AddListener(() => ResetPasswordLinkServer());
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
        CheckUserData();
    }


    void ForgotMyPasswordOpen()
    {
        ViewManager.Show<ForgotPasswordView>();
    }

    void CheckUserData()
    {
        if (PlayerPrefs.GetString(PrefKeys._sessionTokenPref) != "")
        {
            var sessionToken = PlayerPrefs.GetString(PrefKeys._sessionTokenPref);
            GetUserServer();
        }
        else
        {
            ViewManager.Show<SplashScreenView>();
        }

    }

    void Authenticate()
    {
        _networkManager.Authenticate(isLoginOrRegister, AuthFailCallback, AuthCallback);

    }


    void AuthFailCallback(string data) //Retry auth through register
    {
        Debug.Log("AuthFailCallback");
        PlayerPrefs.SetString(PrefKeys._sessionTokenPref, "");
        _networkManager.Authenticate(isLoginOrRegister, FailCallback, AuthCallback);
    }

    void FailCallback(string data)
    {
        var response = JsonUtility.FromJson<FailCallbackError>(data);
        errorMessageGO.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = response.msg;
        errorMessageGO.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => FadeOut(errorMessageGO));
        FadeIn(errorMessageGO);

        print(data);
    }

    void AuthCallback(string data)
    {
        Debug.Log("Auth Success");
        var json = JsonUtility.FromJson<DataAuthResponse>(data);
        Debug.Log("AccesToken: " + json.jwt);
        PlayerPrefs.SetString(PrefKeys._sessionTokenPref, json.jwt);
        PlayerPrefs.SetString(PrefKeys._uuidPref,
            json.data.user.uuid); //THIS NEEDS TO CHANGE FOR LEADERBOARD IMPLEMENTATION
        PlayerPrefs.Save();
        GetUserServer();
    }
    private void CreateUsernameServer()
    {
        _networkManager.CreateUsername(createUsername.text, FailCallback, CreateUsernameServerCallback);
    }

    void CreateUsernameServerCallback(string data)
    {
        var json = JsonUtility.FromJson<UsernameResponse>(data);

    }

    public void FetchNFTServer()
    {
        StartCoroutine(_networkManager.FetchNFTToServer(carSpect, FailCallback, FetchNFTServerCallback));
    }

    void FetchNFTServerCallback(string data)
    {
        print(data);
        //  var json = JsonUtility.FromJson<CarAttributesInGame>(data);
        GetAllNFTServer();
    }

    public void GetAllNFTServer()
    {
        _networkManager.GetAllNFTFromServer(FailCallback, GetAllNFTServerCallback);
    }

    void GetAllNFTServerCallback(string data)
    {
        allCar = JsonUtility.FromJson<AllNFTServerResponse>(data);
        print(allCar);
        print(allCar.data.Count);

        ViewManager.Show<MainMenuView>();
        chatPanel.SetActive(true);
        DropDownOnValueChanged(0);
        _uiManager.TransferAllNFTsToGame();
        camTransition.ChangeCameraPositionByIndex(0);
        _uiManager.GameOpening();
       // DatabaseHandler.Instance.NewMessageSection();
    }

    public void AddWalletIDServer(string data)
    {
        _networkManager.AddWalletIDToServer(data, FailCallback, AddWalletIDServerCallback);
    }

    void AddWalletIDServerCallback(string data)
    {
        var json = JsonUtility.FromJson<AddWalletIDServerResponse>(data);
        print(json);
    }
    public void GetNftsByIndexServer(string data)
    {
        _networkManager.GetNftByIndexFromServer(data, FailCallback, GetNftsByIndexServerCallback);
    }

    void GetNftsByIndexServerCallback(string data)
    {
        var json = JsonUtility.FromJson<GetNftByIndexServerResponse>(data);
        print(json);
        print(json.success);
    }

    public void GetNftsByUuidServer(string data)
    {
        _networkManager.GetNftByUuidFromServer(data, FailCallback, GetNftsByUuidServerCallback);
    }

    void GetNftsByUuidServerCallback(string data)
    {
        var json = JsonUtility.FromJson<GetNftByUuidServerResponse>(data);
        print(json);
        print(json.success);
    }

    public void GetUserServer()
    {
        _networkManager.GetUserFromServer(FailCallback, GetUserServerCallback);
    }

    void GetUserServerCallback(string data)
    {
        userInformation = JsonUtility.FromJson<GetUserResponse>(data);
        usernameText.text = userInformation.data.user.username;
        print(userInformation.data.user.username);
        if (userInformation.data.user.username != "")
        {
            ViewManager.Show<WalletConnectView>();
        }
        else
        {
            ViewManager.Show<UsernameView>();
        }
    }

    public void UpdatePasswordServer()
    {
        _networkManager.UpdatePassword(oldPassword.text, newPassword.text, newPasswordVerify.text, FailCallback, UpdatePasswordServerCallback);
    }

    void UpdatePasswordServerCallback(string data)
    {
        var json = JsonUtility.FromJson<UpdatePasswordResponse>(data);
        print(json.success);
        print(json.msg);
    }

    public void ResetPasswordLinkServer()
    {
        _networkManager.SendResetLinkFromServer(mailToChangePassword.text, FailCallback, ResetPasswordLinkServerCallback);
    }

    void ResetPasswordLinkServerCallback(string data)
    {
        var json = JsonUtility.FromJson<UpdatePasswordResponse>(data);
        resetLinkServerResponse.text = json.msg;
    }

    public void UpdateCarSpecServer(string car_uuid, string spec_name, bool upgradable)
    {
        _networkManager.UpdateCarSpec(car_uuid, spec_name, upgradable, FailCallback, UpdateCarSpecServerCallback);
    }

    void UpdateCarSpecServerCallback(string data)
    {
        // Debug.Log(data);
    }



    //////////////////////////////////////MISSIONS///////////////////////////////////////////
    public void GetmissionGroupserver()
    {
        _networkManager.GetAllMissionFromServer(FailCallback, GetmissionGroupserverCallback);
    }

    void GetmissionGroupserverCallback(string data)
    {
        missionGroups = JsonUtility.FromJson<Root>(data);
        Debug.Log(missionGroups.users.Count);
        for (int i = 0; i < missionGroups.users.Count; i++)
        {
            GetMissionDetailServer(missionGroups.users[i].engine_id);
        }
    }

    public void GetMissionDetailServer(int fuelID)
    {
        print(currentFuelId.ToString());
        _networkManager.GetMissionDetailFromServer(fuelID.ToString(), FailCallback, GetMissionDetailServerCallback);
    }

    void GetMissionDetailServerCallback(string data)
    {
        var fuelType = JsonUtility.FromJson<Details>(data);
        DateTime endTime;
        DateTime currentTime;
        TimeSpan duration;

        for (int i = 0; i < fuelType.data.Count; i++)
        {
            if (fuelType.data[i].is_active == true)
            {

                switch (fuelType.data[i].engine_id)
                {
                    case 1:
                        endTime = System.DateTime.Parse(fuelType.data[i].end_time_date);
                        currentTime = System.DateTime.Parse(fuelType.start_time_date);

                        duration = endTime - currentTime;
                        print("duration : " + duration.TotalSeconds);
                        hydrogenRemainingTime = duration.TotalSeconds;
                        hydrogenRemainingTimeGO.transform.parent.gameObject.GetComponent<CountdownTimer>().countdownTime = hydrogenRemainingTime;
                        hydrogenRemainingTimeGO.transform.parent.gameObject.GetComponent<CountdownTimer>().countdownInternal = hydrogenRemainingTime;
                        

                        break;

                    case 2:
                        endTime = System.DateTime.Parse(fuelType.data[i].end_time_date);
                        currentTime = System.DateTime.Parse(fuelType.start_time_date);

                        duration = endTime - currentTime;
                        print("duration : " + duration.TotalSeconds);
                        solarRemainingTime = duration.TotalSeconds;
                        solarRemainingTimeGO.transform.parent.gameObject.GetComponent<CountdownTimer>().countdownTime = solarRemainingTime;
                        solarRemainingTimeGO.transform.parent.gameObject.GetComponent<CountdownTimer>().countdownInternal = solarRemainingTime;

                        break;

                    case 3:
                        endTime = System.DateTime.Parse(fuelType.data[i].end_time_date);
                        currentTime = System.DateTime.Parse(fuelType.start_time_date);

                        duration = endTime - currentTime;
                        print("duration : " + duration.TotalSeconds);
                        ionRemainingTime = duration.TotalSeconds;
                        ionRemainingTimeGO.transform.parent.gameObject.GetComponent<CountdownTimer>().countdownTime = ionRemainingTime;
                        ionRemainingTimeGO.transform.parent.gameObject.GetComponent<CountdownTimer>().countdownInternal = ionRemainingTime;
                        print("SESSSSSSS");

                        break;

                    case 4:
                        endTime = System.DateTime.Parse(fuelType.data[i].end_time_date);
                        currentTime = System.DateTime.Parse(fuelType.start_time_date);

                        duration = endTime - currentTime;
                        print("duration : " + duration.TotalSeconds);
                        nuclearRemainingTime = duration.TotalSeconds;
                        nuclearRemainingTimeGO.transform.parent.gameObject.GetComponent<CountdownTimer>().countdownTime = nuclearRemainingTime;
                        nuclearRemainingTimeGO.transform.parent.gameObject.GetComponent<CountdownTimer>().countdownInternal = nuclearRemainingTime;

                        break;
                }
            }
        }

        switch (fuelType.data[0].engine_id)
        {
            case 1:
                if (fuelType.isRewardEnabled)
                {
                    hydrogenGetReward.SetActive(true);
                    hydrogenNotification.SetActive(true);
                    hydrogenRemainingTimeGO.SetActive(false);
                    hydrogenStartMission.SetActive(false);
                }
                else if (fuelType.checkHasMission)
                {
                    hydrogenGetReward.SetActive(false);
                    hydrogenNotification.SetActive(false);
                    hydrogenStartMission.SetActive(false);
                    hydrogenRemainingTimeGO.SetActive(true);
                }
                else
                {
                    hydrogenGetReward.SetActive(false);
                    hydrogenNotification.SetActive(false);
                    hydrogenStartMission.SetActive(true);
                    hydrogenRemainingTimeGO.SetActive(false);
                }

                break;

            case 2:
                if (fuelType.isRewardEnabled)
                {
                    solarGetReward.SetActive(true);
                    solarNotification.SetActive(true);
                    solarStartMission.SetActive(false);
                    solarRemainingTimeGO.SetActive(false);
                }
                else if (fuelType.checkHasMission)
                {
                    solarGetReward.SetActive(false);
                    solarNotification.SetActive(false);
                    solarStartMission.SetActive(false);
                    solarRemainingTimeGO.SetActive(true);
                }
                else
                {
                    solarGetReward.SetActive(false);
                    solarNotification.SetActive(false);
                    solarStartMission.SetActive(true);
                    solarRemainingTimeGO.SetActive(false);
                }

                break;

            case 3:
                if (fuelType.isRewardEnabled)
                {
                    ionGetReward.SetActive(true);
                    ionNotification.SetActive(true);
                    ionStartMission.SetActive(false);
                    ionRemainingTimeGO.SetActive(false);
                }
                else if (fuelType.checkHasMission)
                {
                    ionGetReward.SetActive(false);
                    ionNotification.SetActive(false);
                    ionStartMission.SetActive(false);
                    ionRemainingTimeGO.SetActive(true);
                }
                else
                {
                    ionGetReward.SetActive(false);
                    ionNotification.SetActive(false);
                    ionStartMission.SetActive(true);
                    ionRemainingTimeGO.SetActive(false);
                }

                break;

            case 4:
                if (fuelType.isRewardEnabled)
                {
                    nuclearGetReward.SetActive(true);
                    nuclearNotification.SetActive(true);
                    nuclearStartMission.SetActive(false);
                    nuclearRemainingTimeGO.SetActive(false);
                }
                else if (fuelType.checkHasMission)
                {
                    nuclearGetReward.SetActive(false);
                    nuclearNotification.SetActive(false);
                    nuclearStartMission.SetActive(false);
                    nuclearRemainingTimeGO.SetActive(true);
                }
                else
                {
                    nuclearGetReward.SetActive(false);
                    nuclearNotification.SetActive(false);
                    nuclearStartMission.SetActive(true);
                    nuclearRemainingTimeGO.SetActive(false);
                }

                break;
        }
        ViewManager.Show<MissionView>();
        Debug.Log(data);
    }


    public void ChangeFuelID(int fuelID)
    {
        currentFuelId = fuelID;
    }

    public void ChangeMissionID(int missionID)
    {
        selectedMissionID = missionID;
        if (missionID <= 4 && missionID > 0)
        {
            currentFuelId = missionGroups.users[0].engine_id;
        }
        else if (missionID <= 8 && missionID > 4)
        {
            currentFuelId = missionGroups.users[1].engine_id;
        }
        else if (missionID <= 12 && missionID > 8)
        {
            currentFuelId = missionGroups.users[2].engine_id;
        }
        else if (missionID <= 16 && missionID > 12)
        {
            currentFuelId = missionGroups.users[3].engine_id;
        }
    }
    public void GotoMissionServer()
    {
        if (selectedMissionID != 0)
        {
            switch (currentFuelId)
            {
                case 1:
                    hydrogenRemainingTimeGO.SetActive(true);
                    hydrogenStartMission.SetActive(false);
                    break;
                case 2:
                    solarRemainingTimeGO.SetActive(true);
                    solarStartMission.SetActive(false);
                    break;
                case 3:
                    ionRemainingTimeGO.SetActive(true);
                    ionStartMission.SetActive(false);
                    break;
                case 4:
                    nuclearRemainingTimeGO.SetActive(true);
                    nuclearStartMission.SetActive(false);
                    break;

            }
            print(selectedMissionID.ToString());

            _networkManager.GotoMissionFromServer(selectedMissionID.ToString(), FailCallback, GotoMissionServerCallback);

        }
        else
        {
            FailCallbackError errorMessage = new FailCallbackError();
            errorMessage.msg = "Please Select Mission Time";
            string data = JsonUtility.ToJson(errorMessage);
            FailCallback(data);
        }
    }

    void GotoMissionServerCallback(string data)
    {
        Debug.Log(data);
        GetMissionDetailServer(currentFuelId);
    }

    public void AddRewardMissionServer()
    {


        switch (currentFuelId)
        {
            case 1:

                hydrogenGetReward.SetActive(false);
                hydrogenNotification.SetActive(false);
                hydrogenStartMission.SetActive(true);
                hydrogenRemainingTimeGO.SetActive(false);

                break;

            case 2:

                solarGetReward.SetActive(false);
                solarNotification.SetActive(false);
                solarStartMission.SetActive(true);
                solarRemainingTimeGO.SetActive(false);

                break;

            case 3:
                ionGetReward.SetActive(false);
                ionNotification.SetActive(false);
                ionStartMission.SetActive(true);
                ionRemainingTimeGO.SetActive(false);


                break;

            case 4:
                nuclearGetReward.SetActive(false);
                nuclearNotification.SetActive(false);
                nuclearStartMission.SetActive(true);
                nuclearRemainingTimeGO.SetActive(false);
                break;
        }

        _networkManager.AddRewardMissionFromServer(currentFuelId.ToString(), FailCallback, AddRewardMissionServerCallback);
    }

    void AddRewardMissionServerCallback(string data)
    {
        Debug.Log(data);
        GetCurrentlyFuelsServer();
    }

    public void GetCurrentlyFuelsServer()
    {
        _networkManager.GetUserFuelFromServer(FailCallback, GetCurrentlyFuelsServerCallback);
    }

    void GetCurrentlyFuelsServerCallback(string data)
    {
        var json = JsonUtility.FromJson<AllFuels>(data);
        Debug.Log(data);

        for (int i = 0; i < json.fuelDetail.Count; i++)
        {
            switch (json.fuelDetail[i].engine_id)
            {
                case 1:
                    hydrogenCount = json.fuelDetail[i].current_fuel;
                    //    hydrogenFuelCountText.text ="" + hydrogenCount;
                    break;

                case 2:
                    solarCount = json.fuelDetail[i].current_fuel;
                    //    solarFuelCountText.text ="" + solarCount;
                    break;

                case 3:
                    ionCount = json.fuelDetail[i].current_fuel;
                    //    ionFuelCountText.text ="" + ionCount;
                    break;

                case 4:
                    nuclearCount = json.fuelDetail[i].current_fuel;
                    //    nuclearFuelCountText.text ="" + nuclearCount;
                    break;
            }

        }

    }


    //////////////////////////////////Main Menu Dropdown Settings/////////////////////////////////////////////
    public void DropDownOnValueChanged(int index)
    {
        switch (index)
        {
            case 0:
                fuelText.text = "" + hydrogenCount;
                break;
            case 1:
                fuelText.text = "" + solarCount;
                break;
            case 2:
                fuelText.text = "" + ionCount;
                break;
            case 3:
                fuelText.text = "" + nuclearCount;
                break;
        }
    }
    ////////////////////////////////////////////UPGRADES///////////////////////////////////////////////////////////
    public void UpgradeEngineRequestServer(string carUuid)
    {
        _networkManager.UpgradeEngineRequestFromServer(carUuid, FailCallback, UpgradeEngineRequestServerCallback);
    }

    void UpgradeEngineRequestServerCallback(string data)
    {
        var json = JsonUtility.FromJson<UpgradeTurboResponse>(data);
        print(json.data.new_level);
        _uiManager.UpdateEngineUIVariables(json.data.new_level);
    }

    public void UpgradeDriveTrainRequestServer(string carUuid)
    {
        _networkManager.UpgradeDriveTrainRequestFromServer(carUuid, FailCallback, UpgradeDriveTrainRequestServerCallback);
    }

    void UpgradeDriveTrainRequestServerCallback(string data)
    {
        var json = JsonUtility.FromJson<UpgradeDrivetrainResponse>(data);
        print(json.data.new_level);
        _uiManager.UpdateDrivetrainUIVariables(json.data.new_level);
    }

    public void UpgradeTurboRequestServer(string carUuid)
    {
        _networkManager.UpgradeTurboRequestFromServer(carUuid, FailCallback, UpgradeTurboRequestServerCallback);
    }

    void UpgradeTurboRequestServerCallback(string data)
    {
        var json = JsonUtility.FromJson<UpgradeTurboResponse>(data);
        print(json.data.new_level);
        _uiManager.UpdateTurboUIVariables(json.data.new_level);
    }

    ///////////////////RESIZE POPUP ANIMATION/////////////////////
    public void FadeIn(GameObject comingUI)
    {
        selectedMissionID = 0;
        if (comingUI.transform.GetChild(0).transform.localScale == new Vector3(1, 1, 1) || isPopUpDone)
            return;
        StartCoroutine(_FadeInCoroutine(comingUI.transform.GetChild(0).transform, comingUI.GetComponent<Image>(), 0f, 1f));
    }

    private IEnumerator _FadeInCoroutine(Transform uiTransform, Image panelImage, float start, float end)
    {
        isPopUpDone = true;
        float elapsedTime = 0;
        float totalProgress = 0;
        while (elapsedTime < popUpDuration)
        {
            elapsedTime += Time.deltaTime;
            var progress = Time.deltaTime / popUpDuration;
            progress = Mathf.Min(progress, 1 - totalProgress);
            totalProgress += progress;
            var localScale = uiTransform.localScale;
            localScale = new Vector3(localScale.x + progress, localScale.y + progress, 1);
            uiTransform.localScale = localScale;
            //////for the panel/////
            float alpha = Mathf.Lerp(start, end, elapsedTime / popUpDuration);
            Color color = panelImage.color;
            color.a = alpha;
            panelImage.color = color;
            yield return null;
        }

        isPopUpDone = false;
        uiTransform.localScale = Vector3.one;
    }

    public void FadeOut(GameObject comingUI)
    {
        selectedMissionID = 0;
        if (comingUI.transform.GetChild(0).transform.localScale == new Vector3(0, 0, 1) || isPopUpDone)
            return;
        StartCoroutine(_FadeOutCoroutine(comingUI.transform.GetChild(0).transform, comingUI.GetComponent<Image>(), 1f, 0f));
    }

    private IEnumerator _FadeOutCoroutine(Transform uiTransform, Image panelImage, float start, float end)
    {
        isPopUpDone = true;
        float elapsedTime = 0;
        float totalProgress = 0;
        while (elapsedTime < popUpDuration)
        {
            elapsedTime += Time.deltaTime;
            var progress = Time.deltaTime / popUpDuration;
            progress = Mathf.Min(progress, 1 - totalProgress);
            totalProgress += progress;
            var localScale = uiTransform.localScale;
            localScale = new Vector3(localScale.x - progress, localScale.y - progress, 1);
            uiTransform.localScale = localScale;
            //////for the panel/////
            float alpha = Mathf.Lerp(start, end, elapsedTime / popUpDuration);
            Color color = panelImage.color;
            color.a = alpha;
            panelImage.color = color;
            yield return null;
        }
        isPopUpDone = false;
        uiTransform.localScale = Vector3.forward;
    }
}


[Serializable]
public class CarAttributesInGame
{
    public string name;
    public string description;
    public string uuid;
    public string image;
    public string dna;
    public int edition;
    public long date;
    public List<AttributeInGame> attributes;
    public string compiler;
}

[Serializable]
public class AttributeInGame
{
    public string trait_type;
    public string value;
}


[Serializable]
public class CarSpectIngame
{
    public List<CarAttributesInGame> nfts;
}
///////////////////////////////////////////////////////


[Serializable]
public class CarSpecs
{
    public string trait_type;
    public string value;
    public int level;
    public int xp;
}


[Serializable]
public class AllNFTDetailsServer
{
    public int id;
    public int user_id;
    public int car_id;
    public string uuid;
    public int category_id;
    public string category_name;
    public string name;
    public string description;
    public string dna;
    public List<CarSpecs> attributes;
    public string edition;
    public string date;
    public object compiler;
    public string image;
    public object extended_url;
    public int rank_points;
    public int axp_points;
    public int engine_level;
    public int drive_train_level;
    public int turbo_level;
    public DateTime createdAt;
    public DateTime updatedAt;
}



[Serializable]
public class AllNFTServerResponse
{
    public bool success;
    public List<AllNFTDetailsServer> data;
}
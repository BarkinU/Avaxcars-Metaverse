using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ColorDictionary
{
    public string ledColorName;
    public Color ColorRGB;
}
public class UIManager : MonoBehaviour
{

    [Header("                                   ClassReferences")]
    private static UIManager instance = null;
    private GameManager _gameManager;

    [Header("                                   Resources Variables")]

    public Dictionary<string, Texture2D> ResourcesColor = new Dictionary<string, Texture2D>();
    public Dictionary<string, Color> ResourcesLedColor = new Dictionary<string, Color>();
    public Dictionary<string, Texture2D> ResourcesCarRarityTextures;

    [Header("                                   UI Variables")]
    public TextMeshProUGUI carGEngineText, carGWeightText, carGDrivetrainText, carGTurboText, carGWheelTypeText, carGFuelTypeText, carGRarityText, carGCategoryName, carURankPoints, carUAxpPoints, carURarityText, carUEngineLevelText, carUDrivetrainLevelText, carUTurboLevelText, carGNameText, carUEngineText, carUWeightText, carUDrivetrainText, carUTurboText, carUWheelTypeText, carUFuelTypeText;
    public int currentEngineLevel, currentDrivetrainLevel, currentTurboLevel;
    public Image[] engineLevelBar, drivetrainLevelBar, turboLevelBar;
    public Button nextButton, previousButton, upgradeNextButton, upgradePreviousButton, backButton;



    [Header("                                   Menu Car Variables")]
    public List<GameObject> Cars;
    public List<GameObject> CarsInstantiates;
    public int currentIndex;
    public GameObject baseModel;
    private Texture2D tempColor, rarityTexture;
    public ColorDictionary[] tempLedColor;

    [Header("                                   Loading Progress Bar")]
    public Image progressBar;
    public float minimumValue, currentValue, maximumValue;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("UIManager").AddComponent<UIManager>();
            }

            return instance;
        }
    }

    private void OnEnable()
    {
        instance = this;

    }

    public void TransferAllNFTsToGame()
    {
        _gameManager = GameManager.Instance;
        DontDestroyOnLoad(this.gameObject);
        InitializeCarProperties();
        AddButtonListeners();

    }

    private void InitializeCarProperties()
    {
        currentIndex = 0;

        for (int i = 0; i < _gameManager.allCar.data.Count; i++)
        {
            Cars.Add(baseModel);
            Cars[i].GetComponent<CarBase>().InitializeCarProperties(_gameManager.allCar.data[i]);
            SpawnMenuCar(i);
        }
    }

    private void SpawnMenuCar(int number)
    {
        CarsInstantiates.Add(Instantiate(Cars[number], Vector3.zero, Quaternion.Euler(0f, 145f, 0f)));
        CarsInstantiates[number].SetActive(false);
    }

    private void AddButtonListeners()
    {
        nextButton.onClick.AddListener(() => NextCarButton());
        previousButton.onClick.AddListener(() => PreviousCarButton());
        upgradeNextButton.onClick.AddListener(() => UpgradeNextCarButton());
        upgradePreviousButton.onClick.AddListener(() => UpgradePreviousCarButton());


    }

    private void RefreshCarProperties()
    {
        ////Car Materials
        var myMaterial = new List<Material>();
        CarsInstantiates[currentIndex].GetComponentInChildren<MeshRenderer>().GetMaterials(myMaterial);

        /////GarageView
        carGEngineText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carEngine;
        carGWeightText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carWeight;
        carGDrivetrainText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carDriveTrain;
        carGTurboText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carTurbo;
        carGWheelTypeText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carWheelType;
        carGFuelTypeText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carEngine;
        carGRarityText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carRarity;
        carURarityText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carRarity;
        carGNameText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carName;
        carGCategoryName.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carCategoryName;
        carURankPoints.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().rankPoints;
        carUAxpPoints.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().axpPoints;
        carUEngineLevelText.text = "Level " + CarsInstantiates[currentIndex].GetComponent<CarBase>().engineLevel;
        carUDrivetrainLevelText.text = "Level " + CarsInstantiates[currentIndex].GetComponent<CarBase>().drivetrainLevel;
        carUTurboLevelText.text = "Level " + CarsInstantiates[currentIndex].GetComponent<CarBase>().turboLevel;
        currentEngineLevel = CarsInstantiates[currentIndex].GetComponent<CarBase>().engineLevel;
        currentDrivetrainLevel = CarsInstantiates[currentIndex].GetComponent<CarBase>().drivetrainLevel;
        currentTurboLevel = CarsInstantiates[currentIndex].GetComponent<CarBase>().turboLevel;


        //////UpgradeView
        carUEngineText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carEngine;
        carUWeightText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carWeight;
        carUDrivetrainText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carDriveTrain;
        carUTurboText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carTurbo;
        carUWheelTypeText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carWheelType;
        carUFuelTypeText.text = "" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carEngine;


        ///////////SET CAR LED COLOR/////////////
        for (int i = 0; i < tempLedColor.Length; i++)
        {
            print(tempLedColor[i].ledColorName);
            if (CarsInstantiates[currentIndex].GetComponent<CarBase>().carLedColor == tempLedColor[i].ledColorName)
            {
                if (CarsInstantiates[currentIndex].GetComponent<CarBase>().carRarity != "Common")
                {
                    myMaterial[1].EnableKeyword("_EMISSION");
                    myMaterial[0].DisableKeyword("_EMISSION");
                    myMaterial[1].SetColor("_EmissionColor", tempLedColor[i].ColorRGB);
                }
                else
                {
                    myMaterial[0].EnableKeyword("_EMISSION");
                    myMaterial[1].DisableKeyword("_EMISSION");
                    myMaterial[0].SetColor("_EmissionColor", tempLedColor[i].ColorRGB);
                    
                }
                break;
            }
        }

        ///////SET CAR TEXTURES AND EMISSIONS/////////////
        tempColor = Resources.Load("CaseColor/" + CarsInstantiates[currentIndex].GetComponent<CarBase>().carColor) as Texture2D;   ////Car Color Texture
        myMaterial[1].SetTexture("_BaseMap", tempColor);
        switch (CarsInstantiates[currentIndex].GetComponent<CarBase>().carRarity)
        {

            case "Common":
                rarityTexture = Resources.Load("Emissions/Common_Emission") as Texture2D;
                CarsInstantiates[currentIndex].GetComponentInChildren<MeshRenderer>().GetMaterials(myMaterial);
                myMaterial[0].SetTexture("_EmissionMap", rarityTexture);
                myMaterial[1].DisableKeyword("_EMISSION");
                break;
            case "UnCommon":
                rarityTexture = Resources.Load("Emissions/Uncommon_Emission") as Texture2D;
                CarsInstantiates[currentIndex].GetComponentInChildren<MeshRenderer>().GetMaterials(myMaterial);
                myMaterial[1].SetTexture("_EmissionMap", rarityTexture);
                myMaterial[0].SetTexture("_BaseMap", tempColor);
                myMaterial[0].DisableKeyword("_EMISSION");
                break;
            case "Rare":
                rarityTexture = Resources.Load("Emissions/Rare_Emission") as Texture2D;
                CarsInstantiates[currentIndex].GetComponentInChildren<MeshRenderer>().GetMaterials(myMaterial);
                myMaterial[1].SetTexture("_EmissionMap", rarityTexture);
                myMaterial[0].SetTexture("_BaseMap", tempColor);
                myMaterial[0].DisableKeyword("_EMISSION");
                break;
            case "Epic": 
                rarityTexture = Resources.Load("Emissions/Epic_Emission") as Texture2D;
                CarsInstantiates[currentIndex].GetComponentInChildren<MeshRenderer>().GetMaterials(myMaterial);
                myMaterial[1].SetTexture("_EmissionMap", rarityTexture);
                myMaterial[0].SetTexture("_BaseMap", tempColor);
                myMaterial[0].DisableKeyword("_EMISSION");

                break;
        }
    }


    public void GarageOpening()
    {
        currentIndex = 0;
        RefreshCarProperties();

        _gameManager.camTransition.ChangeCameraPositionByIndex(2);
    }
    public void UpgradeOpening()
    {
        currentIndex = 0;
        RefreshCarProperties();
        UpdateLevelBar();

        _gameManager.camTransition.ChangeCameraPositionByIndex(2);
    }

    public void GameOpening()
    {
        RefreshCarProperties();
        currentIndex = 0;
        CarsInstantiates[currentIndex].SetActive(true);
        CarsInstantiates[currentIndex].transform.position = Vector3.zero;
        CarsInstantiates[currentIndex].transform.rotation = Quaternion.Euler(0f, 145f, 0f);

    }

    public void GarageClosing()
    {
        if (currentIndex != 0)
        {
            CarsInstantiates[currentIndex].SetActive(false);
            CarsInstantiates[0].SetActive(true);
        }
    }

    private void NextCarButton()
    {
        if (currentIndex < Cars.Count - 1)
        {
            CarsInstantiates[currentIndex].SetActive(false);
            currentIndex++;
            CarsInstantiates[currentIndex].transform.position = Vector3.zero;
            CarsInstantiates[currentIndex].transform.rotation = Quaternion.Euler(0f, 145f, 0f);
            CarsInstantiates[currentIndex].SetActive(true);
        }
        RefreshCarProperties();
    }

    private void PreviousCarButton()
    {
        if (currentIndex > 0)
        {
            CarsInstantiates[currentIndex].SetActive(false);
            currentIndex--;
            CarsInstantiates[currentIndex].transform.position = Vector3.zero;
            CarsInstantiates[currentIndex].transform.rotation = Quaternion.Euler(0f, 145f, 0f);
            CarsInstantiates[currentIndex].SetActive(true);
        }
        RefreshCarProperties();
    }

    private void UpgradeNextCarButton()
    {
        if (currentIndex < Cars.Count - 1)
        {
            CarsInstantiates[currentIndex].SetActive(false);
            currentIndex++;
            CarsInstantiates[currentIndex].transform.position = Vector3.zero;
            CarsInstantiates[currentIndex].transform.rotation = Quaternion.Euler(0f, 145f, 0f);
            CarsInstantiates[currentIndex].SetActive(true);

        }
        RefreshCarProperties();
        UpdateLevelBar();
    }

    private void UpgradePreviousCarButton()
    {
        if (currentIndex > 0)
        {
            CarsInstantiates[currentIndex].SetActive(false);
            currentIndex--;
            CarsInstantiates[currentIndex].transform.position = Vector3.zero;
            CarsInstantiates[currentIndex].transform.rotation = Quaternion.Euler(0f, 145f, 0f);
            CarsInstantiates[currentIndex].SetActive(true);

        }
        RefreshCarProperties();
        UpdateLevelBar();
    }

    public void UpdateLevelBar()
    {
        for (int i = 0; i < engineLevelBar.Length; i++)
        {
            engineLevelBar[i].enabled = !DisplayLevelLevelAndRarity(currentEngineLevel, i);
            drivetrainLevelBar[i].enabled = !DisplayLevelLevelAndRarity(currentDrivetrainLevel, i);
            turboLevelBar[i].enabled = !DisplayLevelLevelAndRarity(currentTurboLevel, i);

        }
    }


    private bool DisplayLevelLevelAndRarity(float _currentLevel, int levelNumber)
    {
        return (levelNumber >= _currentLevel);
    }


    public void UpgradeEngineSpec()
    {
        _gameManager.UpgradeEngineRequestServer(CarsInstantiates[currentIndex].GetComponent<CarBase>().carUuid);
    }

    public void UpgradeDriveTrainSpec()
    {
        _gameManager.UpgradeDriveTrainRequestServer(CarsInstantiates[currentIndex].GetComponent<CarBase>().carUuid);
    }

    public void UpgradeTurboSpec()
    {
        _gameManager.UpgradeTurboRequestServer(CarsInstantiates[currentIndex].GetComponent<CarBase>().carUuid);
    }

    public void UpdateEngineUIVariables(int newLevel)
    {
        currentEngineLevel = newLevel;
        carUEngineLevelText.text = "" + currentTurboLevel;
        UpdateLevelBar();
    }

    public void UpdateDrivetrainUIVariables(int newLevel)
    {
        currentDrivetrainLevel = newLevel;
        carUDrivetrainLevelText.text = "" + currentDrivetrainLevel;
        UpdateLevelBar();
    }

    public void UpdateTurboUIVariables(int newLevel)
    {
        currentTurboLevel = newLevel;
        carUTurboLevelText.text = "" + currentTurboLevel;
        UpdateLevelBar();
    }


}





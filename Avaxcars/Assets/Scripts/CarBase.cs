using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBase : MonoBehaviour
{
    public string carName, carDescription, carUuid, carImage, carDna, carColor, carRarity, carLedColor, carEngine, carWeight, carDriveTrain, carTurbo, carWheelType, compiler, carEdition, carDate, carCategoryName, ownerUsername, joinedRoomName;
    public int categoryID, rankPoints, axpPoints, engineLevel, drivetrainLevel, turboLevel, car_id;
    public int engineRarity, drivetrainRarity, turboRarity;
    private Texture2D tempColor, rarityTexture;
    public ColorDictionary[] tempLedColor;
    public CarRaceInfo racerResult;

    public void InitializeCarProperties(AllNFTDetailsServer carSpect)
    {

        ///Car Spects
        this.carName = carSpect.name;
        this.carDescription = carSpect.description;
        this.carUuid = carSpect.uuid;
        this.carImage = carSpect.image;
        this.carDna = carSpect.dna;
        this.carEdition = carSpect.edition;
        this.carDate = carSpect.date;
        this.carCategoryName = carSpect.category_name;
        this.categoryID = carSpect.category_id;
        this.rankPoints = carSpect.rank_points;
        this.axpPoints = carSpect.axp_points;
        this.engineLevel = carSpect.engine_level;
        this.drivetrainLevel = carSpect.drive_train_level;
        this.turboLevel = carSpect.turbo_level;
        this.car_id = carSpect.car_id;
        this.carColor = carSpect.attributes[0].value.ToString();
        this.carRarity = carSpect.attributes[1].value.ToString();
        this.carLedColor = carSpect.attributes[2].value.ToString();
        this.carEngine = carSpect.attributes[3].value.ToString();
        this.carWeight = carSpect.attributes[4].value.ToString();
        this.carDriveTrain = carSpect.attributes[5].value.ToString();
        this.carTurbo = carSpect.attributes[6].value.ToString();
        this.carWheelType = carSpect.attributes[7].value.ToString();

        ////Car Engine Rarity
        switch (carEngine)
        {

            case "Hidrojen":
                engineRarity = 1;
                break;
            case "Solar":
                engineRarity = 2;
                break;
            case "Lityum":
                engineRarity = 3;
                break;
            case "Nuclear":
                engineRarity = 4;
                break;
        }
        ////Car Engine Rarity
        switch (carDriveTrain)
        {

            case "DT1":
                drivetrainRarity = 1;
                break;
            case "DT2":
                drivetrainRarity = 2;
                break;
            case "DT3":
                drivetrainRarity = 3;
                break;
            case "DT4":
                drivetrainRarity = 4;
                break;
        }

        ////Car Engine Rarity
        switch (carTurbo)
        {

            case "TBO1":
                turboRarity = 1;
                break;
            case "TBO2":
                turboRarity = 2;
                break;
            case "TBO3":
                turboRarity = 3;
                break;
            case "TBO4":
                turboRarity = 4;
                break;
        }


    }

    public void ChangeAllCarsToNFT()
    {
        ////Car Materials
        var myMaterial = new List<Material>();
        gameObject.GetComponentInChildren<MeshRenderer>().GetMaterials(myMaterial);


        ///////////SET CAR LED COLOR/////////////
        for (int i = 0; i < tempLedColor.Length; i++)
        {
            print(tempLedColor[i].ledColorName);
            if (carLedColor == tempLedColor[i].ledColorName)
            {
                if (carRarity != "Common")
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
        tempColor = Resources.Load("CaseColor/" + carColor) as Texture2D;   ////Car Color Texture
        myMaterial[1].SetTexture("_BaseMap", tempColor);
        switch (carRarity)
        {

            case "Common":
                rarityTexture = Resources.Load("Emissions/Common_Emission") as Texture2D;
                gameObject.GetComponentInChildren<MeshRenderer>().GetMaterials(myMaterial);
                myMaterial[0].SetTexture("_EmissionMap", rarityTexture);
                myMaterial[1].DisableKeyword("_EMISSION");
                break;
            case "UnCommon":
                rarityTexture = Resources.Load("Emissions/Uncommon_Emission") as Texture2D;
                gameObject.GetComponentInChildren<MeshRenderer>().GetMaterials(myMaterial);
                myMaterial[1].SetTexture("_EmissionMap", rarityTexture);
                myMaterial[0].SetTexture("_BaseMap", tempColor);
                myMaterial[0].DisableKeyword("_EMISSION");
                break;
            case "Rare":
                rarityTexture = Resources.Load("Emissions/Rare_Emission") as Texture2D;
                gameObject.GetComponentInChildren<MeshRenderer>().GetMaterials(myMaterial);
                myMaterial[1].SetTexture("_EmissionMap", rarityTexture);
                myMaterial[0].SetTexture("_BaseMap", tempColor);
                myMaterial[0].DisableKeyword("_EMISSION");
                break;
            case "Epic":
                rarityTexture = Resources.Load("Emissions/Epic_Emission") as Texture2D;
                gameObject.GetComponentInChildren<MeshRenderer>().GetMaterials(myMaterial);
                myMaterial[1].SetTexture("_EmissionMap", rarityTexture);
                myMaterial[0].SetTexture("_BaseMap", tempColor);
                myMaterial[0].DisableKeyword("_EMISSION");

                break;
        }
    }

}

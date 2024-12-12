using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CarScript : MonoBehaviour
{

    #region In Game
     public WheelCollider[] colliders;
    private Rigidbody RB;
    [SerializeField] private CarBase carBase;
    public CameraFollow myCamera;
    #endregion

    #region NFT Car Properties 

     public string engineType;
     public string driveTrain;
     public string turbo;
     public string wheelType;
     public string weight;
    #endregion

    #region Others
    public float distance;
    public GameObject carSign;
    public GameObject[] wheelModels;
    public float totalDistance;
    private Color myEmissionColor;
    private Color myBodyColor;
    public List<float> racerValues;
    public List<Material> myMat = new List<Material>();
     public float wheelRotn = 0;
    private bool emissionCheck = false;
    public double finishTime;

    #endregion


    private void Start()
    {


        colliders = GetComponentsInChildren<WheelCollider>();
        RB = GetComponent<Rigidbody>();
        myCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        carBase = GetComponent<CarBase>();
    }

    public void GenerateCarSpec()
    {
        totalDistance = RaceManager.instance.currentRaceDistance;
        carSign.GetComponent<Button>().onClick.AddListener(ListTargetSelector);
        this.transform.GetComponentInChildren<MeshRenderer>().GetMaterials(myMat);
        myEmissionColor = myMat[1].GetColor("_EmissionColor");
        myBodyColor = myMat[1].GetColor("_BaseColor");
        carSign.transform.GetChild(0).GetComponent<Image>().color = myBodyColor;
        carSign.transform.GetChild(2).GetComponent<Image>().color = myBodyColor;
    }
    public FinishHandler finishObject;
    private void Update()
    {

        distance = Vector2.Distance(Vector2.zero, transform.position);

        if (RaceManager.instance.preRaceProcess && emissionCheck == false)
        {

            myMat[1].SetColor("_EmissionColor", myEmissionColor);
            emissionCheck = true;

        }
        if (arrow != null)
            arrow.transform.LookAt(Camera.main.transform.position);

    }
    public GameObject arrow;

    void ListTargetSelector()
    {

        myCamera.carProperties.SetActive(true);
        myCamera.targetID.text = this.gameObject.transform.name;
        myCamera.targetEngine.text = "" + GetComponent<CarBase>().carEngine.ToString();
        myCamera.targetDT.text = "" + GetComponent<CarBase>().carDriveTrain.ToString();
        myCamera.targetTurbo.text = "" + GetComponent<CarBase>().carTurbo.ToString();
        myCamera.targetWeight.text = "" + GetComponent<CarBase>().carWeight.ToString();
        myCamera.targetWheel.text = "" + GetComponent<CarBase>().carWheelType.ToString();

        if (RaceManager.instance.rmArrow != null)
            RaceManager.instance.rmArrow.SetActive(false);

        RaceManager.instance.rmArrow = arrow;
        arrow.SetActive(true);

    }

    public void PingPongLed()
    {

        myMat[1].SetColor("_EmissionColor", Color.Lerp(myEmissionColor, Color.black, Mathf.PingPong(Time.time, .85f)));

    }

    void FixedUpdate()
    {
        if (RaceManager.instance.winnerPanel.activeSelf == false)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                wheelRotn += this.gameObject.GetComponent<Rigidbody>().velocity.x * 5 * Time.deltaTime;
                wheelModels[i].transform.rotation = (transform.rotation) * Quaternion.Euler(wheelRotn, colliders[i].steerAngle, transform.rotation.z);

            }
        }
        if (RaceManager.instance.winnerPanel.activeSelf == true && RaceManager.instance.finishAnimTimer < 3)
        {
            transform.GetChild(3).gameObject.SetActive(false);
            for (int i = 0; i < colliders.Length; i++)
            {

                wheelRotn += 100 * Time.deltaTime;
                wheelModels[i].transform.rotation = (transform.rotation) * Quaternion.Euler(wheelRotn, 30, transform.rotation.z);

            }
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    [Header("                               @@@ In Game Adjustments @@@")]
    [SerializeField] private RaceManager raceManager;

    [Header("                               @@@ Camera @@@")]
    public Transform target; // camera target
    private int cameraIndex = 0; // camera's view index
    private Quaternion currentRotn = Quaternion.identity;
    private Quaternion desiredRotn = Quaternion.identity;
    private int ellipticalDirection = 1;
    private int lastEllipticalDirection = 1;
    public float rotnDamping = 5f;
    private Vector3 targetPosn, lastFollowerPosn = Vector3.zero;
    public float xPosition = 20;
    public float yPosition = 5;
    public float pitchAngle = 7f;
    private Vector3 lastTargetPositn = Vector3.zero;
    public float ellipticX = 0f;
    public float ellipticY = 0f;
    public float minEllipticY = -5f;
    public float maxEllipticY = 5f;
    private float ellipticXSpeed = 7.5f;
    private float ellipticYSpeed = 5f;
    [Header("                               @@@ Game Variables @@@")]


    #region Manuel&AutoCamera
    public Ray ray;
    public RaycastHit hit;
    public int layer_mask;
    public Toggle autoCameraToggle;
    public TextMeshProUGUI distanceText;

    #endregion

    void Start()
    {

        cameraIndex = 4;
        layer_mask = LayerMask.GetMask("Player");
        target = raceManager.racers[0].transform;

    }
    void Update()
    {

        if (raceManager.phaseSelect == RaceManager.phases.first)
        {
            if (raceManager.startPresentationTimer < 3.5f)
            {
                raceManager.startLogo.GetComponent<Image>().color += new Color(0, 0, 0, 1f * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, new Vector3(-3000, 44, 80), Time.deltaTime);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(25, 180, 0), Time.deltaTime);

            }
            else
            {
                raceManager.startLogo.GetComponent<Image>().color -= new Color(0, 0, 0, 2* Time.deltaTime);

                transform.position += new Vector3(Time.deltaTime, 0, 0);
                transform.eulerAngles += new Vector3(-Time.deltaTime, Time.deltaTime * 2, 0);

            }

        }

        if (raceManager.preRaceProcess && (raceManager.racers[9].transform.position.x < raceManager.currentRaceDistance))
        {

            SwitchTarget();
            TargetSelector();
            Camera();
            Elliptic();

        }
        else if ((raceManager.racers[9].transform.position.x > raceManager.currentRaceDistance))
        {
            if (raceManager.finishTimer < 6)
            {
                this.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.GetComponent<Camera>().fieldOfView, 80, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 40, transform.position.z), Time.deltaTime);
            }

            else if (raceManager.finishTimer > 6 && raceManager.finishTimer < 9)
            {

                transform.position = new Vector3(raceManager.firstPlaceGO.transform.position.x + 6f, 2.5f, Mathf.Lerp(transform.position.z, raceManager.firstPlaceGO.transform.position.z - 4f, 5 * Time.deltaTime));
                transform.eulerAngles = new Vector3(20, -80, 0);

            }

        }

    }
    void LateUpdate()
    {

        distanceText.text = Mathf.RoundToInt(target.transform.position.x).ToString() + " / " + ((int)raceManager.currentRaceDistance);

    }

    void Camera()
    {
        lastEllipticalDirection = 1;

        desiredRotn = target.transform.rotation * Quaternion.AngleAxis((ellipticalDirection == 1 ? -120 : 180) + (true ? ellipticX : 0), Vector3.up);
        desiredRotn = desiredRotn * Quaternion.AngleAxis((true ? ellipticY : 0), Vector3.right);

        // Y ekseninde rotation u sonumleme
        if (Time.time > 1)
            currentRotn = Quaternion.Lerp(currentRotn, desiredRotn, rotnDamping * Time.deltaTime);
        else
            currentRotn = desiredRotn;

        targetPosn = new Vector3(target.transform.position.x, target.transform.position.y, -17);
        targetPosn -= (currentRotn) * Vector3.forward * (xPosition * .90f);
        targetPosn += Vector3.up * (yPosition * .85f);

        transform.position = new Vector3(targetPosn.x, Mathf.Lerp(targetPosn.y, targetPosn.y - .2f, Time.deltaTime), targetPosn.z);

        transform.LookAt(new Vector3(target.transform.position.x, target.transform.position.y, -20));
        transform.eulerAngles = new Vector3(currentRotn.eulerAngles.x + (pitchAngle * Mathf.Lerp(1f, .75f, (target.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) / 200f)), transform.eulerAngles.y, 0);

        lastFollowerPosn = -transform.position;
        lastTargetPositn = -targetPosn;

    }

    // Automatically changes the camera's focus target
    private void SwitchTarget()
    {

        foreach (GameObject player in raceManager.racers)
        {

            if (player.transform.position.x > target.transform.position.x)
            {

                target = player.transform;

            }

        }

    }

    void Elliptic()
    {

        ellipticY = Mathf.Clamp(ellipticY, minEllipticY, maxEllipticY); //Clamping Y.

        if (autoCameraToggle.isOn)
        {

            if (target.transform.position.x > 5 && target.transform.position.x < (raceManager.currentRaceDistance / 8))
            {
                ellipticX = Mathf.Lerp(ellipticX, 30, Time.deltaTime);
                ellipticY = Mathf.Lerp(ellipticY, 5.6f, Time.deltaTime);
            }
            else if (target.transform.position.x > (raceManager.currentRaceDistance / 8) && target.transform.position.x < (raceManager.currentRaceDistance / 5))
            {
                ellipticX = Mathf.Lerp(ellipticX, 105, Time.deltaTime);
                ellipticY = Mathf.Lerp(ellipticY, 70, Time.deltaTime);

            }
            else if (target.transform.position.x > (raceManager.currentRaceDistance / 5) && target.transform.position.x < (raceManager.currentRaceDistance / 3))
            {

                ellipticX = Mathf.Lerp(ellipticX, -101, Time.deltaTime);
                ellipticY = Mathf.Lerp(ellipticY, 10, Time.deltaTime);

            }
            else if (target.transform.position.x > (raceManager.currentRaceDistance / 3) && target.transform.position.x < (raceManager.currentRaceDistance / 1.7f))
            {

                ellipticX = Mathf.Lerp(ellipticX, 105, Time.deltaTime);
                ellipticY = Mathf.Lerp(ellipticY, 70, Time.deltaTime);
            }
            else if (target.transform.position.x > (raceManager.currentRaceDistance / 1.7f) && target.transform.position.x < (raceManager.currentRaceDistance))
            {

                ellipticX = Mathf.Lerp(ellipticX, 30, Time.deltaTime);
                ellipticY = Mathf.Lerp(ellipticY, 5.6f, Time.deltaTime);

            }

        }

    }

    public void OnDrag(PointerEventData pointerData)
    {
        if (autoCameraToggle.isOn == false)
        {
            // Drag input which is received from UI.
            ellipticX += pointerData.delta.x * ellipticXSpeed * .02f;
            ellipticY -= pointerData.delta.y * ellipticYSpeed * .02f;
        }

    }

    #region TargetProperties
    public TextMeshProUGUI targetID;
    public TextMeshProUGUI targetEngine;
    public TextMeshProUGUI targetDT;
    public TextMeshProUGUI targetTurbo;
    public TextMeshProUGUI targetWeight;
    public TextMeshProUGUI targetWheel;
    public GameObject carProperties;

    #endregion
    private GameObject arrow;
    public void TargetSelector()
    {

        if (Input.GetMouseButtonDown(0))
        {

            ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000, layer_mask))
            {
                if (raceManager.rmArrow != null)
                    raceManager.rmArrow.SetActive(false);
                if (arrow != null)
                    arrow.SetActive(false);

                arrow = hit.transform.Find("Arrow").gameObject;
                arrow.gameObject.SetActive(true);
                carProperties.SetActive(true);
                targetID.text = hit.transform.name;
                targetEngine.text = "" + hit.transform.GetComponent<CarScript>().engineType.ToString();
                targetDT.text = "" + hit.transform.GetComponent<CarScript>().driveTrain.ToString();
                targetTurbo.text = "" + hit.transform.GetComponent<CarScript>().turbo.ToString();
                targetWeight.text = "" + hit.transform.GetComponent<CarScript>().weight.ToString();
                targetWheel.text = "" + hit.transform.GetComponent<CarScript>().wheelType.ToString();

            }
            else
            {
                carProperties.SetActive(false);
                if (arrow != null)
                {
                    arrow.gameObject.SetActive(false);
                }
                if (raceManager.rmArrow != null)
                {
                    raceManager.rmArrow.SetActive(false);
                }

            }

        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishHandler : MonoBehaviour
{

    public bool isFinished = false;

    public int finishIndex = 0;
    public CameraFollow cameraFollow;
    public SwitchToggle switcher;
    private int number = 1;
    public TextMeshProUGUI[] signTimers;
    public GameObject[] timerBG;
    public RaceManager raceManager;

    void Start()
    {
        raceManager = RaceManager.instance;

    }
    void FinishCheck()
    {

        timerBG[finishIndex].SetActive(true);

        raceManager.topPlacementsTime[finishIndex].text = "" + raceManager.raceTotalTime;

        signTimers[finishIndex].text = "" + raceManager.raceTotalTime;

        raceManager.finishNames[finishIndex].text = raceManager.racers[9 - finishIndex].name;

        if (number == 1)
        {

            switcher.switchBtn.transform.localPosition = new Vector3(-23.9f, 0, 0);
            number++;
            cameraFollow.autoCameraToggle.isOn = true;
            cameraFollow.autoCameraToggle.interactable = false;

        }

        finishIndex += 1;

        if (finishIndex >= 9)
        {

            isFinished = true;

        }

    }

    void Update()
    {

        if (finishIndex <= 9 && raceManager.racers[9 - finishIndex].transform.position.x - raceManager.currentRaceDistance >= 0)
            FinishCheck();

    }


}

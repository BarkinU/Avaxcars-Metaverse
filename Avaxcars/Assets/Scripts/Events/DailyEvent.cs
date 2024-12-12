using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DailyEvent : MonoBehaviour {
    public Button timerButton;
    public TextMeshProUGUI timeLabel;
    public string StartTime;
    public string EndTime;
    private double tcounter;
    private TimeSpan eventStartTime;
    private TimeSpan eventEndTime;
    private TimeSpan currentTime;
    private TimeSpan _remainingTime;
    private string Timeformat;
    private bool timerSet;
    private bool countIsReady;
    private bool countIsReady2;

    /*void Start () {
        eventStartTime = TimeSpan.Parse (StartTime);
        eventEndTime = TimeSpan.Parse (EndTime);
        StartCoroutine ("CheckTime");
    }

    private IEnumerator CheckTime () {
        Debug.Log ("==> Checking the time");
        timeLabel.text = "Checking the time";
        yield return StartCoroutine (
            TimeManager.sharedInstance.getTime ()
        );
        updateTime ();
        Debug.Log ("==> Time check complete!");

    }

    private void updateTime () {
        currentTime = TimeSpan.Parse (TimeManager.sharedInstance.getCurrentTimeNow ());
        timerSet = true;
    }

    void Update () {
        if (timerSet) {
            if (currentTime >= eventStartTime && currentTime <= eventEndTime) { //this means the event as already started and players can click and join
                _remainingTime = eventEndTime.Subtract (currentTime);
                tcounter = _remainingTime.TotalMilliseconds;
                countIsReady2 = true;

            } else if (currentTime < eventStartTime) { //this means the event had not started yet for today
                _remainingTime = eventStartTime.Subtract (currentTime);
                tcounter = _remainingTime.TotalMilliseconds;
                countIsReady = true;
            } else { //the event as already passed
                disableButton ("Event is over, comeback tomorrow");
            }
        }

        if (countIsReady) { startCountdown (); }
        if (countIsReady2) { startCountdown2 (); }
    }

    //setup the time format string
    public string GetRemainingTime (double x) {
        TimeSpan tempB = TimeSpan.FromMilliseconds (x);
        Timeformat = string.Format ("{0:D2}:{1:D2}:{2:D2}", tempB.Hours, tempB.Minutes, tempB.Seconds);
        return Timeformat;
    }

    private void startCountdown () {
        timerSet = false;
        tcounter -= Time.deltaTime * 1000;
        disableButton ("Starting Soon : " + GetRemainingTime (tcounter));

        if (tcounter <= 0) {
            countIsReady = false;
            validateTime ();
        }
    }

    private void startCountdown2 () {
        timerSet = false;
        tcounter -= Time.deltaTime * 1000;
        enableButton ("Event Started! Time Remaining : " + GetRemainingTime (tcounter));

        if (tcounter <= 0) {
            countIsReady2 = false;
            validateTime ();
        }
    }

    //enable button function
    private void enableButton (string x) {
        timerButton.interactable = true;
        timeLabel.text = x;
    }

    //disable button function
    private void disableButton (string x) {
        timerButton.interactable = false;
        timeLabel.text = x;
    }

    //validator
    private void validateTime () {
        Debug.Log ("==> Validating time to make sure no speed hack!");
        StartCoroutine ("CheckTime");
    }

    public void StartEventScene () {
        SceneManager.LoadScene ("SampleScene");
    }

*/}
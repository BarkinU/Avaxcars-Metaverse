/*using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchFuel : MonoBehaviour {
    public float msToWait = 5000.0f;
    private TextMeshProUGUI remaningTime;
    private Button fuelButton;
    private ulong lastFuelSearch;
    private int firstClick;
    public List<float> hydrogenChanceList;
    public List<float> solarChanceList;
    public List<float> ionChanceList;
    public List<float> nuclearChanceList;

    private void Start () {
        fuelButton = GetComponent<Button> ();
        remaningTime = GetComponentInChildren<TextMeshProUGUI> ();
        lastFuelSearch = ulong.Parse (PlayerPrefs.GetString ("LastFuelSearch"));
        firstClick = PlayerPrefs.GetInt ("FuelMissionClick");

        if (!isFuelReady ()) {
            fuelButton.interactable = false;
        }
    }
    public void SearchFuelClick () {
        if (firstClick == 0) {
            lastFuelSearch = (ulong) DateTime.Now.Ticks;
            PlayerPrefs.SetString ("LastFuelSearch", DateTime.Now.Ticks.ToString ());
            fuelButton.interactable = false;
            PlayerPrefs.SetInt ("FuelMissionClick", 1);
            firstClick = PlayerPrefs.GetInt ("FuelMissionClick");
        } else {
            GetReward ();
            PlayerPrefs.SetInt ("FuelMissionClick", 0);
            firstClick = PlayerPrefs.GetInt ("FuelMissionClick");
            remaningTime.text = "Search Fuel!";
            
        }

    }

    private void Update () {
        if (!fuelButton.IsInteractable ()) {
            if (isFuelReady ()) {
                fuelButton.interactable = true;
                return;
            }

            //SetTimer
            ulong diff = ((ulong) DateTime.Now.Ticks - lastFuelSearch);
            ulong m = diff / TimeSpan.TicksPerMillisecond;
            float secondsLeft = (float) (msToWait - m) / 1000.0f;

            string r = "";
            //Hours
            r += ((int) secondsLeft / 3600).ToString () + "h ";
            secondsLeft -= ((int) secondsLeft / 3600) * 3600;
            //Minutes
            r += ((int) secondsLeft / 60).ToString ("00") + "m ";
            //Seconds
            r += ((int) secondsLeft % 60).ToString ("00") + "s";
            remaningTime.text = r;
        }
    }

    private bool isFuelReady () {
        ulong diff = ((ulong) DateTime.Now.Ticks - lastFuelSearch);
        ulong m = diff / TimeSpan.TicksPerMillisecond;
        float secondsLeft = (float) (msToWait - m) / 1000.0f;

        if (secondsLeft < 0) {
            if (firstClick == 0) {
                remaningTime.text = "Search Fuel!";
            } else {
                remaningTime.text = "Ready!";
            }

            return true;
        }

        return false;

    }

    private void GetReward () {
        for (int i = 1; i < 5; i++) {
            switch (i) {
                case 1:
                    for (int j = 0; j < hydrogenChanceList.Count; j++) {
                        float rnd = UnityEngine.Random.Range (0, 101);
                        if (rnd <= hydrogenChanceList[j]) {
                            GameManager.Instance.hydrogenFuelCount++;
                        }
                    }
                    break;
                case 2:
                    for (int j = 0; j < solarChanceList.Count; j++) {
                        float rnd = UnityEngine.Random.Range (0, 101);
                        if (rnd <= solarChanceList[j]) {
                            GameManager.Instance.solarFuelCount++;
                        }
                    }
                    break;
                case 3:
                    for (int j = 0; j < ionChanceList.Count; j++) {
                        float rnd = UnityEngine.Random.Range (0, 101);
                        if (rnd <= ionChanceList[j]) {
                            GameManager.Instance.ionFuelCount++;
                        }
                    }
                    break;
                case 4:
                    for (int j = 0; j < nuclearChanceList.Count; j++) {
                        float rnd = UnityEngine.Random.Range (0, 101);
                        if (rnd <= nuclearChanceList[j]) {
                            GameManager.Instance.nuclearFuelCount++;
                        }
                    }
                    break;
            }

        }
        GameManager.Instance.SaveRewards();
        GameManager.Instance.UpdateTexts();
    }

}*/
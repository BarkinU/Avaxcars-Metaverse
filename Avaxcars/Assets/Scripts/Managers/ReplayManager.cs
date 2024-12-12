using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour {
    public RaceManager raceManager;

    public void PauseSimulation () {
        foreach (GameObject racer in raceManager.racers) {
            racer.GetComponent<ActionReplay> ().isPaused = true;
            racer.GetComponent<ActionReplay> ().isRewind = false;
            racer.GetComponent<ActionReplay> ().isSlow = false;
        }
    }

    public void ResumeSimulation () {
        foreach (GameObject racer in raceManager.racers) {
            racer.GetComponent<ActionReplay> ().isPaused = false;
            racer.GetComponent<ActionReplay> ().isRewind = false;
            racer.GetComponent<ActionReplay> ().isSlow = false;
        }

    }

    public void SlowSimulation () {
        if (raceManager.racers[1].GetComponent<ActionReplay> ().isRewind == true) {
            foreach (GameObject racer in raceManager.racers) {
                racer.GetComponent<ActionReplay> ().isPaused = false;
                racer.GetComponent<ActionReplay> ().isRewind = true;
                racer.GetComponent<ActionReplay> ().isSlow = true;
            }
        } else {
            foreach (GameObject racer in raceManager.racers) {
                racer.GetComponent<ActionReplay> ().isPaused = false;
                racer.GetComponent<ActionReplay> ().isRewind = false;
                racer.GetComponent<ActionReplay> ().isSlow = true;
            }
        }

    }

    public void RewindSimulation () {
        foreach (GameObject racer in raceManager.racers) {
            racer.GetComponent<ActionReplay> ().isPaused = false;
            racer.GetComponent<ActionReplay> ().isRewind = true;
            racer.GetComponent<ActionReplay> ().isSlow = false;
        }
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public enum CountdownFormatting { DaysHoursMinutesSeconds, HoursMinutesSeconds, MinutesSeconds, Seconds };
    public CountdownFormatting countdownFormatting = CountdownFormatting.MinutesSeconds; //Controls the way the timer string will be formatted
    public bool showMilliseconds = true; //Whether to show milliseconds in countdown formatting
    public double countdownTime; //Countdown time in seconds
    [SerializeField]
    private TMP_Text timerText;
    public double countdownInternal;
    bool countdownOver = false;
    GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        countdownInternal = countdownTime;
    }

    void OnEnable()
    {
        countdownInternal = countdownTime; //Initialize countdown

    }

    void FixedUpdate()
    {
        if (countdownInternal > 0)
        {
            countdownInternal -= Time.deltaTime;

            //Clamp the timer value so it never goes below 0
            if (countdownInternal < 0)
            {
                countdownInternal = 0;
            }

            timerText.text = FormatTime(countdownInternal, countdownFormatting, showMilliseconds);
        }
        else
        {
            if (!countdownOver)
            {
                countdownOver = true;

                Debug.Log("Countdown has finished running...");

                _gameManager.GetmissionGroupserver();
            }
        }
    }

    public string FormatTime(double time, CountdownFormatting formatting, bool includeMilliseconds)
    {
        string timeText = "";

        int intTime = (int)time;
        int days = intTime / 86400;
        int hoursTotal = intTime / 3600;
        int hoursFormatted = hoursTotal % 24;
        int minutesTotal = intTime / 60;
        int minutesFormatted = minutesTotal % 60;
        int secondsTotal = intTime;
        int secondsFormatted = intTime % 60;
        int milliseconds = (int)(time * 100);
        milliseconds = milliseconds % 100;

        if (includeMilliseconds)
        {
            if (formatting == CountdownFormatting.DaysHoursMinutesSeconds)
            {
                timeText = string.Format("{0:00}:{1:00}:{2:00}:{3:00}:{4:00}", days, hoursFormatted, minutesFormatted, secondsFormatted, milliseconds);
            }
            else if (formatting == CountdownFormatting.HoursMinutesSeconds)
            {
                timeText = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hoursTotal, minutesFormatted, secondsFormatted, milliseconds);
            }
            else if (formatting == CountdownFormatting.MinutesSeconds)
            {
                timeText = string.Format("{0:00}:{1:00}:{2:00}", minutesTotal, secondsFormatted, milliseconds);
            }
            else if (formatting == CountdownFormatting.Seconds)
            {
                timeText = string.Format("{0:00}:{1:00}", secondsTotal, milliseconds);
            }
        }
        else
        {
            if (formatting == CountdownFormatting.DaysHoursMinutesSeconds)
            {
                timeText = string.Format("Remaining Time " + "\n{0:00}:{1:00}:{2:00}:{3:00}", days, hoursFormatted, minutesFormatted, secondsFormatted);
            }
            else if (formatting == CountdownFormatting.HoursMinutesSeconds)
            {
                timeText = string.Format("Remaining Time " + "\n{0:00}:{1:00}:{2:00}", hoursTotal, minutesFormatted, secondsFormatted);
            }
            else if (formatting == CountdownFormatting.MinutesSeconds)
            {
                timeText = string.Format("{0:00}:{1:00}", minutesTotal, secondsFormatted);
            }
            else if (formatting == CountdownFormatting.Seconds)
            {
                timeText = string.Format("{0:00}", secondsTotal);
            }
        }

        return timeText;
    }
}
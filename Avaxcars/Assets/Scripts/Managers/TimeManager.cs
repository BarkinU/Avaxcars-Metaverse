using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    /* 
          necessary variables to hold all the things we need.
        php url
        timedata, the data we get back
        current time
        current date
    
 
    public static TimeManager sharedInstance = null;
    private string _url = "http://leatonm.net/wp-content/uploads/2017/candlepin/getdate.php";
    private string _timeData;
    private string _currentTime;
    private string _currentDate;
 
 
    //make sure there is only one instance of this always.
    void Awake() {
        if (sharedInstance == null) {
            sharedInstance = this;
        } else if (sharedInstance != this) {
            Destroy (gameObject);  
        }
        DontDestroyOnLoad(gameObject);
    }
 
 
    //time fether coroutine
    public IEnumerator getTime()
    {
       
        WWW www = new WWW (_url);
        yield return www;
       
        _timeData = www.text;
        string[] words = _timeData.Split('/');    
        //timerTestLabel.text = www.text;
       
 
        //setting current time
        _currentDate = words[0];
        _currentTime = words[1];
    }
 
 
    //get the current time at startup
    void Start()
    {
      
        StartCoroutine ("getTime");
    }
 
    //get the current date
    public string getCurrentDateNow()
    {
        return _currentDate;
    }
 
 
    //get the current Time
    public string getCurrentTimeNow()
    {
        return _currentTime;
    }
 
    */ 
 
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public string sender;
    public string text;
    public string date;

    public Message(string sender, string text, string date)
    {
        this.sender = sender;
        this.text = text; 
        this.date = date;
    }
    
}

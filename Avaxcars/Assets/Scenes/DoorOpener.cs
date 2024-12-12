using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;
    public Button animButton;
    public Button animButton2;


    void Start()
    {

        animButton.GetComponent<Button>().onClick.AddListener(Opener);
        animButton2.GetComponent<Button>().onClick.AddListener(Opener);


    }

    void Opener()
    {

        myDoor.Play("MenuAnimator", 0, .0f);

    }
    void Closer()
    {

        myDoor.Play("MenuAnimationReverse", 0, .0f);

    }


}

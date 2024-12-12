using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{

    public Material myMaterial;
    private float counter;
    private void Start()
    {

        myMaterial = gameObject.GetComponent<Renderer>().material;

    }
}

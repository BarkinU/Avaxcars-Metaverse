using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


[Serializable]
public class CameraPositions
{
    public string positionsName;
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;
    public int indexReference;

}
public class MoveCameraBetweenPositions : MonoBehaviour
{
    public CameraPositions[] camPositions;
    private int mCurrentIndex;
    private Vector3 currentPosition;
    private Quaternion currentQuaternion;
    private bool transactionInProcess = false;
    private float changePositionDuration = 2f;
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody rigidbody;

    float x = 0.0f;
    float y = 0.0f;

    bool orbitable;


    void Start()
    {


        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }


    IEnumerator LerpPosition(Vector3 orbitPosition, Quaternion orbitRotation, float duration, int index)
    {
     

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


        float time = 0;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, orbitPosition, time / duration);
            transform.rotation = Quaternion.Lerp(startRotation, orbitRotation, time / duration);

            time += Time.deltaTime;
            GameManager.isMenuCameraFree = false;
            if (time >= duration)
            {
                transactionInProcess = false;

                switch (camPositions[mCurrentIndex].positionsName)
                {
                    case Strings.CameraGarageFirstPoint:

                        ChangeCameraPositionByIndex(1);

                        break;
                    case Strings.CameraFreeLook:

                        GameManager.isMenuCameraFree = true;

                        break;
                }


            }
            yield return null;
        }
        transform.position = orbitPosition;

        yield return new WaitForSeconds(1f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void ChangeCameraPositionByIndex(int index)
    {

        if (!transactionInProcess)
        {
            transactionInProcess = true;   ///wait until next lerp finish 

            mCurrentIndex = index;
            currentPosition = camPositions[mCurrentIndex].cameraPosition;
            currentQuaternion.eulerAngles = camPositions[mCurrentIndex].cameraRotation;
            StartCoroutine(LerpPosition(currentPosition, currentQuaternion, changePositionDuration, index));



        }

    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            orbitable = true;
        else if (Input.GetMouseButtonUp(0))
            orbitable = false;

        if (GameManager.isMenuCameraFree)
        {
            if (orbitable)
            {
                if (target)
                {
                    bool checker = false;
                    if (checker == false)
                    {
                        Vector3 angles = transform.eulerAngles;
                        x = angles.y;
                        y = angles.x;
                        checker = true;
                    }

                    x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                    y -= +Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                    y = ClampAngle(y, yMinLimit, yMaxLimit);

                    Quaternion rotation = Quaternion.Euler(y, x, 0);

                    distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

                    RaycastHit hit;
                    if (Physics.Linecast(target.position, transform.position, out hit))
                    {
                        distance -= hit.distance;
                    }
                    Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                    Vector3 position = rotation * negDistance + target.position;

                    transform.rotation = rotation;
                    transform.position = position;
                }
            }
        }

    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

}

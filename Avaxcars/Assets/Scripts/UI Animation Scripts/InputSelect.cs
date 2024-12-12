using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputSelect : MonoBehaviour
{
    private float requestedMinSize = 12f;
    private bool isSelectedProcessDone = false;
    public TMP_Text inputFieldTitle;
    
    public void SelectInputField(float distance)
    {


        if (!isSelectedProcessDone)
        {
            isSelectedProcessDone = true;
            StartCoroutine(LerpSelectedTitleText(0.5f, distance));
        }

    }

    IEnumerator LerpSelectedTitleText(float duration, float distance)
    {
        float time = 0;

        Vector2 deSelectedPosition = inputFieldTitle.transform.position;
        Vector2 targetPosition = new Vector2(inputFieldTitle.transform.position.x, inputFieldTitle.transform.position.y + distance);

        while (time < duration)
        {
            inputFieldTitle.fontSize = Mathf.Lerp(inputFieldTitle.fontSize, requestedMinSize, duration);
            inputFieldTitle.transform.position = Vector2.Lerp(deSelectedPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

}

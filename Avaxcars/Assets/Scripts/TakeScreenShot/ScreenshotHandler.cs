using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour {

    private IEnumerator Screenshot () {
        yield return new WaitForEndOfFrame ();
        Texture2D texture = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);

        texture.ReadPixels (new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        byte[] byteArray = texture.EncodeToPNG ();
        File.WriteAllBytes (Application.dataPath + "/CameraScreenshot.png", byteArray);
        Debug.Log ("Saved CameraScreenshot.png");

        Destroy(texture);
    }

    public void ScreenshotButton () {
        StartCoroutine (Screenshot ());
    }
}
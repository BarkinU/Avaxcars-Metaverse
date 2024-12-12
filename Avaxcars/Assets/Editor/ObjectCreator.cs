using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ObjectCreator : MonoBehaviour
{

}
public class MyTools2
{
    [MenuItem("MyTools/CreateGameObjectsss")]
    static void Create()
    {

        for (int x = 1; x != 25; x++)
        {
            GameObject go = new GameObject("MyCreatedGO" + x);
            go.gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Logo");
            go.transform.localScale = new Vector3(3, 3, 0);
            go.transform.position = new Vector3(go.transform.position.x - (100 * x), 1.46f, 8.44f);
        }
    }
}

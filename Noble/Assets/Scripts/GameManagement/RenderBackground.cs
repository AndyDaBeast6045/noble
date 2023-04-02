using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class loads in background images used in game
public class RenderBackground : MonoBehaviour
{
    // image references
    public GameObject[] backgroundImages;

    // camera
    private Camera playerCamera;

    // screen space vars
    private Vector2 screenBounds;
    public float choke;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = gameObject.GetComponent<Camera>();
        screenBounds = playerCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, playerCamera.transform.position.z)
        );
        foreach (GameObject obj in backgroundImages)
        {
            LoadObjects(obj);
        }
    }

    // Load all necessary objects into the scene
    public void LoadObjects(GameObject obj)
    {
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int numChilds = (int)Mathf.Ceil(screenBounds.x * 2 / objectWidth);
        GameObject clone = Instantiate(obj) as GameObject;
        for (int i = 0; i <= numChilds; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(
                objectWidth * i,
                obj.transform.position.y,
                obj.transform.position.z
            );
            c.name = obj.name;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    // make sure that the objects are located in the right position in the scene
    public void RepostionObjects(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {
            // leftmost
            GameObject firstChild = children[1].gameObject;

            // rightmost
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth =
                lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;
            if (
                transform.position.x + screenBounds.x
                > lastChild.transform.position.x + halfObjectWidth
            )
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(
                    lastChild.transform.position.x + halfObjectWidth * 2,
                    lastChild.transform.position.y,
                    lastChild.transform.position.z
                );
            }
            else if (
                transform.position.x - screenBounds.x
                < firstChild.transform.position.x - halfObjectWidth
            )
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(
                    firstChild.transform.position.x - halfObjectWidth * 2,
                    firstChild.transform.position.y,
                    firstChild.transform.position.z
                );
            }
        }
    }

    // reposition the objects after every frame
    void LateUpdate()
    {
        foreach (GameObject obj in backgroundImages)
        {
            RepostionObjects(obj);
        }
    }
}

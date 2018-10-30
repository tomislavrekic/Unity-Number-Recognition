using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Painter : MonoBehaviour
{
    public Camera camera;
    Vector3 cameraOrigin;

    [SerializeField]
    Vector3 rayDirection;

    [SerializeField]
    float rayRange;

    Color painterColor;

    RaycastHit2D hit;
    Ray cameraRay;

    private void Start()
    {
        painterColor = Color.black;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero))
            {
                hit.collider.GetComponent<SpriteRenderer>().color = painterColor;
            }


        }
    }
}

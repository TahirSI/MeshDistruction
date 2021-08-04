using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutObjects : MonoBehaviour
{

    public GameObject ob;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Cutter.Cut(ob, Camera.main.ScreenPointToRay(Input.mousePosition), Input.GetMouseButton(0));

        /*
        // Check for mouse input
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition * 0.8f);
            RaycastHit hit;

            // Casts the ray and get the first game object hit
            Physics.Raycast(ray, out hit);

            if (hit.transform.tag == "Player")
            {
                
            }
        }
        */
    }
}

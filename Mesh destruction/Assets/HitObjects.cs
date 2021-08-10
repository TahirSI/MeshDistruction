using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjects : MonoBehaviour
{
    public int cuts = 2;
    public float forcExplostion = 900;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100))
            {
                if(hit.transform.gameObject.tag == "Player")
                {

                    var messDis = hit.transform.gameObject;

                    messDis.AddComponent<MeshDestroy>();

                    messDis.GetComponent<MeshDestroy>().Cuts = cuts;
                    messDis.GetComponent<MeshDestroy>().ExplodeForce = forcExplostion;

                    messDis.GetComponent<MeshDestroy>().DestroyMesh();


                }
            }
        }

  
    }
}

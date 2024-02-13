using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;

public class SwitchInteraction : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    Camera arCamera;
    BezierParticle bezierParticle;

    private GameObject[] pipes;
    // Start is called before the first frame update
    void Start()
    {
        arCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        var touch = Input.GetTouch(0);

        if (m_RaycastManager.Raycast(touch.position, m_Hits))
        {
            return;
        }

        if(touch.phase != TouchPhase.Began)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = arCamera.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out hit))
        {
            GameObject go = hit.collider.gameObject;
            if (hit.collider.gameObject.tag == "MainFridgeDoor")
            {
                openDoor(go.GetComponent<FridgeDoor>());
            }
        }
    }

    private void openDoor(FridgeDoor fridgeDoorObject)
    {
        if (this.pipes == null)
        {
            bezierParticle = FindObjectOfType<BezierParticle>();
            this.pipes = GameObject.FindGameObjectsWithTag("Pipe");
            var bezierParticleList = GameObject.FindObjectsOfType<BezierParticle>();
        }
        var pipe = pipes.FirstOrDefault(pipe => pipe.name.Equals("fromKuehlschranktoEnergieflussbox"));
        pipe.GetComponent<BezierParticle>().setSpeed(fridgeDoorObject.open());
    }
}
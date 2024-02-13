using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateCameraMode : MonoBehaviour
{
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject navigationMenu = null;
    [SerializeField] GameObject cameraModeMenu = null;
    [SerializeField] TextMeshProUGUI speedText = null;

    [SerializeField] ActivateScript activateCam = null;
    [SerializeField] PathFollower camPathFollower = null;

    bool cameraModeActivated = false;
    public float cameraSpeed = 1;
    bool exit = false;

    public void ToggleCameraMode()
    {
        cameraModeActivated = !cameraModeActivated;

        mainMenu.SetActive(!cameraModeActivated);
        navigationMenu.SetActive(!cameraModeActivated);
        cameraModeMenu.SetActive(cameraModeActivated);
    }

    private void Update()
    {
        if (!cameraModeActivated) return;

        if (Input.GetKeyDown(KeyCode.K) && !exit)
        {
            Camera.main.gameObject.GetComponent<PathFollower>().enabled = true;

            Debug.Log("Starte Kamerafahrt");
            activateCam.Play();
            exit = true;
        } else if(Input.GetKeyDown(KeyCode.K) && exit)
        {
            Camera.main.gameObject.GetComponent<PathFollower>().enabled = false;
            PathFollower restart = new PathFollower();
            restart.init();
            Debug.Log("Beende Kamerafahrt");
            activateCam.Play();
            exit = false;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            cameraSpeed -= 1;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            cameraSpeed += 1;
        }

        speedText.text = "Geschwindigkeit: " + cameraSpeed + " m/s";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateMovementMode : MonoBehaviour
{
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject navigationMenu = null;
    [SerializeField] GameObject moveModeMenu = null;
    [SerializeField] TextMeshProUGUI currentModeText = null;

    bool moveModeActivated = false;
    byte currentMode = 0;

    public void ToggleMovementMode()
    {
        moveModeActivated = !moveModeActivated;

        mainMenu.SetActive(!moveModeActivated);
        navigationMenu.SetActive(!moveModeActivated);
        moveModeMenu.SetActive(moveModeActivated);
    }

    private void Update()
    {
        if (!moveModeActivated) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            currentMode++;
            if (currentMode > 2) currentMode = 0;
        }

        switch (currentMode)
        {
            case 0:
                currentModeText.text = "Aktueller Bewegungsmodus: Laufen";
                break;
            case 1:
                currentModeText.text = "Aktueller Bewegungsmodus: Fliegen";
                break;
            case 2:
                currentModeText.text = "Aktueller Bewegungsmodus: Teleportieren";
                break;
        }
    }
}

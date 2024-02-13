using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public GameObject mainMenu = null;
    public GameObject mainMenuFirstSelected = null;

    public GameObject lightMenu = null;
    public GameObject lightFirstSelected = null;
    public GameObject navigationMenu = null;
    public GameObject navFirstSelected = null;
    public GameObject dofMenu = null;
    public GameObject dofFirstSelected = null;

    public GameObject lightConfig = null;
    public GameObject lightConfigFirstSelected = null;
    public GameObject movemode = null;
    public GameObject movemodeFirstSelected = null;
    public GameObject kamerafahrt = null;
    public GameObject kamerafahrtFirstSelected = null;
    public GameObject markerConfig = null;
    public GameObject markerConfigFirstSelected = null;

    [Header("Default Colors")]
    [SerializeField] ColorBlock defaultColors;
    [Header("Selected Colors")]
    [SerializeField] ColorBlock selectedColors;

    [Header("Buttons")]
    [SerializeField] Button lightButton;
    [SerializeField] Button navigationButton;
    [SerializeField] Button dofButton;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (mainMenu.activeSelf)
            {
                mainMenu.SetActive(false);
                lightMenu.SetActive(false);
                navigationMenu.SetActive(false);
                dofMenu.SetActive(false);

            } else if (!mainMenu.activeSelf)
            {

                mainMenu.SetActive(true);

                EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);

            }
        }
    }


    public void ToggleLight()
    {
        if (lightMenu.activeSelf)
        {
            lightMenu.SetActive(false);
            lightButton.colors = defaultColors;

        } else if (!lightMenu.activeSelf)
        {
            lightMenu.SetActive(true);
            lightButton.colors = selectedColors;
            navigationMenu.SetActive(false);
            dofMenu.SetActive(false);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(lightFirstSelected);
        }
    }

    public void ToggleNavigation()
    {
        if (navigationMenu.activeSelf)
        {
            navigationMenu.SetActive(false);
            navigationButton.colors = defaultColors;
        }
        else if (!navigationMenu.activeSelf)
        {
            navigationMenu.SetActive(true);
            navigationButton.colors = selectedColors;
            lightMenu.SetActive(false);
            dofMenu.SetActive(false);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(navFirstSelected);
        }
    }

    public void ToggleDoF()
    {
        if (dofMenu.activeSelf)
        {
            dofMenu.SetActive(false);
            dofButton.colors = defaultColors;
        }
        else if (!dofMenu.activeSelf)
        {
            dofMenu.SetActive(true);
            dofButton.colors = selectedColors;
            navigationMenu.SetActive(false);
            lightMenu.SetActive(false);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(dofFirstSelected);
        }
    }

    public void ToggleLightConfig()
    {
        if (lightConfig.activeSelf)
        {
            lightConfig.SetActive(false);
        }
        else if (!lightConfig.activeSelf)
        {
            lightConfig.SetActive(true);

            kamerafahrt.SetActive(false);
            markerConfig.SetActive(false);

            //EventSystem.current.SetSelectedGameObject(null);
            //EventSystem.current.SetSelectedGameObject(lightConfigFirstSelect);
        }
    }

    public void ToggleKamerafahrt()
    {
        if (kamerafahrt.activeSelf)
        {
            kamerafahrt.SetActive(false);
        }
        else if (!kamerafahrt.activeSelf)
        {
            kamerafahrt.SetActive(true);

            lightConfig.SetActive(false);
            markerConfig.SetActive(false);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(kamerafahrtFirstSelected);
        }
    }

    public void ToggleMarkerConfig()
    {
        if (markerConfig.activeSelf)
        {
            markerConfig.SetActive(false);
        }
        else if (!markerConfig.activeSelf)
        {
            markerConfig.SetActive(true);

            kamerafahrt.SetActive(false);
            lightConfig.SetActive(false);

            //EventSystem.current.SetSelectedGameObject(null);
            //EventSystem.current.SetSelectedGameObject(lightConfigFirstSelect);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateLightMode : MonoBehaviour
{
    [SerializeField] GameObject MainMenu = null;
    [SerializeField] GameObject LightMenu = null;
    [SerializeField] GameObject LightModeMenu = null;
    [SerializeField] TextMeshProUGUI currentLightText = null;
    [SerializeField] TextMeshProUGUI colorTemperatureText = null;
    [SerializeField] TextMeshProUGUI intensityText = null;
    [SerializeField] TextMeshProUGUI spotAngleText = null;

    static bool isLightmodeActivated = false;
    bool usingPointLight = true;
    int colorTemperature = 6500;
    float intensity = 1f;
    float sp_angle = 45f;
    //static values
    static float col_Temp;
    static float l_intencity;
    static bool light_type;
    static bool addlight;

    public static float spot_angle;

    public void ToggleLightMode()
    {
        isLightmodeActivated = !isLightmodeActivated;
        MainMenu.SetActive(!isLightmodeActivated);
        LightMenu.SetActive(!isLightmodeActivated);
        LightModeMenu.SetActive(isLightmodeActivated);
    }

    public void Update()
    {

        if (!isLightmodeActivated) return;

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            usingPointLight = !usingPointLight;
            set_light_type();
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            colorTemperature -= 200;
            col_temp();
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            colorTemperature += 200;
            col_temp();
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            intensity += .25f;
            light_intensity();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            intensity -= .25f;
            light_intensity();
        }
        // 9 - add light
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            addlight = true;
        }
        //Spot Angle  ->> Interactive Light.update_Parameters()
        if (Input.GetKeyDown(KeyCode.Y))
        {
            sp_angle += 5f;
            set_Spot_Angle();
            Debug.Log("Spot Angle" + spot_angle);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            sp_angle -= 5f;
            set_Spot_Angle();
            Debug.Log("Spot Angle"+ spot_angle);
        }
        if (usingPointLight)
        {
            currentLightText.text = "Aktuelles Licht: Point Light";
        }
        else
        {
            currentLightText.text = "Aktuelles Licht: Spot Light";

        }

        spotAngleText.gameObject.SetActive(!usingPointLight);

        spotAngleText.text = "Spot Angle: " + spot_angle.ToString();
        colorTemperatureText.text = "Farbtemperatur: " + colorTemperature.ToString();
        intensityText.text = "Intensität: " + intensity.ToString();
    }
    //Set and Get Color Temperature

    public void col_temp()
    {
        col_Temp = colorTemperature;

    }
    public static float get_temp()
    {
        return col_Temp;
    }
    //Light Intencity
    public void light_intensity()
    {
        l_intencity = intensity;
    }

    public static float get_Light_Intencity()
    {
        return l_intencity;
    }
    //IS Light Mode enable?
    public static bool get_Light_Mode()
    {
        return isLightmodeActivated;
    }
    //Light type
    public void set_light_type()
    {
        light_type = usingPointLight;
        //print(light_type);
    }
    //Return Light type
    public static bool Get_light_Type()
    {
        return light_type;
    }
    //Give info - is in scene light?
    public static bool add_light()
    {
        return addlight;
    }
    //Spot Angle
    private void set_Spot_Angle()
    {
        spot_angle = sp_angle;
    }
    public static float get_Spot_Angle()
    {
        return spot_angle;
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static string HomeSceneName = "HomeScreen";
    public static string BuildingSceneName = "BuildingScene";
    public static string EnergyFlowSceneName = "Energiefluss";
    public void LoadHomeScene()
    {
        SceneManager.LoadScene(HomeSceneName, LoadSceneMode.Single);
    }

    public void LoadHouseScene()
    {
        SceneManager.LoadScene(BuildingSceneName, LoadSceneMode.Single);
    }

    public void LoadEnergyFlowScene()
    {
        SceneManager.LoadScene(EnergyFlowSceneName, LoadSceneMode.Single);
    }
}

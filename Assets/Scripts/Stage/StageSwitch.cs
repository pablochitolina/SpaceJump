using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSwitch : MonoBehaviour
{
    public GameObject[] stages;
    void Start()
    {
        if (Current.sceneName == ConstelattionsController.AQUARIUS)
        {
            if (Current.stageNumber < 1 || Current.stageNumber > ConstelattionsController.SIZE_AQUARIUS) SceneManager.LoadScene(ConstelattionsController.MENU);
            stages[Current.stageNumber - 1].SetActive(true);
        }
        if (Current.sceneName == ConstelattionsController.ORION)
        {
            if (Current.stageNumber < 1 || Current.stageNumber > ConstelattionsController.SIZE_ORION) SceneManager.LoadScene(ConstelattionsController.MENU);
            stages[Current.stageNumber - 1].SetActive(true);
        }
        if (Current.sceneName == ConstelattionsController.PEGASUS)
        {
            if (Current.stageNumber < 1 || Current.stageNumber > ConstelattionsController.SIZE_PEGASUS) SceneManager.LoadScene(ConstelattionsController.MENU);
            stages[Current.stageNumber - 1].SetActive(true);
        }
        if (Current.sceneName == ConstelattionsController.HYDRA)
        {
            if (Current.stageNumber < 1 || Current.stageNumber > ConstelattionsController.SIZE_HYDRA) SceneManager.LoadScene(ConstelattionsController.MENU);
            stages[Current.stageNumber - 1].SetActive(true);
        }
        if (Current.sceneName == ConstelattionsController.CETUS)
        {
            if (Current.stageNumber < 1 || Current.stageNumber > ConstelattionsController.SIZE_CETUS) SceneManager.LoadScene(ConstelattionsController.MENU);
            stages[Current.stageNumber - 1].SetActive(true);
        }
        if (Current.sceneName == ConstelattionsController.VIRGO)
        {
            if (Current.stageNumber < 1 || Current.stageNumber > ConstelattionsController.SIZE_VIRGO) SceneManager.LoadScene(ConstelattionsController.MENU);
            stages[Current.stageNumber - 1].SetActive(true);
        }
        if (Current.sceneName == ConstelattionsController.HERCULES)
        {
            if (Current.stageNumber < 1 || Current.stageNumber > ConstelattionsController.SIZE_HERCULES) SceneManager.LoadScene(ConstelattionsController.MENU);
            stages[Current.stageNumber - 1].SetActive(true);
        }
        if (Current.sceneName == ConstelattionsController.DRACO)
        {
            if (Current.stageNumber < 1 || Current.stageNumber > ConstelattionsController.SIZE_DRACO) SceneManager.LoadScene(ConstelattionsController.MENU);
            stages[Current.stageNumber - 1].SetActive(true);
        }
        if (Current.sceneName == ConstelattionsController.ANDROMEDA)
        {
            if (Current.stageNumber < 1 || Current.stageNumber > ConstelattionsController.SIZE_ANDROMEDA) SceneManager.LoadScene(ConstelattionsController.MENU);
            stages[Current.stageNumber - 1].SetActive(true);
        }
    }

    public void next()
    {
        if (Current.sceneName == ConstelattionsController.AQUARIUS)
        {
            Current.stageNumber += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Current.sceneName == ConstelattionsController.ORION)
        {
            Current.stageNumber += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Current.sceneName == ConstelattionsController.PEGASUS)
        {
            Current.stageNumber += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Current.sceneName == ConstelattionsController.HYDRA)
        {
            Current.stageNumber += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Current.sceneName == ConstelattionsController.CETUS)
        {
            Current.stageNumber += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Current.sceneName == ConstelattionsController.VIRGO)
        {
            Current.stageNumber += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Current.sceneName == ConstelattionsController.HERCULES)
        {
            Current.stageNumber += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Current.sceneName == ConstelattionsController.DRACO)
        {
            Current.stageNumber += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Current.sceneName == ConstelattionsController.ANDROMEDA)
        {
            Current.stageNumber += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    public GameObject backAmpli1;
    public GameObject backAmpli2;
    public GameObject backAmpli3;
    public Sprite ampliActive;
    public GameObject block;

    private bool isActive = false;

    void Start()
    {
        int amplis = 0;
        if(gameObject.name == "1")
        {
            isActive = true;
        }
        else
        {
            int i = 0;
            if (System.Int32.TryParse(gameObject.name, out i) && i > 1)
            {
                isActive = GameSaveController.getByStageName(SceneManager.GetActiveScene().name + "-" + (i - 1)) != -1;
            }
            
        }
        amplis = GameSaveController.getByStageName(SceneManager.GetActiveScene().name + "-" + gameObject.name);
        block.SetActive(!isActive);
        if (amplis > 0)
        {
            GameObject ampli = new GameObject();
            ampli.transform.localScale = backAmpli1.transform.localScale;
            ampli.AddComponent<SpriteRenderer>();
            ampli.GetComponent<SpriteRenderer>().sprite = ampliActive;
            ampli.transform.SetParent(transform);
            ampli.transform.position = new Vector3(backAmpli1.transform.position.x, backAmpli1.transform.position.y, -0.2f);
        }
        if (amplis > 1)
        {
            GameObject ampli = new GameObject();
            ampli.transform.localScale = backAmpli2.transform.localScale;
            ampli.AddComponent<SpriteRenderer>();
            ampli.GetComponent<SpriteRenderer>().sprite = ampliActive;
            ampli.transform.SetParent(transform);
            ampli.transform.position = new Vector3(backAmpli2.transform.position.x, backAmpli2.transform.position.y, -0.2f);
        }
        if (amplis  > 2)
        {
            GameObject ampli = new GameObject();
            ampli.AddComponent<SpriteRenderer>();
            ampli.GetComponent<SpriteRenderer>().sprite = ampliActive;
            ampli.transform.localScale = backAmpli3.transform.localScale;
            ampli.transform.SetParent(transform);
            ampli.transform.position = new Vector3(backAmpli3.transform.position.x, backAmpli3.transform.position.y, -0.2f);
        }
    }

    public bool getIsActive()
    {
        return isActive;
    }
}

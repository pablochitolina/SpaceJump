using UnityEngine;

public class ConstelattionsController : MonoBehaviour
{
    public const string MENU = "const_menu";
    public GameObject disableLayer;

    public const string AQUARIUS = "aquarius";
    public const int SIZE_AQUARIUS = 12;

    public const string ORION = "orion";
    public const int SIZE_ORION = 24;

    public const string PEGASUS = "pegasus";
    public const int SIZE_PEGASUS = 0;

    public const string HYDRA = "hydra";
    public const int SIZE_HYDRA = 0;

    public const string CETUS = "cetus";
    public const int SIZE_CETUS = 0;

    public const string VIRGO = "virgo";
    public const int SIZE_VIRGO = 0;

    public const string HERCULES = "hercules";
    public const int SIZE_HERCULES = 0;

    public const string DRACO = "draco";
    public const int SIZE_DRACO = 0;

    public const string ANDROMEDA = "andromeda";
    public const int SIZE_ANDROMEDA = 0;

    public GameObject[] listConst;
    public TextMesh[] listText;


    private void Awake()
    {
        GameSaveController.Load();
    }

    void Start()
    {
        for (int i = 0; i < listConst.Length; i++)
        {
            if (listConst[i].gameObject.name == AQUARIUS)
            {
                int amplis = GameSaveController.contAmpli(AQUARIUS, SIZE_AQUARIUS);
                listText[i].text = amplis + "/" + (SIZE_AQUARIUS * 3);
            }
            if (listConst[i].gameObject.name == ORION)
            {
                checkActivate(i, AQUARIUS, SIZE_AQUARIUS, ORION, SIZE_ORION);
            }
            //if (listConst[i].gameObject.name == PEGASUS)
            //{
            //    checkActivate(i, ORION, SIZE_ORION, PEGASUS, SIZE_PEGASUS);
            //}
            //if (listConst[i].gameObject.name == HYDRA)
            //{
            //    checkActivate(i, PEGASUS, SIZE_PEGASUS, HYDRA, SIZE_HYDRA);
            //}
            //if (listConst[i].gameObject.name == CETUS)
            //{
            //    checkActivate(i, HYDRA, SIZE_HYDRA, CETUS, SIZE_CETUS);
            //}
            //if (listConst[i].gameObject.name == VIRGO)
            //{
            //    checkActivate(i, CETUS, SIZE_CETUS, VIRGO, SIZE_VIRGO);
            //}
            //if (listConst[i].gameObject.name == HERCULES)
            //{
            //    checkActivate(i, VIRGO, SIZE_DRACO, HERCULES, SIZE_HERCULES);
            //}
            //if (listConst[i].gameObject.name == DRACO)
            //{
            //    checkActivate(i, HERCULES, SIZE_HERCULES, DRACO, SIZE_DRACO);
            //}
            //if (listConst[i].gameObject.name == ANDROMEDA)
            //{
            //    checkActivate(i, DRACO, SIZE_DRACO, ANDROMEDA, SIZE_ANDROMEDA);
            //}
        }
    }

    public void checkActivate(int index, string namePrev, int tamPrev, string nameNow, int tamNow)
    {
        if (!GameSaveController.isStageCompleted(namePrev, tamPrev))
        {
            listConst[index].gameObject.GetComponent<Click>().isActivated = false; // TODO false;
            GameObject instate = Instantiate(disableLayer);
            instate.SetActive(true);
            instate.transform.SetParent(listConst[index].transform);
            instate.transform.position = new Vector3(listConst[index].transform.position.x, listConst[index].transform.position.y, -1);
        }
        else
        {
            listConst[index].gameObject.GetComponent<Click>().isActivated = true;
            int amplis = GameSaveController.contAmpli(nameNow, tamNow);
            listText[index].text = amplis + "/" + (tamNow * 3);
        }
    }

    public static int getMaxSizeByScene(string name)
    {
        if (name == AQUARIUS)
        {
            return SIZE_AQUARIUS;
        }
        if (name == ORION)
        {
            return SIZE_ORION;
        }
        if (name == PEGASUS)
        {
            return SIZE_PEGASUS;
        }
        if (name == HYDRA)
        {
            return SIZE_HYDRA;
        }
        if (name == CETUS)
        {
            return SIZE_CETUS;
        }
        if (name == VIRGO)
        {
            return SIZE_VIRGO;
        }
        if (name == HERCULES)
        {
            return SIZE_HERCULES;
        }
        if (name == DRACO)
        {
            return SIZE_DRACO;
        }
        if (name == ANDROMEDA)
        {
            return SIZE_ANDROMEDA;
        }
        return 0;
    }

}

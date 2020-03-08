using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSonarController : MonoBehaviour
{
    private List<ActiveSonarBean> sonars = new List<ActiveSonarBean>();
    public GameObject redSonar;
    public GameObject greenSonar;
    public GameObject blueSonar;
    public List<GameObject> posAmplifics = new List<GameObject>();
    int contRed = 0;
    int contGreen = 0;
    int contBlue = 0;

    public Sprite spriteBlue;
    public Sprite spriteRed;
    public Sprite spriteGreen;
    public Sprite spriteAll;
    public Sprite spriteOff;

    public GameObject fundoSonar0;
    public GameObject fundoSonar1;
    public GameObject fundoSonar2;
    public GameObject fundoSonar3;


    public GameObject parent;

    public void addSonar(ActiveSonarBean sonar)
    {
 
        bool contem = false;
        foreach(ActiveSonarBean son in sonars)
        {
            if(Vector2.Distance(son.getPosSonar(), sonar.getPosSonar()) < 0.1f && (son.getColor() == sonar.getColor() && son.getRotation() == sonar.getRotation()))
            {
                contem = true;
                break;
            }
        }
        //sonars.Add(sonar);
        if (!contem) sonars.Add(sonar);
    }

    public void cleanSonar()
    {
        sonars = new List<ActiveSonarBean>();
        GameObject[] sonarsDestroy = GameObject.FindGameObjectsWithTag("sonarDestroy");
        foreach (GameObject sonarDestroy in sonarsDestroy)
        {
            //sonarDestroy.GetComponent<Animator>().Play("base_sonar_some");
            Destroy(sonarDestroy);
        }
        GameObject[] movelsDestroy = GameObject.FindGameObjectsWithTag("movelDestroy");
        foreach (GameObject movelDestroy in movelsDestroy)
        {
            Destroy(movelDestroy);
        }
        foreach (GameObject pos in posAmplifics)
        {
            pos.GetComponent<SpriteRenderer>().sprite = spriteOff;
        }

    }

    public void contAmplific()
    {
        contRed = 0;
        contBlue = 0;
        contGreen = 0;
        foreach (ActiveSonarBean sonar in sonars)
        {
            foreach (GameObject pos in posAmplifics)
            {
                if (Vector2.Distance(sonar.getPosSonar(), pos.transform.position) < 0.1f)
                {
                    if (sonar.getColor() == "redSonar") contRed++;
                    if (sonar.getColor() == "greenSonar") contGreen++;
                    if (sonar.getColor() == "blueSonar") contBlue++;
                }
            }
        }
    }

    public int getContAmplific(string color)
    {
        if (color == "redSonar") return contRed;
        if (color == "greenSonar") return contGreen;
        if (color == "blueSonar") return contBlue;
        return 0;
    }

    public void AnimSonar(string color, int count, bool isCriarBack, List<GameObject> listBases)
    {
        string cor = color;
        foreach (ActiveSonarBean sonar in sonars)
        {
            bool achou = false;
            if(listBases != null)
            {
                foreach(GameObject go in listBases)
                {
                    if (Vector2.Distance(go.transform.position, sonar.getPosSonar()) < 0.2f)
                    {
                        achou = true;
                        break;
                    }
                }
                if (!achou) continue; ;
            }
            
            bool temAplific = false;
            GameObject ampliCor = null;
            foreach (GameObject pos in posAmplifics)
            {
                if (Vector2.Distance(pos.transform.position, sonar.getPosSonar()) < 0.1f){
                    temAplific = true;
                    ampliCor = pos;
                }
            }
            //if (temAplific) continue;
            GameObject objInstance = null;

            if (sonar.getColor() == "redSonar" && (cor != null ? sonar.getColor() == cor : true))
            {
                objInstance = criarAnim(redSonar, sonar.getPosSonar(), sonar.getRotation());

                if (temAplific && ampliCor != null) ampliCor.GetComponent<SpriteRenderer>().sprite = spriteAll;
                
                if (cor != null)
                {
                    if (isCriarBack)
                    {
                        criarBack(fundoSonar0, sonar.getPosSonar(), new Color32(237, 28, 36, 255), sonar.getRotation(), "sonarDestroy");
                    }
                    else
                    {
                        criarBack(getGOByCount(count), sonar.getPosSonar(), new Color32(237, 28, 36, 255), sonar.getRotation(), "sonarDestroy");
                        objInstance.GetComponent<Animator>().Play("anim_loop_0");
                    }
                }
                else
                {
                    objInstance.GetComponent<Animator>().Play("base_sonar_sl");
                }
            }
            if (sonar.getColor() == "greenSonar" && (cor != null ? sonar.getColor() == cor : true))
            {

                objInstance = criarAnim(greenSonar, sonar.getPosSonar(), sonar.getRotation());

                if (temAplific) ampliCor.GetComponent<SpriteRenderer>().sprite = spriteAll;

                if (cor != null)
                {
                    if (isCriarBack)
                    {
                        criarBack(fundoSonar0, sonar.getPosSonar(), new Color32(138, 201, 38, 255), sonar.getRotation(), "sonarDestroy");
                    }
                    else
                    {
                        criarBack(getGOByCount(count), sonar.getPosSonar(), new Color32(138, 201, 38, 255), sonar.getRotation(), "sonarDestroy");
                        objInstance.GetComponent<Animator>().Play("anim_loop_0");
                    }
                }
                else
                {
                    objInstance.GetComponent<Animator>().Play("base_sonar_sl");
                }
            }
            if (sonar.getColor() == "blueSonar" && (cor != null ? sonar.getColor() == cor : true))
            {
                objInstance = criarAnim(blueSonar, sonar.getPosSonar(), sonar.getRotation());

                if (temAplific) ampliCor.GetComponent<SpriteRenderer>().sprite = spriteAll;

                if (cor != null)
                {
                    if (isCriarBack)
                    {
                        criarBack(fundoSonar0, sonar.getPosSonar(), new Color32(38, 133, 201, 255), sonar.getRotation(), "sonarDestroy");
                    }
                    else
                    {
                        criarBack(getGOByCount(count), sonar.getPosSonar(), new Color32(38, 133, 201, 255), sonar.getRotation(), "sonarDestroy");
                        objInstance.GetComponent<Animator>().Play("anim_loop_0");
                    }
                }
                else
                {
                    objInstance.GetComponent<Animator>().Play("base_sonar_sl");
                }
            }
        }
    }

    private void criarBack(GameObject obj, Vector2 pos, Color32 color, Quaternion rotation, string tag)
    {
        GameObject fundo = Instantiate(obj, new Vector3(pos.x, pos.y, 1), rotation);
        fundo.transform.SetParent(parent.transform);
        fundo.transform.position = new Vector3(pos.x, pos.y, 1.1f);
        fundo.transform.localScale = new Vector3(0.868f, 1f, 1f);
        fundo.SetActive(true);
        fundo.tag = tag;
        fundo.GetComponent<SpriteRenderer>().color = color;
    }

    private GameObject criarAnim(GameObject obj, Vector2 pos, Quaternion rotation)
    {
        GameObject objInstance = Instantiate(obj, new Vector3(pos.x, pos.y, 1), rotation);
        objInstance.transform.SetParent(parent.transform);
        objInstance.transform.position = new Vector3(pos.x, pos.y, 1);
        objInstance.transform.localScale = new Vector3(0.8660256f, 1f, 1f);
        objInstance.SetActive(true);
        objInstance.tag = "sonarDestroy";

        return objInstance;
    }

    private GameObject getGOByCount(int count)
    {
        if (count == 3) return fundoSonar3;
        if (count == 2) return fundoSonar2;
        if (count == 1) return fundoSonar1;
        return fundoSonar0;
    }

    public List<ActiveSonarBean> getSonars()
    {
        return sonars;
    }

    public void setSonars(List<ActiveSonarBean> s)
    {
        sonars = s;
    }
}

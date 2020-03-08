using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BaseMovelController : MonoBehaviour
{
    private const float DELAY_ROWS = 0.05f;
    private const float DELAY_ANIMATOR = 0.2f;

    public GameObject rb1;
    public GameObject rb2;
    public GameObject rr1;
    public GameObject rr2;
    public GameObject rg1;
    public GameObject rg2;

    int irb1 = 0;
    int irb2 = 0;
    int irr1 = 0;
    int irr2 = 0;
    int irg1 = 0;
    int irg2 = 0;

    private List<SonarHexController> listRB1 = new List<SonarHexController>();
    private List<SonarHexController> listRB2 = new List<SonarHexController>();
    private List<SonarHexController> listRR1 = new List<SonarHexController>();
    private List<SonarHexController> listRR2 = new List<SonarHexController>();
    private List<SonarHexController> listRG1 = new List<SonarHexController>();
    private List<SonarHexController> listRG2 = new List<SonarHexController>();

    public string ru = "";
    public string r = "";
    public string rd = "";
    public string ld = "";
    public string l = "";
    public string lu = "";
  
    public Vector2 posStart;
    public Vector2 posInit;

    public int rotation = 0;

    private bool isRb1Anima = true;
    private bool isRb2Anima = true;
    private bool isRr1Anima = true;
    private bool isRr2Anima = true;
    private bool isRg1Anima = true;
    private bool isRg2Anima = true;

    private bool isRb1AnimaM = true;
    private bool isRb2AnimaM = true;
    private bool isRr1AnimaM = true;
    private bool isRr2AnimaM = true;
    private bool isRg1AnimaM = true;
    private bool isRg2AnimaM = true;

    private List<ActiveSonarBean> listSonarRb1 = new List<ActiveSonarBean>();
    private List<ActiveSonarBean> listSonarRb2 = new List<ActiveSonarBean>();
    private List<ActiveSonarBean> listSonarRg1 = new List<ActiveSonarBean>();
    private List<ActiveSonarBean> listSonarRg2 = new List<ActiveSonarBean>();
    private List<ActiveSonarBean> listSonarRr1 = new List<ActiveSonarBean>();
    private List<ActiveSonarBean> listSonarRr2 = new List<ActiveSonarBean>();

    private bool deuHitRb1 = false;
    private bool deuHitRb2 = false;
    private bool deuHitRg1 = false;
    private bool deuHitRg2 = false;
    private bool deuHitRr1 = false;
    private bool deuHitRr2 = false;

    public GameObject animator;

    private ActiveSonarController scriptActiveSonarController;

    private RouteController routeController;

    public GameObject parent;

    private System.Action callbackSonar = null;

    private bool mostraSonar = false;

    void Start()
    {
        routeController = Camera.main.GetComponent<RouteController>();
        scriptActiveSonarController = Camera.main.GetComponent<ActiveSonarController>();
        rb1.SetActive(true);
        foreach(SonarHexController t in rb1.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null)  listRB1.Add(t);
        }
        rb2.SetActive(true);
        foreach (SonarHexController t in rb2.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null) listRB2.Add(t);
        }
        rr1.SetActive(true);
        foreach (SonarHexController t in rr1.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null) listRR1.Add(t);
        }
        rr2.SetActive(true);
        foreach (SonarHexController t in rr2.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null) listRR2.Add(t);
        }
        rg1.SetActive(true);
        foreach (SonarHexController t in rg1.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null) listRG1.Add(t);
        }
        rg2.SetActive(true);
        foreach (SonarHexController t in rg2.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null) listRG2.Add(t);
        }
    }

    public void playAnim(string name)
    {
        GameObject objInstance = Instantiate(animator);
        objInstance.transform.SetParent(parent.transform);
        objInstance.transform.localScale = new Vector3(0.8660256f, 1f, 1f);
        objInstance.transform.position = transform.position;
        objInstance.gameObject.tag = "movelDestroy";
        objInstance.GetComponent<Animator>().Play(name);
    }

    public void Rotate(System.Action callback, GameObject copy, float delay)
    {
        transform.position = posInit;
        mudaLabels();
        StartCoroutine(rotate(callback, copy, delay));
    }

    IEnumerator rotate(System.Action callback, GameObject copy, float delay)
    {
        int iteracoes = 5;
        float passo = delay / iteracoes;
        for(int i = 0; i < iteracoes; i++)
        {
            yield return new WaitForSeconds(passo);
            rotation += (60 / iteracoes) * (-1);
            transform.eulerAngles = Vector3.forward * rotation;
            copy.transform.eulerAngles = Vector3.forward * rotation;
        }
        callback();
    }

    private void mudaLabels()
    {
        string aux_ru = ru;
        ru = lu;
        lu = l;
        l = ld;
        ld = rd;
        rd = r;
        r = aux_ru;
    }

    public void MouseUp(System.Action c, bool m)
    {

        mostraSonar = m;
        callbackSonar = c;
        irb1 = 0;
        irb2 = 0;
        irr1 = 0;
        irr2 = 0;
        irg1 = 0;
        irg2 = 0;
        isRb1Anima = true;
        isRb2Anima = true;
        isRr1Anima = true;
        isRr2Anima = true;
        isRg1Anima = true;
        isRg2Anima = true;
        deuHitRb1 = false;
        deuHitRb2 = false;
        deuHitRg1 = false;
        deuHitRg2 = false;
        deuHitRr1 = false;
        deuHitRr2 = false;
        listSonarRb1 = new List<ActiveSonarBean>();
        listSonarRb2 = new List<ActiveSonarBean>();
        listSonarRg1 = new List<ActiveSonarBean>();
        listSonarRg2 = new List<ActiveSonarBean>();
        listSonarRr1 = new List<ActiveSonarBean>();
        listSonarRr2 = new List<ActiveSonarBean>();
        //StartCoroutine(Sonar(callback, mostraSonar));
        Anima();
    }


    public void MouseUpMove(GameObject go)
    {
        isRb1AnimaM = true;
        isRb2AnimaM = true;
        isRr1AnimaM = true;
        isRr2AnimaM = true;
        isRg1AnimaM = true;
        isRg2AnimaM = true;
        rb1.SetActive(true);
        foreach (SonarHexController t in rb1.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null) listRB1.Add(t);
        }
        rb2.SetActive(true);
        foreach (SonarHexController t in rb2.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null) listRB2.Add(t);
        }
        rr1.SetActive(true);
        foreach (SonarHexController t in rr1.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null) listRR1.Add(t);
        }
        rr2.SetActive(true);
        foreach (SonarHexController t in rr2.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null)  listRR2.Add(t);
        }
        rg1.SetActive(true);
        foreach (SonarHexController t in rg1.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null) listRG1.Add(t);
        }
        rg2.SetActive(true);
        foreach (SonarHexController t in rg2.GetComponentsInChildren<SonarHexController>())
        {
            if (t != null && t.gameObject != null) listRG2.Add(t);
        }
        for (int i = 0; i < listRB1.Count; i++)
        {
            if (isRb1AnimaM) isRb1AnimaM = listRB1[i].AnimaMove(go);
            if (isRb2AnimaM) isRb2AnimaM = listRB2[i].AnimaMove(go);
            if (isRr1AnimaM) isRr1AnimaM = listRR1[i].AnimaMove(go);
            if (isRr2AnimaM) isRr2AnimaM = listRR2[i].AnimaMove(go);
            if (isRg1AnimaM) isRg1AnimaM = listRG1[i].AnimaMove(go);
            if (isRg2AnimaM) isRg2AnimaM = listRG2[i].AnimaMove(go);
        }
    }

    private void testaHits()
    {
        
        if (deuHitRb1)
        {
            foreach (ActiveSonarBean sonar in listSonarRb1)
            {
                ActiveSonarBean so = sonar;
                so.setBaseOrigem(gameObject);
                scriptActiveSonarController.addSonar(so);
            }
        }
        if (deuHitRb2)
        {
            foreach (ActiveSonarBean sonar in listSonarRb2)
            {
                ActiveSonarBean so = sonar;
                so.setBaseOrigem(gameObject);
                scriptActiveSonarController.addSonar(so);
            }
        }
        if (deuHitRr1)
        {
            foreach (ActiveSonarBean sonar in listSonarRr1)
            {
                ActiveSonarBean so = sonar;
                so.setBaseOrigem(gameObject);
                scriptActiveSonarController.addSonar(so);
            }
        }
        if (deuHitRr2)
        {
            foreach (ActiveSonarBean sonar in listSonarRr2)
            {
                ActiveSonarBean so = sonar;
                so.setBaseOrigem(gameObject);
                scriptActiveSonarController.addSonar(so);
            }
        }
        if (deuHitRg1)
        {
            foreach (ActiveSonarBean sonar in listSonarRg1)
            {
                ActiveSonarBean so = sonar;
                so.setBaseOrigem(gameObject);
                scriptActiveSonarController.addSonar(so);
            }
        }
        if (deuHitRg2)
        {
            foreach (ActiveSonarBean sonar in listSonarRg2)
            {
                ActiveSonarBean so = sonar;
                so.setBaseOrigem(gameObject);
                scriptActiveSonarController.addSonar(so);
            }
        }
        callbackSonar();
    }

    private void Anima()
    {
 
        listRB1[0].Anima(callbackRb1, gameObject, mostraSonar);
        listRB2[0].Anima(callbackRb2, gameObject, mostraSonar);
        listRR1[0].Anima(callbackRr1, gameObject, mostraSonar);
        listRR2[0].Anima(callbackRr2, gameObject, mostraSonar);
        listRG1[0].Anima(callbackRg1, gameObject, mostraSonar);
        listRG2[0].Anima(callbackRg2, gameObject, mostraSonar);
        
    }

    private const float DELAY_TESTA_HIT = 0.1f;
    private float timeTestaHit = DELAY_TESTA_HIT;

    private void Update()
    {
        if (!(isRb1Anima || isRb2Anima || isRr1Anima || isRr2Anima || isRg1Anima || isRg2Anima))
        {
            timeTestaHit -= Time.deltaTime;
            if(timeTestaHit < 0)
            {
                isRb1Anima = true;
                isRb2Anima = true;
                isRr1Anima = true;
                isRr2Anima = true;
                isRg1Anima = true;
                isRg2Anima = true;
                testaHits();
            }
        }
    }

    public void callbackRb1(SonarHexBean sonarHex)
    {
        if (sonarHex.isAnima())
        {
            if (sonarHex.getSonar() != null) listSonarRb1.Add(sonarHex.getSonar());
            listRB1[++irb1].Anima(callbackRb1, gameObject, mostraSonar);
        }
        else
        {
            isRb1Anima = false;
            deuHitRb1 = sonarHex.isHit();
        }
        timeTestaHit = DELAY_TESTA_HIT;
    }
    public void callbackRb2(SonarHexBean sonarHex)
    {
        if (sonarHex.isAnima())
        {
            if (sonarHex.getSonar() != null) listSonarRb2.Add(sonarHex.getSonar());
            listRB2[++irb2].Anima(callbackRb2, gameObject, mostraSonar);
        }
        else
        {
            isRb2Anima = false;
            deuHitRb2 = sonarHex.isHit();
        }
        timeTestaHit = DELAY_TESTA_HIT;
    }
    public void callbackRr1(SonarHexBean sonarHex)
    {
        if (sonarHex.isAnima())
        {
            if (sonarHex.getSonar() != null) listSonarRr1.Add(sonarHex.getSonar());
            listRR1[++irr1].Anima(callbackRr1, gameObject, mostraSonar);
        }
        else
        {
            isRr1Anima = false;
            deuHitRr1 = sonarHex.isHit();
        }
        timeTestaHit = DELAY_TESTA_HIT;
    }
    public void callbackRr2(SonarHexBean sonarHex)
    {
        if (sonarHex.isAnima())
        {
            if (sonarHex.getSonar() != null) listSonarRr2.Add(sonarHex.getSonar());
            listRR2[++irr2].Anima(callbackRr2, gameObject, mostraSonar);
        }
        else
        {
            isRr2Anima = false;
            deuHitRr2 = sonarHex.isHit();
        }
        timeTestaHit = DELAY_TESTA_HIT;
    }
    public void callbackRg1(SonarHexBean sonarHex)
    {
        if (sonarHex.isAnima())
        {
            if (sonarHex.getSonar() != null) listSonarRg1.Add(sonarHex.getSonar());
            listRG1[++irg1].Anima(callbackRg1, gameObject, mostraSonar);
        }
        else
        {
            isRg1Anima = false;
            deuHitRg1 = sonarHex.isHit();
        }
        timeTestaHit = DELAY_TESTA_HIT;
    }
    public void callbackRg2(SonarHexBean sonarHex)
    {
        if (sonarHex.isAnima())
        {
            if (sonarHex.getSonar() != null) listSonarRg2.Add(sonarHex.getSonar());
            listRG2[++irg2].Anima(callbackRg2, gameObject, mostraSonar);
        }
        else
        {
            isRg2Anima = false;
            deuHitRg2 = sonarHex.isHit();
        }
        timeTestaHit = DELAY_TESTA_HIT;
    }
}

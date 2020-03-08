using System.Collections;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public GameObject ok;
    public GameObject ul;
    public GameObject ur;
    public GameObject r;
    public GameObject l;
    public GameObject dr;
    public GameObject dl;
    public GameObject rot;
    private GameObject baseMovel;

    private TilesController scriptTiles;

    private const float MAX_TIME_CLICK = 1f;
    private bool click = false;
    private float tempoClick = MAX_TIME_CLICK;
    private string lastClickedTag = "";
    private const float DELAY_CLICK = 0f;
    private const float DELAY_SOME = 6;
    private float tempoSome = DELAY_SOME;
    
    void Start()
    {
        scriptTiles = Camera.main.GetComponent<TilesController>();
    }

    public void setBaseMovel(GameObject b)
    {
        Current.isControll = true;
        tempoSome = DELAY_SOME;
        baseMovel = b;
    }

    public GameObject getBaseMovel()
    {
        return baseMovel;
    }
    
    void Update()
    {
        if (Current.isOnlyDrag) return;
        if (Current.isControll)
        {
            tempoSome -= Time.deltaTime;
            if (tempoSome < 0) some();
        }
        if (click)
        {
            tempoClick -= Time.deltaTime;
            if (tempoClick <= 0f)
            {
                clickUp(lastClickedTag, true);
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "upRight")
                {
                    tempoSome = DELAY_SOME;
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    ur.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "upLeft")
                {
                    tempoSome = DELAY_SOME;
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    ul.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "left")
                {
                    tempoSome = DELAY_SOME;
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    l.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "right")
                {
                    tempoSome = DELAY_SOME;
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    r.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "downRight")
                {
                    tempoSome = DELAY_SOME;
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    dr.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "downLeft")
                {
                    tempoSome = DELAY_SOME;
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    dl.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "ok")
                {
                    tempoSome = DELAY_SOME;
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    ok.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "rot")
                {
                    tempoSome = DELAY_SOME;
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    rot.GetComponent<Animator>().Play("click");
                }
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && click)
            {
                if (hit.collider.gameObject.tag != lastClickedTag) return;
                clickUp(hit.collider.gameObject.tag, false);
            }
        }
    }

    private void clickUp(string tag, bool isCancel)
    {
        if (tag == "ok")
        {
            click = false;
            ok.GetComponent<Animator>().Play("clickUp");
            if(!isCancel) StartCoroutine(delayClick(tag));
        }
        if (tag == "upRight")
        {
            click = false;
            ur.GetComponent<Animator>().Play("clickUp");
            if (!isCancel)  StartCoroutine(delayClick(tag));
        }
        if (tag == "upLeft")
        {
            click = false;
            ul.GetComponent<Animator>().Play("clickUp");
            if (!isCancel)  StartCoroutine(delayClick(tag));
        }
        if (tag == "downRight")
        {
            click = false;
            dr.GetComponent<Animator>().Play("clickUp");
            if (!isCancel)  StartCoroutine(delayClick(tag));
        }
        if (tag == "downLeft")
        {
            click = false;
            dl.GetComponent<Animator>().Play("clickUp");
            if (!isCancel)  StartCoroutine(delayClick(tag));
        }
        if (tag == "left")
        {
            click = false;
            l.GetComponent<Animator>().Play("clickUp");
            if (!isCancel)  StartCoroutine(delayClick(tag));
        }
        if (tag == "right")
        {
            click = false;
            r.GetComponent<Animator>().Play("clickUp");
            if (!isCancel)  StartCoroutine(delayClick(tag));
        }
        if (tag == "rot")
        {
            click = false;
            rot.GetComponent<Animator>().Play("clickUp");
            if (!isCancel)  StartCoroutine(delayClick(tag));
        }
    }

    IEnumerator delayClick(string tag)
    {
        string key = StageCreator.getKey(baseMovel.transform);
        baseMovel.GetComponent<BaseMovelController>().posInit = baseMovel.transform.position;
        yield return new WaitForSeconds(DELAY_CLICK);
        bool moveu = true;
        if(Vector2.Distance(baseMovel.GetComponent<BaseMovelController>().posStart, baseMovel.transform.position) < 0.1 && (tag == "upRight" || tag == "upLeft" || tag == "left" || tag == "right" || tag == "downRight" || tag == "downLeft"))
        {
            baseMovel.transform.position = Current.firstPos;
        }
        else
        {
            if (tag == "upRight")
            {
                baseMovel.transform.position = new Vector2(baseMovel.transform.position.x + 0.1f, baseMovel.transform.position.y + Current.minDist);
            }
            if (tag == "upLeft")
            {
                baseMovel.transform.position = new Vector2(baseMovel.transform.position.x - 0.1f, baseMovel.transform.position.y + Current.minDist);
            }
            if (tag == "left")
            {
                baseMovel.transform.position = new Vector2(baseMovel.transform.position.x - Current.minDist, baseMovel.transform.position.y);
            }
            if (tag == "right")
            {
                baseMovel.transform.position = new Vector2(baseMovel.transform.position.x + Current.minDist, baseMovel.transform.position.y);
            }
            if (tag == "downRight")
            {
                baseMovel.transform.position = new Vector2(baseMovel.transform.position.x + 0.1f, baseMovel.transform.position.y - Current.minDist);
            }
            if (tag == "downLeft")
            {
                baseMovel.transform.position = new Vector2(baseMovel.transform.position.x - 0.1f, baseMovel.transform.position.y - Current.minDist);
            }
        }
        if (tag == "rot")
        {
            baseMovel.GetComponent<Drag>().rotate();
        }
        if (tag == "ok")
        {
            moveu = false;
            some();
        }
        if (moveu)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(baseMovel.transform.position, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "back")
                {
                    baseMovel.transform.position = hit.collider.gameObject.transform.position;
                    break;
                }
            }
            scriptTiles.ChangePosition(key, baseMovel, baseMovel.GetComponent<BaseMovelController>().posInit, baseMovel.GetComponent<BaseMovelController>().posStart, Drag.DELAY_ANIMA);
        }
    }

    private void some()
    {
        baseMovel.GetComponent<Drag>().some();
        someOut();
    }

    public void someOut()
    {
        Current.isControll = false;
        gameObject.SetActive(false);
    }
}

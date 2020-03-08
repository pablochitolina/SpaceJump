using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPrevController : MonoBehaviour
{
    public GameObject btnPrev;
    public GameObject btnNext;
    public TextMesh cont;
    private int pages;
    private int page = 1;
    private float distance = 0;

    public GameObject itens;
    public GameObject layerNext;
    public GameObject layerPrev;

    private Swipe dir = Swipe.None;
    
    void Start()
    {
        pages = itens.transform.childCount;
        if (page == 1)
        {
            layerPrev.SetActive(true);
        }
        else
        {
            layerPrev.SetActive(false);
        }
        if (page == pages)
        {
            layerNext.SetActive(true);
        }
        else
        {
            layerNext.SetActive(false);
        }
        cont.text = page + "/" + pages;
        if (pages > 1)
        {
            distance = itens.transform.GetChild(1).transform.position.x - itens.transform.GetChild(0).transform.position.x;
        }
    }
    
    float velocidade = 20f;
    void Update()
    {
        if (itens.transform.position.x > (page - 1) * -distance && dir == Swipe.Right)
        {
            itens.transform.Translate(-Vector3.right * velocidade * Time.deltaTime);
        }
        if (itens.transform.position.x < (page - 1) * -distance && dir == Swipe.Left)
        {
            itens.transform.Translate(Vector3.right * velocidade * Time.deltaTime);
        }
        if (Vector2.Distance(itens.transform.position, new Vector2((page - 1) * -distance, 0)) < 0.3f && dir != Swipe.None)
        {
            dir = Swipe.None;
            itens.transform.position = new Vector2((page - 1) * -distance, 0);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "nextPage" && page < pages)
                {
                    
                    page++;
                    StartCoroutine(delayCheck());
                    btnNext.GetComponent<Animator>().Play("clickDownUp");
                    dir = Swipe.Right;
                }
                if (hit.collider.gameObject.tag == "prevPage" && page > 1)
                {
                    page--;
                    StartCoroutine(delayCheck());
                    btnPrev.GetComponent<Animator>().Play("clickDownUp");
                    dir = Swipe.Left;
                }
                cont.text = page + "/" + pages;
            }
        }
    }

    IEnumerator delayCheck()
    {
        if (page < 1 || page > pages)
        {
            itens.transform.position = new Vector3(0, 0, 0);
            page = 1;
        }
        yield return new WaitForSeconds(0.05f);
        if (page == 1)
        {
            layerPrev.SetActive(true);
        }
        else
        {
            layerPrev.SetActive(false);
        }
        if (page == pages)
        {
            layerNext.SetActive(true);
        }
        else
        {
            layerNext.SetActive(false);
        }
    }
}

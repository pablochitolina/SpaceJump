using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CircleCollider2D))]

public class Click : MonoBehaviour
{
    public Animator animator;

    private const float MAX_TIME_CLICK = 1f;
    private bool click = false;
    private float tempoClick = MAX_TIME_CLICK;
    private const float DELAY_CLICK = 0.1f;
    private bool isMenuInicial = false;
    public bool isActivated = false; // TODO setar true para iniciar sempre ativado

    private void Update()
    {
        if (click)
        {
            tempoClick -= Time.deltaTime;
            if (tempoClick <= 0f)
            {
                clickUp();
            }
        }
    }

    private void Start()
    {
        isMenuInicial = SceneManager.GetActiveScene().name == ConstelattionsController.MENU;
    }

    void OnMouseDown()
    {
        if (!isMenuInicial && !gameObject.GetComponent<StageController>().getIsActive()) return;
        if (!isActivated) return; 
        click = true;
        animator.Play("click");
        
    }

    void OnMouseUp()
    {
       if(click) clickUp();
    }

    private void clickUp()
    {
        click = false;
        animator.Play("clickUp");
        StartCoroutine(delayClick(tag));
    }

    IEnumerator delayClick(string tag)
    {
        yield return new WaitForSeconds(DELAY_CLICK);
        Current.sceneName = SceneManager.GetActiveScene().name;
        string name = "";
        if (isMenuInicial)
        {
            name = gameObject.name;
            
        }
        else
        {
            Current.stageNumber = int.Parse(gameObject.name);
            name = Current.sceneName + "_stg";
        }
        SceneManager.LoadScene(name);
    }
}

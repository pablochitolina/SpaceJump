using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject btnAgain;
    public GameObject btnNext;
    public GameObject btnMenu;
    public GameObject congratz;
    public TextMesh unlockedName;
    public GameObject btnGo;
    private string goTo = "const_menu";

    public GameObject anim1;
    public GameObject anim2;
    public GameObject anim3;
    public TextMesh stage;

    private const float MAX_TIME_CLICK = 1f;
    private bool click = false;
    private float tempoClick = MAX_TIME_CLICK;
    private string lastClickedTag = "";
    private const float DELAY_CLICK = 0.1f;
    private const float DELAY_ANIM_AMPLI = 0.1f;


    void OnEnable()
    {
        string text = "";
        if(Current.stageNumber < 10)
        {
            text = "STAGE 0" + Current.stageNumber;
        }
        else
        {
            text = "STAGE " + Current.stageNumber;
        }
        stage.text = text;
        if (Current.sceneName == ConstelattionsController.AQUARIUS)
        {
            checkMenuEnd(ConstelattionsController.SIZE_AQUARIUS, ConstelattionsController.ORION);
        }
        if (Current.sceneName == ConstelattionsController.ORION)
        {
            checkMenuEnd(ConstelattionsController.SIZE_ORION, "const_menu");// ConstelattionsController.PEGASUS);
        }
        if (Current.sceneName == ConstelattionsController.PEGASUS)
        {
            checkMenuEnd(ConstelattionsController.SIZE_PEGASUS, "const_menu");// ConstelattionsController.HYDRA);
        }
        if (Current.sceneName == ConstelattionsController.HYDRA)
        {
            checkMenuEnd(ConstelattionsController.SIZE_HYDRA, "const_menu");// ConstelattionsController.CETUS);
        }
        if (Current.sceneName == ConstelattionsController.CETUS)
        {
            checkMenuEnd(ConstelattionsController.SIZE_CETUS, "const_menu");// ConstelattionsController.VIRGO);
        }
        if (Current.sceneName == ConstelattionsController.VIRGO)
        {
            checkMenuEnd(ConstelattionsController.SIZE_VIRGO, "const_menu");// ConstelattionsController.HERCULES);
        }
        if (Current.sceneName == ConstelattionsController.HERCULES)
        {
            checkMenuEnd(ConstelattionsController.SIZE_HERCULES, "const_menu");// ConstelattionsController.DRACO);
        }
        if (Current.sceneName == ConstelattionsController.DRACO)
        {
            checkMenuEnd(ConstelattionsController.SIZE_DRACO, "const_menu");// ConstelattionsController.ANDROMEDA);
        }
        if (Current.sceneName == ConstelattionsController.ANDROMEDA)
        {
            btnNext.transform.parent.gameObject.SetActive((Current.stageNumber < ConstelattionsController.SIZE_ANDROMEDA));
        }
        StartCoroutine(delayAnimaBonus());
    }

    private void checkMenuEnd(int size, string nameProx)
    {
        btnNext.transform.parent.gameObject.SetActive((Current.stageNumber < size));
        if (PlayerPrefs.GetString(nameProx) != "s" && Current.stageNumber == size)
        {
            PlayerPrefs.SetString(nameProx, "s");
            congratz.SetActive(true);
            goTo = nameProx;
        }
    }

    IEnumerator delayAnimaBonus()
    {
        yield return new WaitForSeconds(0.3f);
        animaBonus();
    }

    void animaBonus()
    {
        int numBonus = Camera.main.GetComponent<RouteController>().numBonus;
        for (int i = 1; i <= numBonus; i++ )
        {
            if(i == 1)
            {
                StartCoroutine(delayAmpli1(anim1.GetComponent<Animator>(), DELAY_ANIM_AMPLI * i));
            }
            else if (i == 2)
            {
                StartCoroutine(delayAmpli2(anim2.GetComponent<Animator>(), DELAY_ANIM_AMPLI * i));
            }
            if (i == 3)
            {
                StartCoroutine(delayAmpli3(anim3.GetComponent<Animator>(), DELAY_ANIM_AMPLI * i));
            }
        }
    }

    IEnumerator delayAmpli1(Animator anim, float time)
    {
        yield return new WaitForSeconds(time);
        anim.Play("aparece");
    }

    IEnumerator delayAmpli2(Animator anim, float time)
    {
        yield return new WaitForSeconds(time);
        anim.Play("aparece");
    }

    IEnumerator delayAmpli3(Animator anim, float time)
    {
        yield return new WaitForSeconds(time);
        anim.Play("aparece");
    }

    void Update()
    {
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
                if (hit.collider.gameObject.tag == "btnNext")
                {
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    btnNext.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "btnAgain")
                {
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    btnAgain.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "btnMenu")
                {
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    btnMenu.GetComponent<Animator>().Play("click");
                }
                if (hit.collider.gameObject.tag == "btnGo")
                {
                    lastClickedTag = hit.collider.gameObject.tag;
                    click = true;
                    tempoClick = MAX_TIME_CLICK;
                    btnGo.GetComponent<Animator>().Play("click");
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

        if (tag == "btnNext")
        {
            click = false;
            btnNext.GetComponent<Animator>().Play("clickUp");
            if (!isCancel) StartCoroutine(delayClick(tag));
        }
        if (tag == "btnAgain")
        {
            click = false;
            btnAgain.GetComponent<Animator>().Play("clickUp");
            if (!isCancel) StartCoroutine(delayClick(tag));
        }
        if (tag == "btnMenu")
        {
            click = false;
            btnMenu.GetComponent<Animator>().Play("clickUp");
            if (!isCancel) StartCoroutine(delayClick(tag));
        }
        if (tag == "btnGo")
        {
            click = false;
            btnGo.GetComponent<Animator>().Play("clickUp");
            if (!isCancel) StartCoroutine(delayClick(tag));
        }
    }

    IEnumerator delayClick(string tag)
    {
        yield return new WaitForSeconds(DELAY_CLICK);
        congratz.SetActive(false);
        Current.isControll = false;
        if (tag == "btnNext")
        {
            Camera.main.GetComponent<StageSwitch>().next();
        }
        if (tag == "btnAgain")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (tag == "btnMenu")
        {
            Current.stageNumber = 1;
            SceneManager.LoadScene(Current.sceneName);
        }
        if (tag == "btnGo")
        {
            Current.stageNumber = 1;
            Current.sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(goTo);
        }

    }
}

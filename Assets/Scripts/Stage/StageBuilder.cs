using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;

public class StageBuilder : MonoBehaviour
{
    public GameObject base_init;
    public GameObject base_end;
    public GameObject[] basesMoveis;
    private GameObject[] basesMoveisSuffle;

    public Tilemap tilemapActive;
    public GameObject parent;
    public Dictionary<string, TileBean> mapaAtivo = new Dictionary<string, TileBean>();

    public GameObject bmEsconde;
    public GameObject sEsconde;
    public Transform parentGeradas;
    public Transform parentMapa;
    public GameObject btnGeraPontos;


    private List<Vector2> listPosition = new List<Vector2>();

    private Vector2 posN1 = new Vector2(-10, -10);
    private Vector2 posN2 = new Vector2(-10, -10);
    private Vector2 posN3 = new Vector2(-10, -10);
    private Vector2 posN4 = new Vector2(-10, -10);
    private Vector2 posN5 = new Vector2(-10, -10);
    private Vector2 posN6 = new Vector2(-10, -10);
    private Vector2 posN7 = new Vector2(-10, -10);

    private Swipe deP1 = Swipe.None;
    private Swipe deP2 = Swipe.None;
    private Swipe deP3 = Swipe.None;
    private Swipe deP4 = Swipe.None;
    private Swipe deP5 = Swipe.None;
    private Swipe deP6 = Swipe.None;
    private Swipe deP7 = Swipe.None;
    private Swipe deP8 = Swipe.None;

    private string color = "";

    public int numMinBases = 0;

    private bool achou = false;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "geraPontos")
                {

                    geraPontos();
                    btnGeraPontos.GetComponent<Animator>().Play("clickDownUp");
                }

            }
        }
    }

    void Start()
    {
        bmEsconde.SetActive(false);
        sEsconde.SetActive(false);
        basesMoveisSuffle = shuffleGO(basesMoveis);

        bool achouFirst = false;
        Vector2 posFirst = new Vector2(0, 0);
        foreach (Vector3Int localPlace in tilemapActive.cellBounds.allPositionsWithin)
        {
            if (tilemapActive.HasTile(localPlace))
            {
                Vector3 pos = tilemapActive.CellToWorld(localPlace);
                GameObject obj = new GameObject();
                obj.tag = "baseActive";
                obj.transform.SetParent(parentMapa);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.transform.position = pos;
                CircleCollider2D cc = obj.AddComponent<CircleCollider2D>();
                cc.radius = 0.4f;
                cc.isTrigger = true;
                string key = getKey(obj.transform);
                mapaAtivo.Add(key, new TileBean(obj, TileBean.TILE_ACTIVE));
                if (!achouFirst)
                {
                    achouFirst = true;
                    posFirst = obj.transform.position;
                }
                float tempDist = Vector2.Distance(posFirst, obj.transform.position);
                if (tempDist < Current.minDist && tempDist > 0.1f)
                {
                    Current.minDist = tempDist;
                }

            }
        }
        // Debug.Log(mapaAtivo.Count);

        

    }
   
    private void geraPontos()
    {
        listPosition = new List<Vector2>();

        posN1 = new Vector2(-10, -10);
        posN2 = new Vector2(-10, -10);
        posN3 = new Vector2(-10, -10);
        posN4 = new Vector2(-10, -10);
        posN5 = new Vector2(-10, -10);
        posN6 = new Vector2(-10, -10);
        posN7 = new Vector2(-10, -10);

        deP1 = Swipe.None;
        deP2 = Swipe.None;
        deP3 = Swipe.None;
        deP4 = Swipe.None;
        deP5 = Swipe.None;
        deP6 = Swipe.None;
        deP7 = Swipe.None;
        deP8 = Swipe.None;
        color = "";
        

        achou = false;

        GameObject[] sonarsDestroy = GameObject.FindGameObjectsWithTag("sonarDestroy");
        foreach (GameObject sonarDestroy in sonarsDestroy)
        {
            Destroy(sonarDestroy);
        }
        basesMoveisSuffle = shuffleGO(basesMoveis);

        List<Swipe> dirFree0 = new List<Swipe>();
        if (base_init.GetComponent<BaseFixaController>().ru != "n") dirFree0.Add(Swipe.UpRight);
        if (base_init.GetComponent<BaseFixaController>().r != "n") dirFree0.Add(Swipe.Right);
        if (base_init.GetComponent<BaseFixaController>().rd != "n") dirFree0.Add(Swipe.DownRight);
        if (base_init.GetComponent<BaseFixaController>().ld != "n") dirFree0.Add(Swipe.DownLeft);
        if (base_init.GetComponent<BaseFixaController>().l != "n") dirFree0.Add(Swipe.Left);
        if (base_init.GetComponent<BaseFixaController>().lu != "n") dirFree0.Add(Swipe.UpLeft);
        dirFree0 = shuffleSwipe(dirFree0);
        
        Vector2 pl0 = base_init.transform.position;

        foreach (Swipe dl0 in dirFree0)
        {
            color = getColor(dl0);
            foreach (Vector2 pl1 in shuffleVector(getNextPos(dl0, pl0, 0)))
            {
                if (achou) break;
                deP1 = dl0;
                posN1 = pl1;
                foreach (Swipe dl1 in getListDir(dl0))
                {
                    foreach (Vector2 pl2 in shuffleVector(getNextPos(dl1, pl1, 1)))
                    { 
                        if (achou || numMinBases < 1) break;
                        deP2 = dl1;
                        posN2 = pl2;
                        foreach (Swipe dl2 in getListDir(dl1))
                        {
                            foreach (Vector2 pl3 in shuffleVector(getNextPos(dl2, pl2, 2)))
                            {
                                if (achou || numMinBases < 2) break;
                                deP3 = dl2;
                                posN3 = pl3;
                                foreach (Swipe dl3 in getListDir(dl2))
                                {
                                    foreach (Vector2 pl4 in shuffleVector(getNextPos(dl3, pl3, 3)))
                                    {
                                        if (achou || numMinBases < 3) break;
                                        deP4 = dl3;
                                        posN4 = pl4;
                                        foreach (Swipe dl4 in getListDir(dl3))
                                        {
                                            foreach (Vector2 pl5 in shuffleVector(getNextPos(dl4, pl4, 4)))
                                            {
                                                if (achou || numMinBases < 4) break;
                                                deP5 = dl4;
                                                posN5 = pl5;
                                                foreach (Swipe dl5 in getListDir(dl4))
                                                {
                                                    foreach (Vector2 pl6 in shuffleVector(getNextPos(dl5, pl5, 5)))
                                                    {
                                                        if (achou || numMinBases < 5) break;
                                                        deP6 = dl5;
                                                        posN6 = pl6;
                                                        foreach (Swipe dl6 in getListDir(dl5))
                                                        {
                                                            foreach (Vector2 pl7 in shuffleVector(getNextPos(dl6, pl6, 6)))
                                                            {
                                                                if (achou || numMinBases < 6) break;
                                                                deP7 = dl6;
                                                                posN7 = pl7;
                                                                foreach (Swipe dl7 in getListDir(dl6))
                                                                {
                                                                    foreach (Vector2 pl8 in shuffleVector(getNextPos(dl7, pl7, 7)))
                                                                    {
                                                                        if (achou || numMinBases < 7) break;
                                                                        deP8 = dl7;

                                                                        //continuar aqui
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            List<PosBean> list = new List<PosBean>();
            if (numMinBases > 1)
            {
                list.Add(new PosBean(posN1, getDirInvertido(deP1), (numMinBases == 1 ? dirEnd : deP2)));
                list.Add(new PosBean(posN2, getDirInvertido(deP2), (numMinBases == 2 ? dirEnd : deP3)));
            }
            if (numMinBases > 2)
            {
                list.Add(new PosBean(posN3, getDirInvertido(deP3), (numMinBases == 3 ? dirEnd : deP4)));
            }
            if (numMinBases > 3)
            {
                list.Add(new PosBean(posN4, getDirInvertido(deP4), (numMinBases == 4 ? dirEnd : deP5)));
            }
            if (numMinBases > 4)
            {
                list.Add(new PosBean(posN5, getDirInvertido(deP5), (numMinBases == 5 ? dirEnd : deP6)));
            }
            if (numMinBases > 5)
            {
                list.Add(new PosBean(posN6, getDirInvertido(deP6), (numMinBases == 6 ? dirEnd : deP7)));
            }
            if (numMinBases > 6)
            {
                list.Add(new PosBean(posN7, getDirInvertido(deP7), (numMinBases == 7 ? dirEnd : deP8)));
            }
            if (!achou)
            {
                Debug.Log("Não achou " + color);
            }
            else
            {
                if (geraRotacao(list)) break;
            }

            Debug.Log("#####################################################################");
        }
   
    }

    private bool geraRotacao(List<PosBean> posBeans)
    {
        int numPos = posBeans.Count;
        foreach (PosBean pb in posBeans)
        {
            basesMoveisSuffle = shuffleGO(basesMoveis);
            foreach (GameObject bm in basesMoveisSuffle)
            {
                BaseMovelController baseMovelController = Instantiate(bm.GetComponent<BaseMovelController>());
                bool achouPosRotate = false;
                int rot = 0;
                for (int i = 0; i < 6; i++)
                {
                    if (encaixa(pb, baseMovelController))
                    {
                        numPos--;
                        achouPosRotate = true;
                        Debug.Log(bm.name + " - " + rot);
                        addCopy(bm, pb.getPos(), rot);
                        break;
                    }
                    else
                    {
                        rot -= 60;
                        mudaLabels(baseMovelController);
                    }
                }
                if (achouPosRotate)  break;
            }
        }
        return numPos == 0;
    }

    private void addCopy(GameObject bm, Vector2 pos, int rotation)
    {

        GameObject copyBm = Instantiate(bm);
        Destroy(copyBm.GetComponent<Drag>());
        Destroy(copyBm.GetComponent<CircleCollider2D>());
        copyBm.transform.GetChild(0).gameObject.SetActive(false);
        Destroy(copyBm.transform.GetChild(1).gameObject.GetComponent<ColliderController>());
        copyBm.transform.GetChild(2).gameObject.SetActive(false);
        copyBm.tag = "sonarDestroy";
        copyBm.transform.position = pos;
        copyBm.transform.SetParent(parentGeradas);
        copyBm.transform.localScale = bm.transform.localScale;

        if (rotation != 0) copyBm.transform.eulerAngles = Vector3.forward * rotation;
    }

    private bool encaixa(PosBean pb, BaseMovelController bmc)
    {
        string corDe = "";
        string corPara = "";
        if (pb.getDe() == Swipe.Right) corDe = bmc.r;
        if (pb.getDe() == Swipe.Left) corDe = bmc.l;
        if (pb.getDe() == Swipe.UpRight) corDe = bmc.ru;
        if (pb.getDe() == Swipe.UpLeft) corDe = bmc.lu;
        if (pb.getDe() == Swipe.DownRight) corDe = bmc.rd;
        if (pb.getDe() == Swipe.DownLeft) corDe = bmc.ld;

        if (pb.getPara() == Swipe.Right) corPara = bmc.r;
        if (pb.getPara() == Swipe.Left) corPara = bmc.l;
        if (pb.getPara() == Swipe.UpRight) corPara = bmc.ru;
        if (pb.getPara() == Swipe.UpLeft) corPara = bmc.lu;
        if (pb.getPara() == Swipe.DownRight) corPara = bmc.rd;
        if (pb.getPara() == Swipe.DownLeft) corPara = bmc.ld;

        return corDe == color && corPara == color;
    }

    private void mudaLabels(BaseMovelController bm)
    {
        string aux_ru = bm.ru;
        bm.ru = bm.lu;
        bm.lu = bm.l;
        bm.l = bm.ld;
        bm.ld = bm.rd;
        bm.rd = bm.r;
        bm.r = aux_ru;
    }

    private string getColor(Swipe dir)
    {
        string c = "";
        if (dir == Swipe.UpRight) c = base_init.GetComponent<BaseFixaController>().ru;
        if (dir == Swipe.Right) c = base_init.GetComponent<BaseFixaController>().r;
        if (dir == Swipe.DownRight) c = base_init.GetComponent<BaseFixaController>().rd;
        if (dir == Swipe.DownLeft) c = base_init.GetComponent<BaseFixaController>().ld;
        if (dir == Swipe.Left) c = base_init.GetComponent<BaseFixaController>().l;
        if (dir == Swipe.UpLeft) c = base_init.GetComponent<BaseFixaController>().lu;
        return c;
    }

    private Swipe getDirInvertido(Swipe dir)
    {
        Swipe c = Swipe.None;
        if (dir == Swipe.UpRight) c = Swipe.DownLeft;
        if (dir == Swipe.Right) c = Swipe.Left;
        if (dir == Swipe.DownRight) c = Swipe.UpLeft;
        if (dir == Swipe.DownLeft) c = Swipe.UpRight;
        if (dir == Swipe.Left) c = Swipe.Right;
        if (dir == Swipe.UpLeft) c = Swipe.DownRight;
        return c;
    }

    private string getColorInvertido(Swipe dir)
    {
        string c = "";
        if (dir == Swipe.UpRight) c = base_end.GetComponent<BaseFixaController>().ld;
        if (dir == Swipe.Right) c = base_end.GetComponent<BaseFixaController>().l;
        if (dir == Swipe.DownRight) c = base_end.GetComponent<BaseFixaController>().lu;
        if (dir == Swipe.DownLeft) c = base_end.GetComponent<BaseFixaController>().ru;
        if (dir == Swipe.Left) c = base_end.GetComponent<BaseFixaController>().r;
        if (dir == Swipe.UpLeft) c = base_end.GetComponent<BaseFixaController>().rd;
        return c;
    }

    private List<Swipe> getListDir(Swipe dirAtual)
    {
        List<Swipe> list = new List<Swipe>();
        if (dirAtual == Swipe.UpRight) {
            list.Add(Swipe.Right);
            list.Add(Swipe.Left);
            list.Add(Swipe.UpLeft);
            list.Add(Swipe.DownRight);
        }
        if (dirAtual == Swipe.Right) {
            list.Add(Swipe.UpRight);
            list.Add(Swipe.UpLeft);
            list.Add(Swipe.DownLeft);
            list.Add(Swipe.DownRight);
        }
        if (dirAtual == Swipe.DownRight) {
            list.Add(Swipe.UpRight);
            list.Add(Swipe.Right);
            list.Add(Swipe.Left);
            list.Add(Swipe.DownLeft);
        }
        if (dirAtual == Swipe.DownLeft) {
            list.Add(Swipe.Left);
            list.Add(Swipe.UpLeft);
            list.Add(Swipe.Right);
            list.Add(Swipe.DownRight);
        }
        if (dirAtual == Swipe.Left) {
            list.Add(Swipe.UpRight);
            list.Add(Swipe.UpLeft);
            list.Add(Swipe.DownLeft);
            list.Add(Swipe.DownRight);
        }
        if (dirAtual == Swipe.UpLeft) {
            list.Add(Swipe.UpRight);
            list.Add(Swipe.Right);
            list.Add(Swipe.DownLeft);
            list.Add(Swipe.Left);
        }

        return shuffleSwipe(list);
    }

    public string getKey(Transform trans)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(trans.position, Vector2.zero);
        Vector2 posHit = new Vector2(0, 0);
        bool temHit = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "baseActive")//baseInner
            {
                posHit = hit.collider.gameObject.transform.position;
                temHit = true;
                break;
            }
        }
        if (temHit)
        {
            return Round(Camera.main.WorldToScreenPoint(posHit));
        }
        else
        {
            return Round(Camera.main.WorldToScreenPoint(trans.position));
        }

    }

    private Swipe dirEnd = Swipe.None;
    private List<Vector2> getNextPos(Swipe dir, Vector2 posAtual, int deepeth)
    {
        List<Vector2> free = new List<Vector2>();
        Vector2 d = posAtual;
        int max = 12;
        while (max > 0)
        {
            max--;
            bool isBaseActive = false;
            bool isBaseEnd = false;
            if (dir == Swipe.UpRight) d = new Vector2(d.x + (Current.minDist / 2), d.y + Current.minDist);
            if (dir == Swipe.UpLeft) d = new Vector2(d.x - (Current.minDist / 2), d.y + Current.minDist);
            if (dir == Swipe.Left) d = new Vector2(d.x - Current.minDist, d.y);
            if (dir == Swipe.Right) d = new Vector2(d.x + Current.minDist, d.y);
            if (dir == Swipe.DownRight) d = new Vector2(d.x + (Current.minDist / 2), d.y - Current.minDist);
            if (dir == Swipe.DownLeft) d = new Vector2(d.x - (Current.minDist / 2), d.y - Current.minDist);
 
            string colorEnd = "";
            RaycastHit2D[] hits = Physics2D.RaycastAll(d, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "baseActive")
                {
                    d = hit.collider.gameObject.transform.position;
                    isBaseActive = true;
                }
                if (hit.collider.gameObject.tag == "baseInner") return free;
                if (hit.collider.gameObject.tag == "baseInit") return free;
                if (hit.collider.gameObject.tag == "baseEnd")
                {
                    colorEnd = getColorInvertido(dir);
                    dirEnd = dir;
                    isBaseEnd = true;
                }

            }
           
            if (deepeth == 2 && Vector2.Distance(d, posN1) < 0.1)  return free; 
            if (deepeth == 3 && (Vector2.Distance(d, posN1) < 0.1 || Vector2.Distance(d, posN2) < 0.1)) return free;
            if (deepeth == 4 && (Vector2.Distance(d, posN1) < 0.1 || Vector2.Distance(d, posN2) < 0.1 || Vector2.Distance(d, posN3) < 0.1))  return free;
            if (deepeth == 5 && (Vector2.Distance(d, posN1) < 0.1 || Vector2.Distance(d, posN2) < 0.1 || Vector2.Distance(d, posN3) < 0.1 || Vector2.Distance(d, posN4) < 0.1)) return free;
            if (deepeth == 6 && (Vector2.Distance(d, posN1) < 0.1 || Vector2.Distance(d, posN2) < 0.1 || Vector2.Distance(d, posN3) < 0.1 || Vector2.Distance(d, posN4) < 0.1 || Vector2.Distance(d, posN5) < 0.1)) return free;
            if (deepeth == 7 && (Vector2.Distance(d, posN1) < 0.1 || Vector2.Distance(d, posN2) < 0.1 || Vector2.Distance(d, posN3) < 0.1 || Vector2.Distance(d, posN4) < 0.1 || Vector2.Distance(d, posN5) < 0.1 || Vector2.Distance(d, posN6) < 0.1)) return free;

            if (isBaseEnd)
            {
                if (numMinBases == deepeth && color == colorEnd)
                {
                    achou = true;
                }
                else
                {
                    return free;
                }
            }
            if (!isBaseActive) return free;
            free.Add(d);
        }
        return free;
    }

    private string Round(Vector2 vector)
    {
        float dist1 = Mathf.Round(vector.x / 10);
        float dist2 = Mathf.Round(vector.y / 10);
        return dist1 + "/" + dist2;
    }

    public GameObject[] shuffleGO(GameObject[] list)
    {
        System.Random rng = new System.Random();
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    public List<Swipe> shuffleSwipe(List<Swipe> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Swipe value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    public List<Vector2> shuffleVector(List<Vector2> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Vector2 value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    public int[] shuffleInt(int[] list)
    {
        System.Random rng = new System.Random();
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    public string getKey(Vector2 trans)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(trans, Vector2.zero);
        Vector2 posHit = new Vector2(0, 0);
        bool temHit = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "baseActive")
            {
                posHit = hit.collider.gameObject.transform.position;
                temHit = true;
                break;
            }

        }
        if (temHit)
        {
            return Round(Camera.main.WorldToScreenPoint(posHit));
        }
        else
        {
            return Round(Camera.main.WorldToScreenPoint(trans));
        }

    }

    public class PosBean
    {
        Vector2 pos;
        Swipe de;
        Swipe para;

        public PosBean(Vector2 ps, Swipe d, Swipe p)
        {
            pos = ps;
            de = d;
            para = p;
        }

        public Vector2 getPos()
        {
            return pos;
        }
        public Swipe getDe()
        {
            return de;
        }
        public Swipe getPara()
        {
            return para;
        }
    }
}

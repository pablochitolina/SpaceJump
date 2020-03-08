using UnityEngine;

public class ActiveSonarBean
{
    private Vector2 posSonar;
    private string color;
    private GameObject baseOrigem;
    private Quaternion rotation;
    // Start is called before the first frame update
    public ActiveSonarBean(Vector2 newPosSonar, string newColor, Quaternion r)
    {
        this.posSonar = newPosSonar;
        this.color = newColor;
        this.rotation = r;
    }

    public void setBaseOrigem(GameObject bo)
    {
        baseOrigem = bo;
    }

    public GameObject getBaseOrigem()
    {
        return baseOrigem;
    }

    public Vector2 getPosSonar()
    {
        return this.posSonar;
    }

    public string getColor()
    {
        return this.color;
    }

    public Quaternion getRotation()
    {
        return this.rotation;
    }
}

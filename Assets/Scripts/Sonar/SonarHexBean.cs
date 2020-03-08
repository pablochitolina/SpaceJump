using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarHexBean
{
    public const string RED = "redSonar";
    public const string GREEN = "greenSonar";
    public const string BLUE = "blueSonar";
    private bool anima;
    private bool hit;
    private ActiveSonarBean sonar;
    

    public SonarHexBean(bool newAnima, bool newHit, ActiveSonarBean newSonar)
    {
        this.anima = newAnima;
        this.hit = newHit;
        this.sonar = newSonar;
    }

    public bool isAnima()
    {
        return this.anima;
    }

    public bool isHit()
    {
        return this.hit;
    }

    public ActiveSonarBean getSonar()
    {
        return this.sonar;
    }
}

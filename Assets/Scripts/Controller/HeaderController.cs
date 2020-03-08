using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderController : MonoBehaviour
{

    public TextMesh txtStage;
    void Start()
    {
        string text = "";
        if (Current.stageNumber < 10)
        {
            text = "STAGE 0" + Current.stageNumber;
        }
        else
        {
            text = "STAGE " + Current.stageNumber;
        }
        txtStage.text = text;
    }

}

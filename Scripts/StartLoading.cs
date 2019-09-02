using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLoading : MonoBehaviour
{
    void Update()
    {
        LoaderManager.callTargetScene();
    }
}

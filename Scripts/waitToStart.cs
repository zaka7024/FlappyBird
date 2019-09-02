using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitToStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        show();
        BirdController.getInstance().onBirdStart += hide;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void hide()
    {
        gameObject.SetActive(false);
    }
    
    void show()
    {
        gameObject.SetActive(true);
    }
}

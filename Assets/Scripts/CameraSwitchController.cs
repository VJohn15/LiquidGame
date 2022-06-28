using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchController : MonoBehaviour
{
    public static CameraSwitchController instance = null;

    public Animator anim;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

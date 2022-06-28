using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    void Start()
    {
        
    }

    void Update()
    {
        if (UiManager.instance.oneTimePress)
        {
            anim.SetBool("Look", true);
        }
        else
        {
            anim.SetBool("Look", false);
        }
        
    }

    
    
    public void OpponentMovement()
    {

    }
}

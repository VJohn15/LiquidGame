using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondGlassController : MonoBehaviour
{
    public static SecondGlassController instance = null;

    
    [SerializeField] private GameObject point1;
    [SerializeField] private GameObject point2;
    [SerializeField] private ParticleSystem water;
    
    
    public float time = 1f;
    
    [HideInInspector] public float timer;
    [HideInInspector] public bool oneTime;
    [HideInInspector] public bool checkLoosing;

    
    
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
    
    private void Start()
    {
        timer = time;
        oneTime = true;
        checkLoosing = true;

    }

    void Update()
    {
        if (UiManager.instance.checkSecondGlass && UiManager.instance.start && time >= 0 && checkLoosing)
        {
            GetSecondPosition();
        
            Invoke("WaterFall", 0.4f);
        }
        
        
        if (time <= 0)
        {
            water.enableEmission = false;
            RotateBucketController.instance.emitter.speed = 0;
            UiManager.instance.checkSecondGlass = false;
            Invoke("NextGlass", 1.5f);
            FirstGlassController.instance.checkLoosing = true;
        
            Invoke("GetFirstPosition", 0.7f);
        
        }
        
        if (RotateBucketController.instance.lv.level >= RotateBucketController.instance.liquidLevel && !checkLoosing)
        {
            UiManager.instance.start = false;
            UiManager.instance.winCameraMov = false;
            CameraSwitchController.instance.anim.SetTrigger("SecondPos");
            RotateBucketController.instance.SetPlayerFirstPos();
        
            Debug.Log("win second");
        
            // Invoke("Win", 1f);
        }
    }

    public void Win()
    {
        RotateBucketController.instance.ActivateWinPanel();
    }
    
    public void NextGlass()
    {

        if (oneTime)
        {
            checkLoosing = false;

            UiManager.instance.StartControll();
            UiManager.instance.getMainFirstPos = true;
            FirstGlassController.instance.time = FirstGlassController.instance.timer;

            CameraSwitchController.instance.anim.SetTrigger("FirstPos");
            oneTime = false;
        }
        
    }
    
    
    public void WaterFall()
    {
        time -= Time.deltaTime;
        RotateBucketController.instance.emitter.speed = RotateBucketController.instance.emissionSpeed;
        water.enableEmission = true;
        RotateBucketController.instance.waterDrop.Play();

    }
    
    public void GetFirstPosition()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, 
            point1.transform.position, Time.deltaTime * 3f);
            
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
            point1.transform.rotation , Time.deltaTime * 5f);
    }

    public void GetSecondPosition()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, 
            point2.transform.position, Time.deltaTime * 3f);
            
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
            point2.transform.rotation , Time.deltaTime * 5f);
    }
}

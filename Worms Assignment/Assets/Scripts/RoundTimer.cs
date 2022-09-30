using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour
{
    [SerializeField] float timeDuration = 10f;
    [SerializeField] Text displayText;

    private float timer;

    public bool runTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        runTimer = false;
        //ResetTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!runTimer) return;
        if(timer>0){
            timer -= Time.fixedDeltaTime;
            SetTimerDisplay(timer);
        }
        else{
            timer = 0;
            SetTimerDisplay(timer);
        }
    }

    public void ResetTimer(){
        timer = timeDuration;
    }

    public void SetTimerDisplay(float time){
        float seconds = Mathf.FloorToInt(time);
        displayText.text = time.ToString("00.00");
    }

    public float GetTimer(){
        return timer;
    }

    public void StartTimer(){
        runTimer = true;
        ResetTimer();
    }

    public void StopTimer(){
        timer = 0;
        SetTimerDisplay(timer);
        runTimer = false;
    }
}

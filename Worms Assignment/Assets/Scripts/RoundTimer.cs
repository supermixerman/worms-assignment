using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour
{
    [SerializeField] float timeDuration = 10f;
    [SerializeField] Text displayText;

    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>0){
            timer -= Time.deltaTime;
            SetTimerDisplay(timer);
        }
        else{
            timer = 0;
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

    public void StopTimer(){
        timer = 0;
    }
}

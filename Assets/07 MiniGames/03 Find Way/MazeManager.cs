using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private float countTime = 10f;
    [SerializeField] private Slider timerSlider;

    bool start = false;
    bool finish = false;
    public bool isStart { get { return start; } }
    float currentTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            currentTime += Time.deltaTime;
            timerSlider.value = (countTime - currentTime) / countTime;
            if (timerSlider.value <= 0 && !finish)
            {
                Debug.Log("Game over");
            }
        }
    }

    public void StartMaze()
    {
        start = true;
        currentTime = 0f;
    }

    public void FinishMaze()
    {
        start = false;
        finish = true;
        Debug.Log("Game Clear");
        StageManager.Instance.ChangeStage(3);
    }
}

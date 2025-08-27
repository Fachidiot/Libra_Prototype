using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static StageManager instance;
    public static StageManager Instance{get{return instance;}}
    [Header("Stage Settings")]
    [SerializeField] private GameObject[] stages;
    [SerializeField] private int currentStage;

    private void Awake()
    {
        if (null != instance)
            Destroy(this);
        
        instance = this;
    }

    void Start()
    {
        InitStage();
        stages[currentStage].SetActive(true);
    }

    void InitStage()
    {
        foreach (var stage in stages)
        {
            stage.SetActive(false);
        }
    }

    public void ChangeStage(int index)
    {
        stages[currentStage].SetActive(false);
        stages[index].SetActive(true);
        currentStage = index;
    }
}

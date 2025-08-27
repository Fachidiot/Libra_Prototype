using UnityEngine;

public class CharacterMaker : MonoBehaviour
{
    [SerializeField] private GameObject startButton;

    public void ChooseSexual(bool male)
    {
        if (!startButton.activeSelf)
            startButton.SetActive(true);

        if (male)
        {
            Debug.Log("남성");
        }
        else
        {
            Debug.Log("여성");
        }
    }
}

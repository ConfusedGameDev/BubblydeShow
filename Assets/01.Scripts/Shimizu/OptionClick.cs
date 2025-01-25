using UnityEngine;

public class OptionClick : MonoBehaviour
{
    public GameObject Option;
    public void Open()
    {
        Option.SetActive(true);
    }
    public void Close()
    {
        Option.SetActive(false);
    }
}

using UnityEngine;

public class Bubble : MonoBehaviour
{
    /// <summary>
    /// Called to map an integer value to this bubble’s visual representation.
    /// </summary>
    /// 

    public string mondai = "Which is bigger?!";

    public virtual void mapBubble(int x)
    {
        
        // Child classes can override this with  visuals.
        Debug.Log($"Bubble mapped to value {x}");
    }
}
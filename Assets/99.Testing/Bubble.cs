using UnityEngine;

public class Bubble : MonoBehaviour
{
    /// <summary>
    /// Called to map an integer value to this bubble’s visual representation.
    /// </summary>
    /// 

    public string mondai = "Which is bigger?!";
    public Transform parentObj;
    public Vector3 parentOffset;

    public void upScaleParent(float scale)
    {
        if (parentObj != null)
        {
            parentObj.localScale = Vector3.one *scale;
        }
    }
    public void offsetParent(Vector3 newOffset)
    {
        if (parentObj != null)
        {
            parentOffset = newOffset;
        }
    }
    public virtual void mapBubble(int x)
    {
        
        // Child classes can override this with  visuals.
        Debug.Log($"Bubble mapped to value {x}");
    }
}
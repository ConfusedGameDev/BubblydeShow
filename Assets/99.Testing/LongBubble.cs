using UnityEngine;

public class LongBubble : Control_CatScaler_Linear
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public override void mapBubble(int x)
    {
        longness = x / scale;
    }
}

using UnityEngine;

public class FatBubble : Control_CatScaler_Linear
{
 
    public override void mapBubble(int x)
    {
        fatness = x / scale;
    }
}

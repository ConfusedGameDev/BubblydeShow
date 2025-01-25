using UnityEngine;

public class TallBubble : Control_CatScaler_Linear
{ 
    public override void mapBubble(int x)
    {
        tallness = x / scale;
    }
}

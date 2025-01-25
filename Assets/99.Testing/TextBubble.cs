using TMPro;
using UnityEngine;

public class fatBubble : Bubble
{
    public TMPro.TextMeshProUGUI label;
    public override void mapBubble(int x)
    {
  
        if (label != null)
        {
            label.text= x.ToString();
        }

     }


}

using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BackDanceCatsMovement : MonoBehaviour
{
    public float walkSpeed;
    public float endPosX;
    private Vector3 startPos;
    private float startPosX;
    private float PosX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = this.transform.position ;
        PosX = startPos.x;
        startPosX = startPos.x;
    }
    // Update is called once per frame
    void Update()
    {
        if (walkSpeed > 0)
        {
            PosX += walkSpeed * Time.deltaTime;

            this.transform.position = new Vector3(PosX,startPos.y, startPos.z);
        }

        if(PosX >= endPosX)
        {
            PosX = startPosX;
            
            this.transform.position = new Vector3(PosX, startPos.y, startPos.z);
        }
        //ïâÇØÇΩÇÁè¡Ç¶ÇÈÇÊÇ§Ç…(ñ¢é¿ëï)

    }
}

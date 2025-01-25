using UnityEngine;


public class CatRigController : MonoBehaviour
{
    public Transform bottomBone;
    public Vector3 initialPos;
    public Vector3 rootOffset;
     [Range(0,100)]
    public float offset;
    public Transform rootBone;

    public Transform parentObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
     }

    // Update is called once per frame
    void Update()
    {
        rootBone.position = initialPos+rootOffset;
        bottomBone.position = rootBone.position - offset * Vector3.up;

    }
}

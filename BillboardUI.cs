//Using statement for Unity
using UnityEngine;

//Inherit from MonoBehaviour class
public class BillboardUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Set same rotation for health bars as camera
        transform.rotation = Camera.main.transform.rotation;
    }
}

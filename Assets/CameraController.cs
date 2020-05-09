using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    float horizontal;
float vertical ;
    void Start()
    {
        
    }

    float speed = 5.0f;
    
    void Update(){
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Transform transform = GetComponent<Transform>();
        transform.Translate( new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime);
    }

}

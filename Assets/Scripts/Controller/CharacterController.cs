using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public bool isGetFront = false;
    // Update is called once per frame
    void Update()
    {
        if (!isGetFront)
        {
            ArrayFrontManager.Instance.CreatArayFront();
            isGetFront = true;
        }
        
    }
}

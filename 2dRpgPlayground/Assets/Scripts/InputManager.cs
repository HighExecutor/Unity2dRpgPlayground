using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnSubmit()
    {
        Debug.Log("OnSubmit");
        DialogueManager.GetInstance().SetNext(true);
    }
}

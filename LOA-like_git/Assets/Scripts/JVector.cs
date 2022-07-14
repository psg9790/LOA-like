using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class JVector3
{
    public float x;
    public float y;
    public float z;

    public JVector3(Vector3 _v3)
    {
        x = _v3.x;
        y = _v3.y;
        z = _v3.z;
    }

    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }
}

// public class JVector : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }

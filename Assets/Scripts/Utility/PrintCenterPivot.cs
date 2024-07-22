using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

class PrintCenterPivot: MonoBehaviour
{
    void Awake()
    {
        var center = GetComponentInChildren<MeshRenderer>().bounds.center;
        Debug.Log(center.x.ToString() + "," + center.y.ToString() + "," + center.z.ToString());
    }
}
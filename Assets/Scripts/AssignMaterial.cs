using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class AssignMaterial : MonoBehaviour
{

    public Material[] materials;

    // Start is called before the first frame update
    
    void Start()
    {
        int i = 0;

        foreach(Transform child in transform)
        {
           MeshRenderer mr;
           if (child.TryGetComponent<MeshRenderer>(out mr))
           {
                mr.material = materials[i];
                i++;
                
                if (i >= materials.Length)
                    i = 0;
           }
        }
    
    }

    
}

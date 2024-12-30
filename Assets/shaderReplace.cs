using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shaderReplace : MonoBehaviour
{public Shader Before;public Shader After;

List<Material> shadermats=new List<Material>();
public string propatyName;

[Range(-1,1)]public float slider;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (var mats in item.materials)
            {
                if (mats.shader==Before)
                {
                    mats.shader=After;
                    shadermats.Add(mats);
                }
            }
     
        }
    }
    private void Update() {
        foreach (var item in shadermats)
        {
            item.SetFloat(propatyName,slider);
        }
    }

  
}

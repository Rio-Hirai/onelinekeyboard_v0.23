using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input : MonoBehaviour
{

    private GameObject _parent;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<TextMesh>().text = this.GetComponentInParent<wordestimation>().input_sentence;
    }
}

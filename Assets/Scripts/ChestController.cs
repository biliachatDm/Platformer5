using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ChestController : MonoBehaviour
{
    public Transform target;
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(target.position, duration, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

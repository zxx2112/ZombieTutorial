using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int numberToSpawn;
    public GameObject objectToSpawn;
    public float rangeRadius = 5f;
    
    void Start()
    {
        var trans = transform;
        for(int n=0; n<numberToSpawn; n++)
        {
            var offset = Random.insideUnitCircle * rangeRadius;
            var pos = new Vector3(offset.x,0,offset.y) + trans.position;
            Instantiate(objectToSpawn, pos, Quaternion.identity, trans);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int numberToSpawn;
    public GameObject objectToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        for(int n=0; n<numberToSpawn; n++){
            Instantiate(objectToSpawn, transform.position, Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
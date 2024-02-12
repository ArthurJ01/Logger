using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLogic : MonoBehaviour
{

    [SerializeField] private GameObject logPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void spawnLog()
    {
        Vector3 _treePosition = GetComponent<Transform>().position;
        Quaternion _logRotation = Quaternion.identity;
        _logRotation.eulerAngles = new Vector3(90, 0, 0);
        Instantiate(logPrefab, _treePosition, _logRotation);
        
        Destroy(gameObject);
    }
}

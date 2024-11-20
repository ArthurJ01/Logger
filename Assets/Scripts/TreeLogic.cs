using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLogic : MonoBehaviour
{

    [SerializeField] private GameObject logPrefab;
    [SerializeField] private int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void damageTree(int dmg)
    {
        health--;
        if (health <= 0)
        {
            spawnLog();
        }

        Debug.Log("tree damaged: " + dmg + ", health: " + health);
    }

    public void spawnLog()
    {
        Vector3 _treePosition = GetComponent<Transform>().position;
        Quaternion _logRotation = Quaternion.identity;
        _logRotation.eulerAngles = new Vector3(70, 0, 0);
        Instantiate(logPrefab, _treePosition, _logRotation);
        
        Destroy(gameObject);
    }
}

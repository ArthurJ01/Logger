using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class TreeCutting : MonoBehaviour
{

    [SerializeField] private LayerMask treeLayer;
    [SerializeField] private int damage;

    private bool inCuttingRange = false;
    private GameObject currentTree;


    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions(); // Create it once
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.AxeHit.performed += OnAxeHitPerformed;
    }

    private void OnDisable()
    {
        playerInputActions.Player.AxeHit.performed -= OnAxeHitPerformed;
        playerInputActions.Player.Disable();
    }

    private void OnAxeHitPerformed(InputAction.CallbackContext context)
    {
        if (inCuttingRange && currentTree != null)
        {
            currentTree.GetComponent<TreeLogic>().DamageTree(damage);
        }
    }

    private void OnAxeHitCanceled(InputAction.CallbackContext context)
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & treeLayer) != 0)
        {
            inCuttingRange = true;
            currentTree = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & treeLayer) != 0)
        {
            inCuttingRange = false;
            currentTree = null;
        }
    }
}

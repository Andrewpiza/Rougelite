using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    InputAction moveAction;
    InputAction mouseAction;
    InputAction throwAction;
    InputAction whistleAction;

    public List<List<GameObject>> mushroomSquad;
    private Type selectedType;

    [Header("Throw")]
    [SerializeField]private float throwMinDistance = 1;
    [SerializeField]private float throwMaxDistance = 8;
    private GameObject throwObject;

    [Header("Whistle")]
    [SerializeField]private float whistleMinSize = 1;
    [SerializeField]private float whistleMaxSize = 3;
    private float whistleIncreaseRate = 12;
    private float currentWhistleSize;
    private GameObject whistleObject;
    private bool isWhistling;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();

        mushroomSquad = new List<List<GameObject>>();
        for (int i = 0; i < (int)Type.COUNT; i++){
            mushroomSquad.Add(new List<GameObject>());
        }

        throwObject = transform.GetChild(0).gameObject;
        whistleObject = transform.GetChild(1).gameObject;

        currentWhistleSize = whistleMinSize;

        // Inputs
        moveAction = InputSystem.actions.FindAction("Move");
        mouseAction = InputSystem.actions.FindAction("Point");
        throwAction = InputSystem.actions.FindAction("Throw");
        whistleAction = InputSystem.actions.FindAction("Whistle");
    }

    // Update is called once per frame
    void Update()
    {
        Move(moveAction.ReadValue<Vector2>());

        Vector3 pos = Camera.main.ScreenToWorldPoint(mouseAction.ReadValue<Vector2>());
        pos += new Vector3(0,0,10);

        // Throw 
        if (Vector2.Distance(pos,transform.position) < throwMaxDistance)throwObject.transform.position =pos;
        else
        {
            throwObject.transform.position = transform.position + (pos-transform.position).normalized * throwMaxDistance;
        }

        if (throwAction.WasPressedThisFrame())
        {
            foreach (GameObject mushroom in mushroomSquad[(int)selectedType])
            {
                if (Vector2.Distance(transform.position,mushroom.transform.position) < throwMinDistance)
                {
                    // Remove Mushroom From Squad
                    // Throw Mushroom
                    break;
                }
            }
        }
        // Whistle
        whistleObject.transform.position = pos;
        if (whistleAction.WasPressedThisFrame())
        {
            isWhistling = true;
        }
        if (whistleAction.WasReleasedThisFrame())
        {
            isWhistling= false;
            currentWhistleSize = whistleMinSize;
            whistleObject.transform.localScale = new Vector3(currentWhistleSize,currentWhistleSize,1);
        }

        if (isWhistling)Whistle();

    }

    public void Whistle()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(whistleObject.transform.position,currentWhistleSize/2,LayerMask.GetMask("Mushroom"));

        foreach (Collider2D mushroom in list)
        {
            // if not in squad
            // Add mushroom to squad
        }

        if (currentWhistleSize < whistleMaxSize)
        {
            currentWhistleSize += Time.deltaTime * whistleIncreaseRate;
            if (currentWhistleSize > whistleMaxSize)currentWhistleSize = whistleMaxSize;
            whistleObject.transform.localScale = new Vector3(currentWhistleSize,currentWhistleSize,1);
        }
    }
}

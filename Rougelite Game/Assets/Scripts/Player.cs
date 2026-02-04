using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    InputAction moveAction;
    InputAction mouseAction;

    public List<List<GameObject>> mushroomSquad;
    private Type selectedType;

    [Header("Throw")]
    [SerializeField]private float throwMaxDistance = 8;
    private GameObject throwObject;

    [Header("Whistle")]
    [SerializeField]private float whistleMinSize = 1;
    [SerializeField]private float whistleMaxSize = 3;
    private float currentWhistleSize = 1;
    private GameObject whistleObject;

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
        moveAction = InputSystem.actions.FindAction("Move");
        mouseAction = InputSystem.actions.FindAction("Point");
    }

    // Update is called once per frame
    void Update()
    {
        Move(moveAction.ReadValue<Vector2>());

        Vector3 pos = Camera.main.ScreenToWorldPoint(mouseAction.ReadValue<Vector2>());
        pos += new Vector3(0,0,10);

        if (Vector2.Distance(pos,transform.position) < throwMaxDistance)throwObject.transform.position =pos;
        else
        {
            throwObject.transform.position = transform.position + (pos-transform.position).normalized * throwMaxDistance;
        }

        whistleObject.transform.position = pos;


    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    InputAction moveAction;

    public List<List<GameObject>> mushroomSquad;
    private Type selectedType;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();

        mushroomSquad = new List<List<GameObject>>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        Move(moveAction.ReadValue<Vector2>());
    }
}

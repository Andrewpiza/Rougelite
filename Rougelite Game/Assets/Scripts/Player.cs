using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    InputAction moveAction;
    InputAction mouseAction;
    InputAction throwAction;
    InputAction whistleAction;
    InputAction switchNextAction;
    InputAction switchBackAction;

    public List<List<GameObject>> mushroomSquad;
    private Type selectedType;

    [Header("Throw")]
    [SerializeField]private float throwStrength = 1000;
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
        for (int i = 0; i < (int)Type.None+1; i++){
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
        switchNextAction = InputSystem.actions.FindAction("SwitchTypeNext");
        switchBackAction = InputSystem.actions.FindAction("SwitchTypeBack");

        SwitchTypeTo(Type.None);
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
                    Mushroom m = mushroom.GetComponent<Mushroom>();
                    RemoveMushroomFromSquad(m);
                    ThrowMushroom(m);
                    
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

        // Switch
        if (switchNextAction.WasPressedThisFrame())
        {
            selectedType++;
            if (selectedType >= Type.None)selectedType = 0;
            SwitchTypeTo(selectedType);
        }
        if (switchBackAction.WasPressedThisFrame())
        {
            selectedType--;
            if (selectedType < 0)selectedType = Type.None-1;
            SwitchTypeTo(selectedType);
        }

    }

    public void Whistle()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(whistleObject.transform.position,currentWhistleSize/2,LayerMask.GetMask("Mushroom"));

        foreach (Collider2D mushroom in list)
        {
            Mushroom m = mushroom.GetComponent<Mushroom>();
            if (m.GetTask() != Mushroom.Task.FollowPlayer)AddMushroomToSquad(m);   
        }

        if (currentWhistleSize < whistleMaxSize)
        {
            currentWhistleSize += Time.deltaTime * whistleIncreaseRate;
            if (currentWhistleSize > whistleMaxSize)currentWhistleSize = whistleMaxSize;
            whistleObject.transform.localScale = new Vector3(currentWhistleSize,currentWhistleSize,1);
        }
    }

    public void AddMushroomToSquad(Mushroom mushroom)
    {
        mushroom.SetTask(Mushroom.Task.FollowPlayer);
        mushroomSquad[(int)mushroom.GetMushroomType()].Add(mushroom.gameObject);

        if (selectedType == Type.None)SwitchTypeTo(mushroom.GetMushroomType());
    }

    public void RemoveMushroomFromSquad(Mushroom mushroom)
    {
        mushroom.SetTask(Mushroom.Task.Idle);
        int type = (int)mushroom.GetMushroomType();
        mushroomSquad[type].Remove(mushroom.gameObject);

        if ( mushroomSquad[type].Count == 0)
        {
            for (int i = type+1; i < mushroomSquad.Count-1; i++)
            {
                if (mushroomSquad[i].Count > 0)
                {
                    SwitchTypeTo((Type)i);
                    return;
                }
            }
            
            for (int i = type-1; i >= 0; i--)
            {
                if (mushroomSquad[i].Count > 0)
                {
                    SwitchTypeTo((Type)i);
                    return;
                }
            }
            SwitchTypeTo(Type.None);
        }
    }

    public void ThrowMushroom(Mushroom mushroom)
    {
        mushroom.transform.position = transform.position;
        mushroom.SetThrow(throwObject.transform.position,throwStrength);
    }

    public void SwitchTypeTo(Type type)
    {
        selectedType = type;
        switch (selectedType)
        {
            case Type.Red:
                throwObject.GetComponent<SpriteRenderer>().color = new Color(1,0.1273585f,0.1273585f,0.4f);
                break;
            case Type.Yellow:
                throwObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,0.08962262f,0.4f);
                break;
            case Type.Blue:
                throwObject.GetComponent<SpriteRenderer>().color = new Color(0.240566f,0.240566f,1f,0.4f);
                break;
            case Type.None:
                throwObject.GetComponent<SpriteRenderer>().color = new Color(0.3f,0.3f,0.3f,0.3f);
                break;
        }
    }

    public List<GameObject> GetMushroomsInSquad()
    {
        List<GameObject> allMushrooms = new List<GameObject>();

        foreach (List<GameObject> group in mushroomSquad)
        {
            foreach(GameObject mushroom in group)
            {
                allMushrooms.Add(mushroom);
            }
        }

        return allMushrooms;
    }

    public Type GetSelectedType()
    {
        return selectedType;
    }
}

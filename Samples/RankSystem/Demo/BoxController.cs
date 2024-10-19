using System.Collections;
using System.Collections.Generic;
using CorePublic.Helpers;
using Samples.Demo;
using UnityEngine;

public class BoxController : Singleton<BoxController>
{
    public float Speed => speed.GetCurrentVariable();
    public float Income => income.GetCurrentVariable();
    
    
    [SerializeField] private float gap = 1.5f;
    [SerializeField] private RankFloatVariable speed;
    [SerializeField] private RankFloatVariable income;
    [SerializeField] private RankIntVariable count;
    
    private List<Box> _spawnedBoxes;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnedBoxes = new List<Box>();
        int currentBox = count.GetCurrentVariable();
        for (int i = 0; i < currentBox; i++)
        {
            SpawnBox();
        }
        
        count.VariableChanged += OnCountChanged;

    }

    private void OnCountChanged(int obj)
    {
        SpawnBox();
    }


    public void SpawnBox()
    {
        var box=GameObject.CreatePrimitive(PrimitiveType.Cube);
        _spawnedBoxes.Add(box.AddComponent<Box>());
        var index=_spawnedBoxes.Count-1;
        box.transform.position= new Vector3(gap * index, 0, 0);
    }
    
}

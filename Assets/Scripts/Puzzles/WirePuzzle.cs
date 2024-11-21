using System;
using System.Linq;
using UnityEngine;

public class WirePuzzle : Puzzle
{
    [SerializeField]
    private MouseInteractableWire[] Wires;

    [SerializeField]
    private WireConnectionPoints WireConnectionPoints;

    private int[] PuzzleIndexes = new int[]{ };

    private void Start()
    {
        WireConnectionPoints.OnAllWiresConnected += OnAllWiresConnected;

        for (int i = 0; i < Wires.Length; i++) 
        {
            Wires[i].Index = i;
        }
    }

    public void SetPuzzle(int[] wireIndexes) 
    {
        PuzzleIndexes = wireIndexes;
        //TODO: Set something with the correct colors
        Material[] matchingMaterials = new Material[PuzzleIndexes.Length];
        for (int i = 0; i < wireIndexes.Length; i++)
        {
            int wireIndex = wireIndexes[i];
            matchingMaterials[i] = Wires[wireIndex].GetColouredMaterial();
        }

        WireConnectionPoints.SetMaterialForPoints(matchingMaterials);
    }

    private void OnAllWiresConnected(int[] wireIndexes)
    {
        Debug.Log("Wire indexes: " + string.Join(", ", wireIndexes));
        Debug.Log("Wire order indexes: " + string.Join(", ", PuzzleIndexes));
        if (PuzzleIndexes.SequenceEqual(wireIndexes)) 
        {
            SetPuzzleCompleted(true);
        }
    }

    public override void ButtonValue(int value)
    {
        //IDK actually, add this in tomorrow
    }
}

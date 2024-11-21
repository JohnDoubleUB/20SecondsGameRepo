using System;
using System.Linq;
using UnityEngine;

public class WirePuzzle : Puzzle
{
    [SerializeField]
    private MouseInteractableWire[] Wires;

    [SerializeField]
    private MouseInteractablePanel Panel;

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

    public override void OnReset()
    {
        if (Panel != null) 
        {
            Panel.ResetInteractable();
        }

        foreach(MouseInteractableWire wire in Wires) 
        {
            wire.ResetInteractable();
        }
    }

    public override void OnFullReset()
    {
        if (Panel != null)
        {
            Panel.ResetInteractable();
        }

        foreach (MouseInteractableWire wire in Wires)
        {
            wire.ResetInteractable();
        }
    }

    public void SetPuzzle(int[] wireIndexes) 
    {
        PuzzleIndexes = wireIndexes;

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
        if (PuzzleIndexes.SequenceEqual(wireIndexes)) 
        {
            SetPuzzleCompleted(true);
        }
    }

    private void OnDestroy()
    {
        if (WireConnectionPoints == null) 
        {
            return;
        }

        WireConnectionPoints.OnAllWiresConnected -= OnAllWiresConnected;
    }
}

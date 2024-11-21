using System;
using System.Collections.Generic;
using System.Linq;

public class SwitchData 
{
    public int[] OnSwitches;
    public List<int> LinkableSwitches;
    public List<int> OnSwitchIndexes;
    public bool[] InitialSwitchPositions;
}

public class RadioPuzzleData 
{
    public float Value1;
    public bool Value2;
}

public class SessionData
{
    //List of all the codes (2 digit sections)

    //Simon says
    const string AllowedSimonSaysColors = "RGBY";//'RGYBR'
    const int SimonSaysCount = 5;

    //Switch count
    const int enabledSwitchCount = 2;
    const int switchCount = 5;

    //Wires
    const int wireCount = 4;

    string[] Codes;
    char[] ValidCharacters;
    static Random RandomInstance;
    string CompleteCode = string.Empty;
    int CurrentCodeIndex = 0;

    //Puzzle data
    public string SimonSaysPattern { get; private set; }

    public RadioPuzzleData RadioData { get; private set; }

    //public float RadioValue1 { get; private set; }
    //public float RadioValue2 { get; private set; }

    public SwitchData SwitchData { get; private set; }

    public int[] WireIndexOrder { get; private set; }

    //public string CompletedCode

    public SessionData(int codeCount, int codeLength, int? seed = null, char[] validChar = null)
    {
        ValidCharacters = validChar == null ? new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' } : validChar;

        if(seed == null) 
        { 
            RandomInstance = new Random(); 
        }
        else
        {
            RandomInstance = new Random(seed.Value);
        }

        Codes = GenerateCodes(codeLength, codeCount, out CompleteCode);
        SimonSaysPattern = GenerateSimonSaysPattern();
        RadioData = GenerateRadioPuzzleData();
        SwitchData = GenerateSwitchPattern();
        WireIndexOrder = GenerateWireIndexOrder(); //TODO: this should work however will need checking

    }

    public void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = RandomInstance.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private int[] GenerateWireIndexOrder() 
    {
        int[] newArray = new int[wireCount];

        for (int i = 0; i < newArray.Length; i++) 
        {
            newArray[i] = i;
        }

        Shuffle(newArray);

        return newArray;
    }

    private RadioPuzzleData GenerateRadioPuzzleData() 
    {
        return new RadioPuzzleData { Value1 = (float)RandomInstance.NextDouble(), Value2 = RandomInstance.NextDouble() > 0.5f };
    }

    private string GenerateSimonSaysPattern() 
    {
        List<char> charactersToUse = AllowedSimonSaysColors.ToList();

        string result = "";

        for (int i = 0; i < SimonSaysCount; i++) 
        {
            if (charactersToUse.Count < 1) 
            {
                charactersToUse = AllowedSimonSaysColors.ToList();
            }

            int indexToUse = RandomInstance.Next(0, charactersToUse.Count);
            result += charactersToUse[indexToUse];
            charactersToUse.RemoveAt(indexToUse);
        }

        return result;
    }

    private SwitchData GenerateSwitchPattern() 
    {
        //Determine if it is 2 or 3 
        int randomIndex;

        int[] onSwitches = new int[switchCount];
        bool[] initialSwitchPositions = new bool[switchCount];

        List<int> linkableSwitches = new List<int>();
        List<int> onSwitchIndexes = new List<int>();

        for (int i = 0; i < onSwitches.Length; i++)
        {
            linkableSwitches.Add(i);
            initialSwitchPositions[i] = RandomInstance.NextDouble() > 0.5f;
        }

        for (int i = 0; i < enabledSwitchCount;)
        {
            randomIndex = RandomInstance.Next(0, switchCount);//Random.Range(0, onSwitches.Length);

            if (onSwitches[randomIndex] < 1)
            {
                onSwitches[randomIndex] = 1;
                linkableSwitches.Remove(randomIndex);
                onSwitchIndexes.Add(randomIndex);
                if (randomIndex + 1 < onSwitches.Length)
                {
                    onSwitches[randomIndex + 1] = 2;
                }

                if (randomIndex - 1 > 0)
                {
                    onSwitches[randomIndex - 1] = 2;
                }
                i++;
            }
        }

        return new SwitchData { LinkableSwitches = linkableSwitches, OnSwitches = onSwitches, OnSwitchIndexes = onSwitchIndexes, InitialSwitchPositions = initialSwitchPositions };
    }

    private string[] GenerateCodes(int codeLength, int codeCount, out string completeCode) 
    {
        completeCode = string.Empty;

        string[] newArray = new string[codeCount];

        for(int row = 0; row < codeCount; row++)
        {
            for(int col = 0; col < codeLength; col++) 
            {
                char newCharacter = GetRandomCodeCharacter();
                newArray[row] += newCharacter;
                completeCode += newCharacter;
            }
        }

        return newArray;
    }

    public bool TryGetNextUnusedCode(out string code) 
    {
        if (CurrentCodeIndex < Codes.Length) 
        {
            code = Codes[CurrentCodeIndex];
            CurrentCodeIndex++;
            return true;
        }

        code = null;
        return false;
    }

    public bool AllCodesGiven() 
    { 
        return CurrentCodeIndex >= Codes.Length; 
    }

    private char GetRandomCodeCharacter() 
    {
        return ValidCharacters[RandomInstance.Next(0, ValidCharacters.Length)];
    }

    public string[] GetAllCodes() 
    {
        return Codes;
    }

    public string GetCompleteCode() 
    {
        return CompleteCode;
    }
}

using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAgrided : MonoBehaviour
{

    int rows = 32;
    int cols = 32;
    int[] cells;

    GameObject[,] cellObjs;
    public GameObject baseObject;

    public int[] ruleset = { 0, 0, 0, 1, 1, 1, 1, 0 }; //rule 30

    public int TheRule = 30;
    public int TheBinary;

    private int generation;

    private float timer;

    //wipe the isActive slate clean, or pass the new rule through the previous rule
    public bool regenerateFormation = true;

    private void Reset()
    {
        if(regenerateFormation)
        {
            //disable ALL the girls
            for(int r = 0; r > rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    cellObjs[c, r].SetActive(false);

                }
            }
           
        }
        
        //1d cell data per gen is of column width
        cells = new int[cols];

        //each rebuild first pass is genzero
        generation = 0;
        
        //to set or not set? Maybe use current states? thinking about it...
        cells[cols / 2] = 1;

        timer = 0;


        TheBinary = int_to_binary(TheRule);

        //clear the array
        for (var i = 0; i < 8; i++) ruleset[i] = 0;
        //use strings to bust the binary and assign to the ruleset
        //I'm sure there is some groovy way to do this in one line
        //with math, but I'm too stupid, and this works
        var B = TheBinary.ToString();

        var C = B.ToCharArray();
        var d = 7;
        for (var i = C.Length - 1; i > -1; i--)
        {
            ruleset[d] = (int)char.GetNumericValue(C[i]);
            d--;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        regenerateFormation = true; //maybe not if you have saved a formation to JSON

        //reserve space for the ladies.
        cellObjs = new GameObject[cols, rows];        

        //build grid of ladies
        for(int r = 0; r < rows; r++)
        {
            for(int c = 0; c < cols; c++)
            {
                cellObjs[c, r] = Instantiate(baseObject, transform);
                cellObjs[c, r].transform.localPosition = Vector3.right * c + Vector3.forward * r;
                cellObjs[c, r].SetActive(false);  //everyone exists but everyone is inactive
            }

        }
        //once lady array is built, we can prepare for the first generation 
        Reset();

    }

    // Update is called once per frame
    void Update()
    {
        //spread each gen over a little time so we can see it happen
        if (Time.time - timer > 0.1f)
        {
            //lets keep it square-ish
            if (generation < cols) generate();

            timer = Time.time;
        }

        //regenerate on key R (we don't destroy the ladies)
        if (Input.GetKeyDown(KeyCode.R)) Reset();

    }

    private void generate()
    {
       

        // First we create an empty array for the new values
        var nextgen = new int[cols];

        // Ignore edges that only have one neighor
        for (var i = 1; i < cols - 1; i++)
        {
            var left = cells[i - 1]; // Left neighbor state
            var me = cells[i]; // Current state
            var right = cells[i + 1]; // Right neighbor state
            nextgen[i] = Rules(left, me, right); // Compute next generation state based on ruleset
        }

        // The next generation is the new this generation
        cells = nextgen;

        if(regenerateFormation)
        {
            for (var i = 0; i < cols; i++)
            {
                if (cells[i] == 1)
                    cellObjs[i, generation].SetActive(true);
                else
                    cellObjs[i, generation].SetActive(false);
            }

        }
        else   //with the existing formation, pass animations through it
        {
            for (var i = 0; i < cols; i++)
            {
                if (cells[i] == 1)
                    cellObjs[i, generation].GetComponent<PlayerAnimator>().Sneak();
                else
                    cellObjs[i, generation].GetComponent<PlayerAnimator>().Run();
            }

        }

        generation++;

    }

    private int Rules(int a, int b, int c)
    {
        if (a == 1 && b == 1 && c == 1) return ruleset[0];
        if (a == 1 && b == 1 && c == 0) return ruleset[1];
        if (a == 1 && b == 0 && c == 1) return ruleset[2];
        if (a == 1 && b == 0 && c == 0) return ruleset[3];
        if (a == 0 && b == 1 && c == 1) return ruleset[4];
        if (a == 0 && b == 1 && c == 0) return ruleset[5];
        if (a == 0 && b == 0 && c == 1) return ruleset[6];
        if (a == 0 && b == 0 && c == 0) return ruleset[7];


        return 0;
    }

    private int int_to_binary(int n)
    {
        var binary = 0;
        int remainder, i;
        for (i = 1; n != 0; i = i * 10)
        {
            remainder = n % 2;
            n /= 2;
            binary += remainder * i;
        }

        return binary;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMapping
{
    public int normalAttack = 1001;
    public int spellOne = 0;
    public int spellTwo = 0;
    public int spellThree = 0;
    public void Clear()
    {
        normalAttack = 0;
        spellOne = 0;
        spellTwo = 0;
        spellThree = 0;
    }
    public bool HasStudy(int id)
    {
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Citizen
{
    public int Age { get; set; }
    public bool IsRetired { get; private set; }
    public bool IsDead { get; private set; }

    public Citizen(int age)
    {
        Age = age;
    }

    /// <summary>
    /// Ages citizen by one year
    /// </summary>
    public void AgeOneYear()
    {
        if (IsDead)
        {
            return;
        }

        Age++;

        if (Age >= 65)
        {
            IsRetired = true;
        }

        float deathProbability = CalculateDeathProbability();
        if (1 < deathProbability)
        {
            IsDead = true;
        }
    }

    /// <summary>
    /// Calculates death probability of the citizen
    /// </summary>
    /// <returns></returns>
    private float CalculateDeathProbability()
    {
        if (Age < 65)
        {
            return 0f;
        }
        else
        {
            int yearsOverRetirementAge = Age - 65;
            float baseDeathProbability = 0.01f; 
            System.Random random = new System.Random();
            return baseDeathProbability * Mathf.Pow(2f, yearsOverRetirementAge*(float)random.NextDouble());
        }
    }
}

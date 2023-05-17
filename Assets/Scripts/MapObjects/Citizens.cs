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
        // if (age < 18 || age > 60)
        // {
        //     throw new ArgumentException("Age must be between 18 and 60.");
        // }
        Age = age;
    }

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

        // if (IsRetired && !IsDead)
        // {
        //     float pension = CalculatePension();
        // }

        float deathProbability = CalculateDeathProbability();
        if (1 < deathProbability)
        {
            IsDead = true;
        }
    }

    // private float CalculatePension()
    // {
    //     float totalTaxPaid = 0f;
    //     int yearsConsidered = Mathf.Min(20, City.Year - 65);
    //     foreach (TaxRecord taxRecord in City.TaxRecords)
    //     {
    //         if (taxRecord.Year >= City.Year - yearsConsidered && taxRecord.CitizenId == Id)
    //         {
    //             totalTaxPaid += taxRecord.Amount;
    //         }
    //     }
    //     float averageTaxPaid = yearsConsidered > 0 ? totalTaxPaid / yearsConsidered : 0f;
    //     return averageTaxPaid / 2f;
    // }

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

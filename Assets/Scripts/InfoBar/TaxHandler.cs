using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxHandler : MonoBehaviour
{
    public int taxValue;

    public void OnTaxValueChanged(float value)
    {
        taxValue = Mathf.RoundToInt(value);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(taxValue);
    }
}

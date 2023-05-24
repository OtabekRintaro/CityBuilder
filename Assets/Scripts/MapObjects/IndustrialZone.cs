using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialZone : MapObject
{
    public IndustrialBuildings factoryPrefab;
    public IndustrialBuildings factory;
    
    public bool hasFactory()
    {
        return factory is not null;
    }
    public void buildFactory()
    {
        if (fire is not null)
            return;

        IndustrialBuildings factory = Instantiate<IndustrialBuildings>(factoryPrefab);

        this.factory = factory;
        factory.transform.SetParent(this.transform);
        factory.transform.localPosition = new Vector3(3,0,-2);
    }

    public void DemolishFactory()
    {
        if (factory is null)
            return;
        Destroy(factory);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

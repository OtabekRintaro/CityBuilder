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
        IndustrialBuildings factory = Instantiate<IndustrialBuildings>(factoryPrefab);

        this.factory = factory;
        factory.transform.SetParent(this.transform);
        factory.transform.localPosition = new Vector3(3,0,-2);
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

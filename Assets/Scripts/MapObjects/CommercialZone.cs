using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommercialZone : MapObject
{
    public CommercialBuildings commercialBuildingsPrefab;
    public CommercialBuildings commercialBuildings;

    public bool hasCommercialBuildings()
    {
        return commercialBuildings is not null;
    }
    public void buildCommercialBuildings()
    {
        if (fire is not null)
            return;

        CommercialBuildings commercialBuildings = Instantiate<CommercialBuildings>(commercialBuildingsPrefab);

        this.commercialBuildings = commercialBuildings;
        commercialBuildings.transform.SetParent(this.transform);
        commercialBuildings.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void DemolishBuildings()
    {
        if (commercialBuildings is null)
            return;
        Destroy(commercialBuildings);
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

using System;
using System.Collections.Generic;

namespace LeafEmber.Estate
{

public sealed class EstateService : IEstateService
{
    private readonly EstateSnapshot state = new();

    public EstateService()
    {
        AddFacility("field-edge", "Field edge", "Pilot observation plot", true);
        AddFacility("curing-barn", "Curing barn", "Weathered but usable", true);
        AddFacility("fermentation-room", "Fermentation room", "Basic manual control", true);
        AddFacility("leaf-storage", "Leaf storage", "Dry, limited capacity", true);
        AddFacility("workshop", "Workshop", "One personal rolling bench", true);
        AddFacility("aging-room", "Aging room", "Small reserve capacity", true);
        AddFacility("office", "Finca office", "Records recently recovered", true);
    }

    public IReadOnlyList<FacilityState> Facilities => state.facilities;

    public EstateSnapshot Capture()
    {
        return state.Copy();
    }

    public void Restore(EstateSnapshot snapshot)
    {
        if (snapshot == null || snapshot.facilities == null)
        {
            throw new ArgumentException("The estate snapshot is invalid.", nameof(snapshot));
        }

        state.estateName = snapshot.estateName;
        state.facilities.Clear();
        foreach (FacilityState facility in snapshot.facilities)
        {
            if (facility == null || string.IsNullOrWhiteSpace(facility.id))
            {
                throw new ArgumentException(
                    "Every restored facility requires an identifier.",
                    nameof(snapshot));
            }

            state.facilities.Add(facility.Copy());
        }
    }

    private void AddFacility(
        string id,
        string displayName,
        string condition,
        bool operational)
    {
        state.facilities.Add(new FacilityState
        {
            id = id,
            displayName = displayName,
            condition = condition,
            operational = operational,
        });
    }
}
}

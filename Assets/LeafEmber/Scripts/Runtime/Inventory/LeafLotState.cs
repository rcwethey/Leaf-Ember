using System;
using LeafEmber.Cigar;

namespace LeafEmber.Inventory
{

[Serializable]
public sealed class LeafLotState
{
    public string id;
    public string displayName;
    public string origin;
    public string grower;
    public string tobaccoType;
    public string harvestReference;
    public string processHistory;
    public string intendedRole;
    public string observations;
    public float quantityKilograms;
    public LeafPotentialState potential;
    public LeafProcessState process;

    public LeafLotState Copy()
    {
        return new LeafLotState
        {
            id = id,
            displayName = displayName,
            origin = origin,
            grower = grower,
            tobaccoType = tobaccoType,
            harvestReference = harvestReference,
            processHistory = processHistory,
            intendedRole = intendedRole,
            observations = observations,
            quantityKilograms = quantityKilograms,
            potential = potential?.Copy(),
            process = process?.Copy(),
        };
    }
}
}

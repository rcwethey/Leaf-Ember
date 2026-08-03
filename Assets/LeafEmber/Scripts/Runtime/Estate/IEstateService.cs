using System.Collections.Generic;

namespace LeafEmber.Estate
{

public interface IEstateService
{
    IReadOnlyList<FacilityState> Facilities { get; }

    EstateSnapshot Capture();

    void Restore(EstateSnapshot snapshot);
}
}

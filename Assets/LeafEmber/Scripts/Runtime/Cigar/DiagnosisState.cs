using System;

namespace LeafEmber.Cigar
{

[Serializable]
public sealed class DiagnosisState
{
    public string prototypeId;
    public DiagnosisKind hypothesis;
    public string reasoning;
    public string intendedChange;
    public int recordedAtElapsedBlock;

    public DiagnosisState Copy()
    {
        return (DiagnosisState)MemberwiseClone();
    }
}
}

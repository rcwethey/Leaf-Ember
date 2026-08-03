namespace LeafEmber.Cigar
{

public static class CigarDevelopmentText
{
    public static string IntentName(IntentPreset preset)
    {
        return preset switch
        {
            IntentPreset.QuietWorkshop => "Quiet Workshop",
            IntentPreset.RainOnRedEarth => "Rain on Red Earth",
            IntentPreset.AfterSupper => "After Supper",
            _ => preset.ToString(),
        };
    }

    public static string IntentDescription(IntentPreset preset)
    {
        return preset switch
        {
            IntentPreset.QuietWorkshop =>
                "Medium strength and body; cedar-like wood over restrained roast; gradual, calm progression and a clean finish.",
            IntentPreset.RainOnRedEarth =>
                "Medium-to-full strength; earth and warm spice; building progression with a long, dry finish.",
            IntentPreset.AfterSupper =>
                "Medium strength with fuller flavor intensity; roast and baked sweetness; layered middle and lingering finish.",
            _ => string.Empty,
        };
    }

    public static string BlendName(BlendPreset preset)
    {
        return preset switch
        {
            BlendPreset.EstateForward => "Estate Forward",
            BlendPreset.SpiceBridge => "Spice Bridge",
            BlendPreset.RoundAndRoasted => "Round and Roasted",
            _ => preset.ToString(),
        };
    }

    public static string BlendDescription(BlendPreset preset)
    {
        return preset switch
        {
            BlendPreset.EstateForward =>
                "55% estate seco, 25% Ortega viso, 12% San Jerónimo binder, 8% Las Lomas wrapper.",
            BlendPreset.SpiceBridge =>
                "35% estate seco, 45% Ortega viso, 12% San Jerónimo binder, 8% Las Lomas wrapper.",
            BlendPreset.RoundAndRoasted =>
                "45% estate seco, 30% Ortega viso, 15% San Jerónimo binder, 10% Las Lomas wrapper.",
            _ => string.Empty,
        };
    }

    public static string DiagnosisName(DiagnosisKind diagnosis)
    {
        return diagnosis switch
        {
            DiagnosisKind.ComponentDominance => "Component dominance",
            DiagnosisKind.TobaccoCondition => "Tobacco condition",
            DiagnosisKind.BunchCompression => "Bunch compression and airflow",
            DiagnosisKind.InsufficientRest => "Insufficient prototype rest",
            DiagnosisKind.CombustionAndFormat => "Combustion and format",
            _ => diagnosis.ToString(),
        };
    }

    public static string DiagnosisDescription(DiagnosisKind diagnosis)
    {
        return diagnosis switch
        {
            DiagnosisKind.ComponentDominance =>
                "A forceful filler component may be masking quieter material. Test a gentler ratio.",
            DiagnosisKind.TobaccoCondition =>
                "Leaf condition at rolling may have affected structure, heat, or sensory sharpness.",
            DiagnosisKind.BunchCompression =>
                "Density or airflow may be changing draw, combustion, temperature, and perception.",
            DiagnosisKind.InsufficientRest =>
                "The prototype may need more recovery time before its components present coherently.",
            DiagnosisKind.CombustionAndFormat =>
                "The chosen dimensions may be concentrating or cooling the blend differently than intended.",
            _ => string.Empty,
        };
    }

    public static string AromaFamilyName(AromaFamily family)
    {
        return family switch
        {
            AromaFamily.EarthAndMineral => "earth and mineral",
            AromaFamily.Wood => "wood",
            AromaFamily.Spice => "spice",
            AromaFamily.RoastedAndNutty => "roasted and nutty",
            AromaFamily.SweetAndBaked => "sweet and baked",
            AromaFamily.Fruit => "fruit",
            AromaFamily.Floral => "floral",
            AromaFamily.HerbalAndGreen => "herbal and green",
            AromaFamily.FermentedAndLeather => "fermented and leather",
            _ => family.ToString(),
        };
    }
}
}

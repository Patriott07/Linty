using System.Collections.Generic;

internal static class GuideDatabaseHelpers
{
    public static GuideItem Item(string propertyName, string description, params string[] effects)
    {
        var conditions = new Dictionary<int, string>();
        for (int i = 0; i < effects.Length; i++)
            conditions[i] = effects[i];

        return new GuideItem
        {
            propertyName = propertyName,
            oneLineDesc = description,
            effectConditions = conditions
        };
    }

    public static GuideItem State(GuideItem guide, int state)
    {
        if (guide.effectConditions != null && guide.effectConditions.TryGetValue(state, out string effect))
            guide.currentEffect = effect;

        return guide;
    }

    public static int Range(float value, float low, float high)
    {
        if (value <= low) return 0;
        if (value >= high) return 2;
        return 1;
    }
}

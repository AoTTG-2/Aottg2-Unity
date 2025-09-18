public class BombUtil
{

    public static float GetOldRadiusCost(float radius)
    {
        return (radius - 20f) / 4f;
    }

    public static float GetOldSpeedCost(float speed)
    {
        return (speed - 200f) / 60f;
    }

    public static float GetOldCooldownCost(float cooldown)
    {
        return (cooldown - 5f) / -0.4f;
    }

    public static float GetOldRangeCost(float range)
    {
        return (range - 200f) / 60f;
    }

    public static float GetOldBombRadius(float pointsSpent)
    {
        return pointsSpent * 4f + 20f;
    }

    public static float GetOldBombSpeed(float pointsSpent)
    {
        return pointsSpent * 60f + 200f;
    }

    public static float GetOldBombCooldown(float pointsSpent)
    {
        return pointsSpent * -0.4f + 5f;
    }

    public static float GetOldBombRange(float pointsSpent)
    {
        return (pointsSpent * 60f) + 200f;
    }


    public static float GetBombStat(float pointsSpent, float maxValue, float valuePerPoint, float cutoff)
    {
        if (cutoff > 10f)
        {
            return maxValue - ((10 - pointsSpent) * valuePerPoint);
        }
        if (pointsSpent == 10)
        {
            return maxValue;
        }
        if (pointsSpent > cutoff)
        {
            float i = 10f - pointsSpent;
            return maxValue - (i * valuePerPoint * 0.5f);
        }
        return maxValue - ((10f - cutoff) * valuePerPoint * 0.5f + (cutoff - pointsSpent) * valuePerPoint);
    }

    public static float GetBombRadius(float pointsSpent, float oldMinCost, float oldMaxCost, float cutoff)
    {
        // 2 - 7
        float valuePerPoint = (GetOldBombRadius(oldMaxCost) - GetOldBombRadius(oldMinCost)) / 10f;
        float baseValue = GetOldBombRadius(oldMaxCost);
        return GetBombStat(pointsSpent, baseValue, valuePerPoint, cutoff);
    }

    public static float GetBombRange(float pointsSpent, float oldMinCost, float oldMaxCost, float cutoff)
    {
        // 0 - 4
        float valuePerPoint = (GetOldBombRange(oldMaxCost) - GetOldBombRange(oldMinCost)) / 10f;
        float baseValue = GetOldBombRange(oldMaxCost);
        return GetBombStat(pointsSpent, baseValue, valuePerPoint, cutoff);
    }

    public static float GetBombSpeed(float pointsSpent, float oldMinCost, float oldMaxCost, float cutoff)
    {
        // 0 - 10
        float valuePerPoint = (GetOldBombSpeed(oldMaxCost) - GetOldBombSpeed(oldMinCost)) / 10f;
        float baseValue = GetOldBombSpeed(oldMaxCost);
        return GetBombStat(pointsSpent, baseValue, valuePerPoint, cutoff);
    }

    public static float GetBombCooldown(float pointsSpent, float oldMinCost, float oldMaxCost, float cutoff)
    {
        // 4 - 7
        float valuePerPoint = (GetOldBombCooldown(oldMaxCost) - GetOldBombCooldown(oldMinCost)) / 10f;
        float baseValue = GetOldBombCooldown(oldMaxCost);
        return GetBombStat(pointsSpent, baseValue, valuePerPoint, cutoff);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using MaxPowerLevel.Models;

namespace MaxPowerLevel.Services
{
    public class Season8Recommendations : IRecommendations
    {
        private const int SoftCap = 900;
        private const int PowerfulCap = 950;
        private const int HardCap = 960;

        public IEnumerable<string> GetRecommendations(IEnumerable<Item> allItems, int powerLevel)
        {
            if(powerLevel < SoftCap)
            {
                return new[]
                {
                    $"Rare/Legendary Engrams to increase your power level to {SoftCap}"
                };
            }

            if(powerLevel < PowerfulCap)
            {
                // Recommmend legendary engrams for any slots that could easily be upgraded
                var recommendations = allItems.Where(item => item.PowerLevel <= powerLevel - 2)
                    .OrderBy(item => item.PowerLevel)
                    .GroupBy(item => item.PowerLevel)
                    .Select(items => 
                    {
                        var slotNames = items.Select(item => item.Slot.Name)
                            .OrderBy(slotName => slotName);
                        return $"Rare/Legendary Engrams: {string.Join(", ", slotNames)}";
                    })
                    .Concat(new[] { "Powerful Engrams"})
                    .Concat(new[] { "Pinnacle Engrams" });
                return recommendations;
            }

            if(powerLevel < HardCap)
            {
                return new[] { "Pinnacle Engrams" };
            }

            // At the hard cap. Nothing to do.
            return Enumerable.Empty<string>();
        }

        public IEnumerable<Engram> GetEngramPowerLevels(int powerLevel)
        {
            if (powerLevel < SoftCap)
            {
                return new[]
                {
                    // TODO: Verify power level of engrams before the soft cap
                    new Engram("Rare/Legendary Engram",  powerLevel + 1, powerLevel + 2)
                };
            }

            if (powerLevel < PowerfulCap)
            {
                return new[]
                {
                    new Engram("Rare/Legendary Engram", powerLevel - 3, Math.Min(powerLevel, PowerfulCap)),
                    new Engram("Powerful Engram (Tier 1)", Math.Min(powerLevel + 3, PowerfulCap)),
                    new Engram("Powerful Engram (Tier 2)", Math.Min(powerLevel + 5, PowerfulCap)),
                    new Engram("Powerful Engram (Tier 3)", Math.Min(powerLevel + 6, PowerfulCap)),
                    // TODO: Verify power level of Pinnacle engrams
                    new Engram("Pinnacle Engram", Math.Min(powerLevel + 6, PowerfulCap + 1))
                };
            }

            if (powerLevel <= HardCap)
            {
                // TODO: Verify power levels of engrams at the hard cap
                return new[]
                {
                    new Engram("Rare/Legendary Engram", powerLevel - 3, powerLevel),
                    new Engram("Powerful Engram (Tier 1)", powerLevel),
                    new Engram("Powerful Engram (Tier 2)", powerLevel),
                    new Engram("Powerful Engram (Tier 3)", powerLevel),
                    // TODO: Verify power level of Pinnacle engrams
                    new Engram("Pinnacle Engram", powerLevel + 1)
                };
            }

            throw new Exception($"Unknown power level {powerLevel}");
        }
    }
}
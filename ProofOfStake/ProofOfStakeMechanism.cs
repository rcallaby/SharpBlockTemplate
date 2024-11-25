using System;
using System.Collections.Generic;
using System.Linq;

namespace ProofOfStake
{
    public static class ProofOfStakeMechanism
    {
        public static string SelectValidator(Dictionary<string, decimal> stakes)
        {
            var totalStake = stakes.Values.Sum();
            var random = new Random();
            double pick = random.NextDouble() * (double)totalStake;

            double cumulative = 0;
            foreach (var staker in stakes)
            {
                cumulative += (double)staker.Value;
                if (cumulative >= pick)
                {
                    return staker.Key;
                }
            }

            throw new InvalidOperationException("No valid validator selected.");
        }
    }
}

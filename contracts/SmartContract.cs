using System.Collections.Generic;
using System.Linq;
using Blockchain;

namespace Contracts
{
    public class GovernanceContract : ISmartContract
    {
        private Dictionary<string, int> Votes { get; } = new Dictionary<string, int>();

        public void Execute(BlockchainInstance blockchain, Transaction triggerTransaction)
        {
            string voter = triggerTransaction.Sender;
            string candidate = triggerTransaction.Receiver;

            if (!blockchain.Balances.ContainsKey(voter) || blockchain.Balances[voter] <= 0)
            {
                throw new InvalidOperationException("Insufficient balance to vote.");
            }

            Votes[candidate] = Votes.GetValueOrDefault(candidate) + 1;
        }

        public string GetWinner() => Votes.OrderByDescending(v => v.Value).FirstOrDefault().Key;
    }
}

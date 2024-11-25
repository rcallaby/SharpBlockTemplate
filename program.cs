using Blockchain;
using Contracts;
using ProofOfStake;

class Program
{
    static void Main(string[] args)
    {
        BlockchainInstance blockchain = BlockchainInstance.Instance;

        // Mine genesis block
        Console.WriteLine("Mining Genesis Block...");
        blockchain.MinePendingTransactions("Miner1");

        // Stake tokens
        Console.WriteLine("Staking tokens...");
        blockchain.Stake("Miner1", 10);

        // Add transactions
        blockchain.AddTransaction(new Transaction("Miner1", "User1", 5));
        blockchain.MinePendingTransactionsPoS();

        // Display balances
        Console.WriteLine($"Balance of Miner1: {blockchain.GetBalance("Miner1")}");
        Console.WriteLine($"Balance of User1: {blockchain.GetBalance("User1")}");

        // Governance voting via smart contract
        GovernanceContract governance = new GovernanceContract();
        blockchain.ExecuteSmartContract(governance, new Transaction("Miner1", "Candidate1", 0));
        blockchain.ExecuteSmartContract(governance, new Transaction("Miner1", "Candidate2", 0));
        blockchain.ExecuteSmartContract(governance, new Transaction("Miner1", "Candidate1", 0));

        Console.WriteLine($"Winner of governance vote: {governance.GetWinner()}");
    }
}

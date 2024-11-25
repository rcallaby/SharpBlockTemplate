using System;
using System.Collections.Generic;
using System.Linq;

namespace Blockchain
{
    public class BlockchainInstance
    {
        private static BlockchainInstance _instance;
        public List<Block> Chain { get; private set; }
        public List<Transaction> PendingTransactions { get; private set; } = new List<Transaction>();
        public Dictionary<string, decimal> Balances { get; private set; } = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> Stakes { get; private set; } = new Dictionary<string, decimal>();
        public decimal MiningReward { get; set; } = 10m;

        private BlockchainInstance()
        {
            Chain = new List<Block> { CreateGenesisBlock() };
        }

        public static BlockchainInstance Instance => _instance ??= new BlockchainInstance();

        private Block CreateGenesisBlock() => new Block(0, "0", new List<Transaction>());

        public Block GetLatestBlock() => Chain.Last();

        public void AddTransaction(Transaction transaction)
        {
            if (Balances.ContainsKey(transaction.Sender) && Balances[transaction.Sender] >= transaction.Amount)
            {
                Balances[transaction.Sender] -= transaction.Amount;
                Balances[transaction.Receiver] = Balances.GetValueOrDefault(transaction.Receiver) + transaction.Amount;
                PendingTransactions.Add(transaction);
            }
            else
            {
                throw new InvalidOperationException("Invalid transaction.");
            }
        }

        public void MinePendingTransactions(string minerAddress)
        {
            var block = new Block(Chain.Count, GetLatestBlock().Hash, new List<Transaction>(PendingTransactions));
            block.MineBlock(2);
            Chain.Add(block);
            PendingTransactions.Clear();

            AddTransaction(new Transaction("System", minerAddress, MiningReward));
        }

        public void Stake(string address, decimal amount)
        {
            if (!Balances.ContainsKey(address) || Balances[address] < amount)
            {
                throw new InvalidOperationException("Insufficient balance for staking.");
            }

            Balances[address] -= amount;
            Stakes[address] = Stakes.GetValueOrDefault(address) + amount;
        }

        public void MinePendingTransactionsPoS()
        {
            string validator = ProofOfStakeMechanism.SelectValidator(Stakes);
            var block = new Block(Chain.Count, GetLatestBlock().Hash, new List<Transaction>(PendingTransactions));
            block.MineBlock(2);
            Chain.Add(block);
            PendingTransactions.Clear();

            AddTransaction(new Transaction("System", validator, MiningReward));
        }

        public decimal GetBalance(string address) => Balances.GetValueOrDefault(address, 0);
    }
}

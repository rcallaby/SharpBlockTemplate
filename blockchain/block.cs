using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    public class Block
    {
        public int Index { get; }
        public string PreviousHash { get; }
        public long Timestamp { get; }
        public List<Transaction> Transactions { get; }
        public string Hash { get; private set; }
        public int Nonce { get; private set; }

        public Block(int index, string previousHash, List<Transaction> transactions)
        {
            Index = index;
            PreviousHash = previousHash;
            Transactions = transactions;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            using var sha256 = SHA256.Create();
            var rawData = $"{Index}{PreviousHash}{Timestamp}{Nonce}{string.Join("", Transactions)}";
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData)));
        }

        public void MineBlock(int difficulty)
        {
            string target = new string('0', difficulty);
            while (!Hash.StartsWith(target))
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }
    }
}

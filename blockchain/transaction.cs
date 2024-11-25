namespace Blockchain
{
    public class Transaction
    {
        public string Sender { get; }
        public string Receiver { get; }
        public decimal Amount { get; }

        public Transaction(string sender, string receiver, decimal amount)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = amount;
        }

        public override string ToString() => $"{Sender}->{Receiver}:{Amount}";
    }
}

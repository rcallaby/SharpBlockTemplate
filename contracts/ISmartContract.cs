using Blockchain;

namespace Contracts
{
    public interface ISmartContract
    {
        void Execute(BlockchainInstance blockchain, Transaction triggerTransaction);
    }
}

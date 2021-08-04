using Game.Contract;
using System;

namespace MandatoryContracts.Contracts
{
    internal class MandatoryContract
    {
        internal Contract Contract { get; }
        internal int ContractID { get; }
        internal TimeSpan AutoAcceptAt { get; }

        public MandatoryContract(Contract contract, TimeSpan generatedAt)
        {
            Contract = contract;
            ContractID = contract.ContractNumber;
            AutoAcceptAt = generatedAt.Add(TimeSpan.FromHours(1));
        }
    }
}

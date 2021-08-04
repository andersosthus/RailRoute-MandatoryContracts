using Game.Contract;
using System;

namespace MandatoryContracts.Contracts
{
    internal class MandatoryContract
    {
        internal int ContractID { get; }
        internal TimeSpan AutoAcceptAt { get; }

        public MandatoryContract(Contract contract, TimeSpan generatedAt)
        {
            ContractID = contract.ContractNumber;
            AutoAcceptAt = generatedAt.Add(TimeSpan.FromHours(1));
        }
    }
}

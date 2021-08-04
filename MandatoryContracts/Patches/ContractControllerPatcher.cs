using Game.Contract;
using Game.Train;
using HarmonyLib;
using MandatoryContracts.Contracts;

namespace MandatoryContracts.Patches
{
    [HarmonyPatch(typeof(ContractController))]
    public class ContractControllerPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("TrainScheduleFinished")]
        static void TrainScheduleFinished(object[] args, ContractController __instance)
        {
            var train = (Train)args[0];
            if(train.Contract == null)
            {
                return;
            }

            var contract = train.Contract;
            var isNonRejectable = MandatoryContractsController.IsContractMandatory(contract);
            if (!isNonRejectable)
            {
                return;
            }

            if(contract.Status == ContractStatus.FULFILLED)
            {
                var type = __instance.GetType();
                var removeFunc = type.GetMethod("Remove", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.IgnoreCase);
                if(removeFunc == null)
                {
                    Logger.Error("Unable to find ContractController.Remove method");
                    return;
                }

                MandatoryContractsController.RemoveMandatoryContract(contract);
                removeFunc.Invoke(type, new object[] { contract });

                Logger.Debug("Fulfilled mandatory contract removed");
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("OfferFinalContract")]
        static bool OfferFinalContract(Contract contract, ContractController __instance)
        {
            // if this is a regular contract, let the original code run
            var isNonRejectable = MandatoryContractsController.IsContractMandatory(contract);
            if(!isNonRejectable)
            {
                return true;
            }

            // it's one of our contracts, let's do our thing and prevent the original code from running
            contract.Prototype.CreateScheduleFromRun(contract.Prototype.ActualVisits);
            contract.RecalculateRewardFromTrain(contract.Prototype);
            contract.DispatchedCount++;
            __instance.AcceptContractAfterTrial(contract);

            return false;
        }
    }
}

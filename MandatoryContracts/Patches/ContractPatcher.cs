using Game.Contract;
using HarmonyLib;
using MandatoryContracts.Contracts;

namespace MandatoryContracts.Patches
{
    [HarmonyPatch(typeof(Contract))]
    public class ContractPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("IsRejectable", MethodType.Getter)]
        static void IsRejectable(Contract __instance, ref bool __result)
        {
            var isMandatory = MandatoryContractsController.IsContractMandatory(__instance);

            if (!isMandatory)
            {
                return;
            }

            __result = false;
        }
    }
}

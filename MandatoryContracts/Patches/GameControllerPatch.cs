using Game;
using HarmonyLib;
using MandatoryContracts.Contracts;

namespace MandatoryContracts.Patches
{
    [HarmonyPatch(typeof(GameController))]
    class GameControllerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Awake")]
        static void Awake(GameController __instance)
        {
            Logger.Debug("Patching gamecontroller");
            MandatoryContractsController.Attach(__instance);
        }
    }
}

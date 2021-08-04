using Game.Context;
using Game.Mod;
using HarmonyLib;
using System;
using System.Threading.Tasks;
using Utils;

namespace MandatoryContracts
{
    public class EntryPoint : IGameMod
    {
        private static readonly Harmony _patch = new Harmony(Constants.PatchID);

        internal static IControllers deps;

        public CachedLocalizedString Title => Constants.ModName;
        public CachedLocalizedString Description => "Generates contracts at random interval that you cannot reject";

        public async Task OnContextChanged(IControllers dependencies)
        {
            Logger.Debug("ContextChanged");

            deps = dependencies;

            await Task.Yield();
        }

        public async Task OnDisable()
        {
            DeactivatePatches();
            await Task.Yield();
        }

        public async Task OnEnable()
        {
            ActivatePatches();
            await Task.Yield();
        }

        public void ActivatePatches()
        {
            try
            {
                _patch.PatchAll();
                Logger.Debug("Activated Harmony patches");
            } catch (Exception ex)
            {
                Logger.Error("Failed to activate Harmony patches!");
                Logger.Ex(ex);
            }
        }

        public void DeactivatePatches()
        {
            try
            {
                if(Harmony.HasAnyPatches(Constants.PatchID))
                {
                    _patch.UnpatchAll();
                    Logger.Debug("Deactivated Harmony patches");
                }
            } catch (Exception ex)
            {
                Logger.Error("Failed to deactivate Harmony patches!");
                Logger.Ex(ex);
            }
        }
    }
}

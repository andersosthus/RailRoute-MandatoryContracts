using Game;
using Game.Contract;
using Game.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace MandatoryContracts.Contracts
{
    public class MandatoryContractsController : MonoBehaviour
    {
        protected MandatoryContractsController BaseComponent { get; private set; }
        protected GameController GameController { get; private set; }
        protected ContractController ContractController { get; private set; }
        protected ITimeController TimeController { get; private set; }
        protected IStationRepository StationRepository { get; private set; }

        private TimeSpan previousExecution;

        internal static List<MandatoryContract> NonRejectables = new List<MandatoryContract>();

        public virtual void PostAwake(MandatoryContractsController mandatoryController, GameController gameController)
        {
            BaseComponent = mandatoryController;
            GameController = gameController;
            ContractController = gameController.ContractController;
            TimeController = gameController.TimeController;
            StationRepository = gameController.GetStationRepository();

            NonRejectables.Clear();

            Logger.Info("Initialized controller");
        }

        internal static MandatoryContractsController Attach(GameController gameController)
        {
            var mandatoryController = gameController.gameObject.AddComponent<MandatoryContractsController>();
            mandatoryController.PostAwake(mandatoryController, gameController);

            return mandatoryController;
        }

        private void FixedUpdate()
        {
            if(!GameController.Loaded)
            {
                return;
            }

            if(GameController.LevelController == null || GameController.LevelController.CurrentLevel == null || GameController.LevelController.CurrentLevel.LevelDefinition == null || !GameController.LevelController.CurrentLevel.LevelDefinition.Endless)
            {
                return;
            }

            if(TimeController.CurrentTime.Seconds != previousExecution.Seconds)
            {
                previousExecution = TimeController.CurrentTime;

                // Check if we got any contracts that needs to be accepted
                NonRejectables.ForEach(x =>
                {
                    if(x.Contract.Status == ContractStatus.NEW && TimeController.CurrentTime >= x.AutoAcceptAt)
                    {
                        Logger.Debug($"AutoAcceptedAt {TimeController.CurrentTime}");
                        ContractController.AcceptContract(x.Contract);
                    }
                });

                // Generate a new contract if we are so inclined
                if(UnityEngine.Random.value < 0.002)
                {
                    Logger.Debug("Generating mandatory contract!");
                    GenerateMandatoryContract();
                }
            }
        }

        private void GenerateMandatoryContract()
        {
            // Limit the amount of Mandatory contracts
            var activeStations = StationRepository.GetStations().Count(x => x.Active);
            var maxActiveContracts = activeStations <= 3 ? 2 : (activeStations - 1) * 2;
            if (NonRejectables.Count >= maxActiveContracts)
            {
                return;
            }

            var contractGenerator = ContractController.ContractGenerator;

            Contract contract;
            do
            {
                contract = contractGenerator.RandomContract();

            } while (KeepGeneratingContracts(contract));

            if(contract.Prototype != null)
            {
                foreach(var v in contract.Prototype.Vehicles)
                {
                    Logger.Debug($"Vehicle is of type: {v.GetChar()}");
                }
            }

            RandomizeContractSettings(contract);

            var mandatoryContract = new MandatoryContract(contract, TimeController.CurrentTime);
            NonRejectables.Add(mandatoryContract);

            ContractController.AddContract(contract);
            EventManager.TriggerEvent("newContractGenerated", contract);
        }

        private void RandomizeContractSettings(Contract contract)
        {
            contract.MandatoryCount = UnityEngine.Random.Range(5, 20);

            var distMath = 1000 * contract.RoutingResult.Distance * 1f;
            contract.PaymentPerTrainMaximum = MathUtils.GaussianRounded((int)distMath, 2000);
        }

        private bool KeepGeneratingContracts(Contract contract)
        {
            Logger.Debug("Checking contract");
            if(contract == null)
            {
                return true;
            }

            Logger.Debug("Checking reuse");
            if(contract.ReuseMode == ReuseMode.REUSE_EXIT || contract.ReuseMode == ReuseMode.REUSE_KEEP)
            {
                return true;
            }

            Logger.Debug("Checking mandatory");
            if (contract.MandatoryStop != null && !contract.MandatoryStop.Station.Active)
            {
                return true;
            }

            Logger.Debug("Checking origin and destination");
            if (!contract.Origin.Station.Active || !contract.Destination.Station.Active)
            {
                return true;
            }

            Logger.Debug("All ok");
            return false;
        }

        internal static bool IsContractMandatory(Contract contract)
        {
            return NonRejectables.Any(x => x.ContractID == contract.ContractNumber);
        }

        internal static MandatoryContract GetByContract(Contract contract)
        {
            return NonRejectables.Find(x => x.ContractID == contract.ContractNumber);
        }

        internal static bool RemoveMandatoryContract(Contract contract)
        {
            var mandatoryContract = GetByContract(contract);
            return NonRejectables.Remove(mandatoryContract);
        }
    }
}

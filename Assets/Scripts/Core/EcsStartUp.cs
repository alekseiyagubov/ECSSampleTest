using Data;
using Leopotam.EcsLite;
using SceneData;
using Systems;
using Systems.Core;
using UnityEngine;

namespace Core
{
    public sealed class EcsStartUp : MonoBehaviour, ITimeUpdater
    {
        [SerializeField] private LevelConfiguration _levelConfiguration;

        private EcsWorld _world;
        private EcsSystems _systems;

        public float DeltaTime => Time.deltaTime;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            var sharedData = CreateSharedData();
            InitializeSharedData(sharedData);
            
            _world = new EcsWorld();
            CreateSystems(sharedData);
            _systems.Init();
        }

        private SharedData CreateSharedData()
        {
            var sharedData = new SharedData
            {
                Gates = _levelConfiguration.Gates,
                Player = _levelConfiguration.Player,
                TimeUpdater = this
            };

            return sharedData;
        }

        private void InitializeSharedData(SharedData sharedData)
        {
            sharedData.GameConfiguration.PlayerSpeed = _levelConfiguration.PlayerSpeed;
            sharedData.GameConfiguration.GateOpeningTime = _levelConfiguration.GateOpeningTime;
            sharedData.PlayerAnimator = _levelConfiguration.PlayerAnimator;
            sharedData.GameConfiguration.OpenedGateOffset = _levelConfiguration.OpenedGateOffset;
        }

        private void CreateSystems(SharedData sharedData)
        {
            _systems = new EcsSystems(_world, sharedData);

            _systems.Add(new InputSystem());
            _systems.Add(new PlayerInitializeSystem());
            _systems.Add(new GatesInitializeSystem());
            _systems.Add(new PlayerMovementSystem());
            _systems.Add(new GateSystem());
            _systems.Add(new PlayerTransformUpdateSystem());
            _systems.Add(new PlayerAnimationSystem());
            _systems.Add(new GateTransformUpdateSystem());
        }

        private void Update()
        {
            _systems.Run();
        }
    }
}
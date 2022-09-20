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
            var sharedData = new SharedData();
            sharedData.Gates = _levelConfiguration.Gates;
            sharedData.Player = _levelConfiguration.Player;
            sharedData.TimeUpdater = this;
            sharedData.GameConfiguration.PlayerSpeed = _levelConfiguration.PlayerSpeed;
            sharedData.GameConfiguration.GateOpeningTime = _levelConfiguration.GateOpeningTime;
            sharedData.PlayerAnimator = _levelConfiguration.PlayerAnimator;
            sharedData.GameConfiguration.OpenedGateOffset = _levelConfiguration.OpenedGateOffset;
            
            _world = new EcsWorld();
            _systems = new EcsSystems(_world, sharedData);
            
            _systems.Add(new InputSystem());
            _systems.Add(new PlayerInitializeSystem());
            _systems.Add(new GatesInitializeSystem());
            _systems.Add(new PlayerMovementSystem());
            _systems.Add(new GateSystem());
            _systems.Add(new PlayerTransformUpdateSystem());
            _systems.Add(new PlayerAnimationSystem());
            _systems.Add(new GateTransformUpdateSystem());
            
            _systems.Init();
        }

        private void Update()
        {
            _systems.Run();
        }
    }
}
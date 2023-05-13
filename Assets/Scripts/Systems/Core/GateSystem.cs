using Components.Core;
using Core;
using Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems.Core
{
    public class GateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _playerPositionFilter;
        private EcsPool<PlayerPosition> _playerPositionPool;
        private EcsFilter _gateFilter;
        private EcsPool<Gate> _gatesPool;
        private float _gateOpeningTime;
        private ITimeUpdater _timeUpdater;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerPositionFilter = _world.Filter<PlayerPosition>().End();
            _playerPositionPool = _world.GetPool<PlayerPosition>();

            _gateFilter = _world.Filter<Gate>().End();
            _gatesPool = _world.GetPool<Gate>();

            var sharedData = systems.GetShared<SharedData>();
            _gateOpeningTime = sharedData.GameConfiguration.GateOpeningTime;
            _timeUpdater = sharedData.TimeUpdater;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var playerEntity in _playerPositionFilter)
            {
                ref var playerPositionComponent = ref _playerPositionPool.Get(playerEntity);

                foreach (var gateEntity in _gateFilter)
                {
                    UpdateGateState(playerPositionComponent.Position, gateEntity);
                }
            }
        }

        private void UpdateGateState(Vector3 playerPosition, int gateEntity)
        {
            ref var gateComponent = ref _gatesPool.Get(gateEntity);
            var isPlayerOnButton = CheckPlayerOnButton(playerPosition, gateComponent.ButtonPosition);
            if (isPlayerOnButton)
            {
                gateComponent.OpeningProgress += _timeUpdater.DeltaTime / _gateOpeningTime;
            }
        }

        private static bool CheckPlayerOnButton(Vector3 playerPosition, Vector3 buttonPosition)
        {
            var playerPosition2D = new Vector2(playerPosition.x, playerPosition.z);
            var buttonPosition2D = new Vector2(buttonPosition.x, buttonPosition.z);
            return Vector2.Distance(playerPosition2D, buttonPosition2D) < 0.5f;
        }
    }
}
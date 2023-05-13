using Components;
using Components.Core;
using Core;
using Data;
using Leopotam.EcsLite;
using UnityEngine;
using Input = Components.Input;

namespace Systems.Core
{
    public class PlayerMovementSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _playerPositionFilter;
        private EcsFilter _inputFilter;
        private EcsPool<PlayerPosition> _playerPositionPool;
        private EcsPool<Input> _inputPool;
        private EcsPool<MovingFlag> _movingFlagPool;
        private float _playerSpeed;
        private ITimeUpdater _timeUpdater;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerPositionFilter = _world.Filter<PlayerPosition>().End();
            _playerPositionPool = _world.GetPool<PlayerPosition>();
            _inputPool = _world.GetPool<Input>();
            _inputFilter = _world.Filter<Input>().End();

            _movingFlagPool = _world.GetPool<MovingFlag>();

            var sharedData = systems.GetShared<SharedData>();
            _playerSpeed = sharedData.GameConfiguration.PlayerSpeed;
            _timeUpdater = sharedData.TimeUpdater;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var playerEntity in _playerPositionFilter)
            {
                foreach (var inputEntity in _inputFilter)
                {
                    UpdatePosition(playerEntity, inputEntity);
                }
            }
        }

        private void UpdatePosition(int playerEntity, int inputEntity)
        {
            ref var input = ref _inputPool.Get(inputEntity);
            ref var position = ref _playerPositionPool.Get(playerEntity);

            var inputPosition = new Vector3(input.Position.x, position.Position.y, input.Position.z);
            var nextPosition = CalculatePosition(position.Position, inputPosition);

            ref var movingFlagComponent = ref _movingFlagPool.Get(playerEntity);

            if (Vector3.Distance(position.Position, nextPosition) < 0.01f)
            {
                movingFlagComponent.IsMoving = false;
                return;
            }

            movingFlagComponent.IsMoving = true;
            position.Position = nextPosition;
        }

        private Vector3 CalculatePosition(Vector3 currentPosition, Vector3 targetPosition)
        {
            var progress = 1 / Vector3.Distance(currentPosition, targetPosition);
            return Vector3.Lerp(currentPosition, targetPosition, progress * _timeUpdater.DeltaTime * _playerSpeed);
        }
    }
}
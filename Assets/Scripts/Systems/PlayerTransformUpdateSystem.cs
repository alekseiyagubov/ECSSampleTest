using Components;
using Components.Core;
using Leopotam.EcsLite;

namespace Systems
{
    public class PlayerTransformUpdateSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsPool<PlayerPosition> _positionPool;
        private EcsFilter _playerTransformFilter;
        private EcsPool<PlayerTransform> _transformPool;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerTransformFilter = _world.Filter<PlayerTransform>().Inc<PlayerPosition>().End();
            _positionPool = _world.GetPool<PlayerPosition>();
            _transformPool = _world.GetPool<PlayerTransform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var playerEntity in _playerTransformFilter)
            {
                UpdatePlayerTransform(playerEntity);
            }
        }

        private void UpdatePlayerTransform(int playerEntity)
        {
            ref var positionComponent = ref _positionPool.Get(playerEntity);
            ref var playerTransform = ref _transformPool.Get(playerEntity);

            var targetPosition = positionComponent.Position;
            if (targetPosition == default)
            {
                return;
            }

            var playerTransformPosition = playerTransform.Transform.position;

            var direction = targetPosition - playerTransformPosition;
            if (direction != default)
            {
                playerTransform.Transform.forward = direction;
            }

            playerTransform.Transform.position = targetPosition;
        }
    }
}
using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Animation = Components.Animation;

namespace Systems
{
    public class PlayerAnimationSystem : IEcsRunSystem, IEcsInitSystem
    {
        private static readonly int AnimatorIsRunParameter = Animator.StringToHash("IsMoving");
        
        private EcsFilter _animationFilter;
        private EcsPool<Animation> _animationPool;
        private EcsFilter _playerMovingFilter;
        private EcsPool<MovingFlag> _movingFlagPool;

        private bool _isMoving;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _playerMovingFilter = world.Filter<MovingFlag>().End();
            _movingFlagPool = world.GetPool<MovingFlag>();

            _animationFilter = world.Filter<Animation>().End();
            _animationPool = world.GetPool<Animation>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerMovingFilter)
            {
                ref var movingComponent = ref _movingFlagPool.Get(entity);
                if (movingComponent.IsMoving == _isMoving)
                {
                    continue;
                }
                
                _isMoving = movingComponent.IsMoving;
                    
                foreach (var animationEntity in _animationFilter)
                {
                    var animator = _animationPool.Get(animationEntity);
                    animator.Animator.SetBool(AnimatorIsRunParameter, _isMoving);
                }
            }
        }
    }
}
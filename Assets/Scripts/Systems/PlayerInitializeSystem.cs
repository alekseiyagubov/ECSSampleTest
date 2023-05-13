﻿using Components;
using Components.Core;
using Data;
using Leopotam.EcsLite;
using Animation = Components.Animation;

namespace Systems
{
    public class PlayerInitializeSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var world = systems.GetWorld();
            var playerEntity = world.NewEntity();

            InitPlayerTransform(world, playerEntity, sharedData);
            InitPlayerAnimation(world, playerEntity, sharedData);
        }

        private void InitPlayerTransform(EcsWorld world, int playerEntity, SharedData sharedData)
        {
            var playerTransformPool = world.GetPool<PlayerTransform>();
            ref var playerTransformComponent = ref playerTransformPool.Add(playerEntity);
            playerTransformComponent.Transform = sharedData.Player;

            var playerPositionPool = world.GetPool<PlayerPosition>();
            ref var playerPositionComponent = ref playerPositionPool.Add(playerEntity);
            var playerStartPosition = playerTransformComponent.Transform.position;
            playerPositionComponent.Position = playerStartPosition;

            var playerMovingPool = world.GetPool<MovingFlag>();
            playerMovingPool.Add(playerEntity);
        }

        private void InitPlayerAnimation(EcsWorld world, int playerEntity, SharedData sharedData)
        {
            var animationPool = world.GetPool<Animation>();
            ref var animationComponent = ref animationPool.Add(playerEntity);
            animationComponent.Animator = sharedData.PlayerAnimator;
        }
    }
}
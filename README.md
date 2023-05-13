Test task for introduction to basics of ECS and for learning for the right code decomposition to components and systems.

It's must be 3d isometric top down gameplay, the character must be controlled through the point and click input.

The scene must contains several couples of doors-buttons (one button for each one door).  While the player will be stay over the button - the door must be in opening process (it's mean that the door must be open smoothly, not instantly). If player will leave the button before the opening process finished, the door must stay in the leaved position.

The main game systems must be independent form Unity code. It's mean that this systems must be compatible for example for authoritative server gameplay simulation in the console .net project. But it's okay to use unity's Vector3 structure and Mathf library.

It's enough only 2 animations for character: Idle and Move.

Systems which could be enough to simulate real time gameplay on the authoritative server:
- GateSystem
- PlayerMovementSystem
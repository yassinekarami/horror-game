using System;
using Unity.Behavior;

[BlackboardEnum]
public enum EnemyState
{
	Idle,
	Hit,
	Patrol,
	Chase,
	Attack,
	Dead,
	Trigger
}

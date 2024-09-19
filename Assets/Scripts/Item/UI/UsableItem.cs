using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UsableItem
{
    ItemData data { get;}
    public bool UseItem(LivingEntity livingEntity);
    public IState GetTargetState(PlayerMovementStateMachine stateMachine);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface UsableItem
{
    ItemDataSO data { get;}
    public bool UseItem(LivingEntity livingEntity);
    public IState GetTargetState(PlayerMovementStateMachine stateMachine);

    public event Action OnUseItem;
}

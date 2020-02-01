#if PLANNER_ACTIONS_GENERATED
using System;
using UnityEngine;
using DefenderNameSpace;

public class Explore: StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var defender2 = animator.gameObject.GetComponent<Defender>();
        var action2 = (RandomNavigateAction)defender2.CurrentOperationalAction;
        defender2.CompleteAction();
        action2.AnimationComplete = true;
        
    }
}
#endif
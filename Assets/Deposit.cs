using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefenderNameSpace;

public class Deposit : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
#if PLANNER_DOMAIN_GENERATED
        var defender = animator.gameObject.GetComponent<Defender>();
        //Debug.Log("Just Deposited");
        defender.CompleteAction();
        
#endif
    }
}

using System;
using UnityEngine;
using DefenderNameSpace;

public class SpawnState : StateMachineBehaviour
{
    public GameObject baby;
    protected GameObject spawnedBaby;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spawnedBaby = Instantiate(baby, animator.rootPosition, Quaternion.identity) as GameObject;
        
       
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
#if PLANNER_DOMAIN_GENERATED
        var defender = animator.gameObject.GetComponent<Defender>();

        defender.CompleteAction();
        
#endif
    }
}

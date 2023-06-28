using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeNextStateBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private int _maxLoops = 5;
    
    [SerializeField]
    [Range(0, 100)]
    private int _percentChance = 10;
    
    [SerializeField]
    private int _secBetweenTransitionChecks = 1;

    [SerializeField]
    private List<string> _idleBehavioursList;

    private int _loopCounter = 0;
    private int _oldLoopCounter = 0;

    private float _timer = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _oldLoopCounter = 0;
        _loopCounter = 0;
    } 

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _loopCounter = (int)stateInfo.normalizedTime;

        if (_loopCounter >= _maxLoops)
        {
            TransitionToState(animator);
        } else
        {
            _timer += Time.deltaTime;

            //CheckTransitionOnLoop(animator, stateInfo);

            if (_timer > _secBetweenTransitionChecks)
            {
                _timer = 0;
                CheckTransition(animator);
            }
        }
    }

    private void CheckTransition(Animator animator)
    {
        if (Random.Range(0, 100) < _percentChance)
        {
            TransitionToState(animator);
        }
    }

    private void CheckTransitionOnLoop(Animator animator, AnimatorStateInfo stateInfo)
    {
        // If we just looped
        if (_oldLoopCounter != _loopCounter)
        {
            _oldLoopCounter = _loopCounter;

            //Debug.Log("_loopCounter: " + _loopCounter);
            if (Random.Range(0, _maxLoops - _loopCounter) == 0)
            {
                TransitionToState(animator);
            }
        }
    }

    private void TransitionToState(Animator animator)
    {
        string triggerName = _idleBehavioursList[Random.Range(0, _idleBehavioursList.Count)];

        animator.SetTrigger(triggerName);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}

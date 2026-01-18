using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayerGuideTrigger : PlayerTrigger
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private GuideRobotStateMachine _stateMachine;
    [SerializeField] private string _text;
    [SerializeField] private Transform _targetGuidePoint;
    
    public override void OnTriggered()
    {
        _videoPlayer.Play();
        _stateMachine.Speak(_text, _targetGuidePoint);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            _stateMachine.CurrentState.CanChanged = true;
            _stateMachine.Move();
            _videoPlayer.Stop();
            if (DestroyGameObjectAfterTriggered)
                Destroy(gameObject);
            if (DestroyComponentAfterTriggered)
                Destroy(this);
        }
    }
}

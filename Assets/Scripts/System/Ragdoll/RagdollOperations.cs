using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NTC.Global.System
{
    public class RagdollOperations : RigidbodyArrayOperations
    {
        [Header("Ragdoll Settings")] 
        [SerializeField] private string ragdollLayer = "Ragdoll";
        
        [Header("Character Joints")]
        [SerializeField] private bool enableProjection = true;
        [SerializeField] private bool enablePreprocessing = true;
        [SerializeField, Min(0f)] private float projectionDistance = DefaultProjectionDistance;
        [SerializeField, Min(0f)] private float projectionAngle = DefaultProjectionAngle;
        [Space]
        [SerializeField] private List<CharacterJoint> characterJoints = new (16);

        public IReadOnlyList<CharacterJoint> CharacterJoints => characterJoints;

        private const string TryFixJitterMethodTitle = "Try Fix Ragdoll Jitter And Artifacts";
        private const float DefaultProjectionDistance = 0.1f;
        private const float DefaultProjectionAngle = 180f;
        
        protected override void OnRefresh()
        {
            GetComponentsInChildren(characterJoints);
        }

        protected override void OnApply()
        {
            SetProjectionStatus(enableProjection);
            
            SetPreprocessingStatus(enablePreprocessing);

            if (enableProjection)
            {
                SetProjectionDistance(projectionDistance);

                SetProjectionAngle(projectionAngle);
            }
            
#if UNITY_EDITOR
            Debug.Log("Parameters applied to ragdoll.");
#endif
        }

        [ContextMenu(TryFixJitterMethodTitle)]
        [Button(TryFixJitterMethodTitle)]
        public void TryFixJitterAndArtifacts()
        {
            EnableProjection();
            
            EnablePreprocessing();

            SetProjectionDistance(DefaultProjectionDistance);

            SetProjectionAngle(DefaultProjectionAngle);

            if (ragdollLayer != string.Empty)
            {
                var layer = LayerMask.NameToLayer(ragdollLayer);
                
                Physics.IgnoreLayerCollision(layer, layer, true);
                
#if UNITY_EDITOR
                Debug.Log("Collision Matrix was changed for ragdoll optimization. Please, check just in case");
#endif
            }
        }

        public void EnableRagdoll()
        {
            DisableKinematic();
            EnableGravity();
        }
        
        public void DisableRagdoll()
        {
            EnableKinematic(); 
            DisableGravity();
        }

        public void SetProjectionStatus(bool status)
        {
            characterJoints.ForEach(joint => joint.enableProjection = status);
        }

        public void SetPreprocessingStatus(bool status)
        {
            characterJoints.ForEach(joint => joint.enablePreprocessing = status);
        }

        public void SetProjectionDistance(float distance)
        {
            characterJoints.ForEach(joint => joint.projectionDistance = distance);
        }
        
        public void SetProjectionAngle(float angle)
        {
            characterJoints.ForEach(joint => joint.projectionAngle = angle);
        }

        public void EnableProjection() => SetProjectionStatus(true);
        public void DisableProjection() => SetProjectionStatus(false);
        public void EnablePreprocessing() => SetPreprocessingStatus(true);
        public void DisablePreprocessing() => SetPreprocessingStatus(false);
    }
}
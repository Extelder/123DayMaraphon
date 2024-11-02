using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NTC.Global.System
{
    public class RigidbodyArrayOperations : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] private RigidbodyInterpolation rigidbodyInterpolation = RigidbodyInterpolation.None;
        [SerializeField] private CollisionDetectionMode collisionDetectionMode = CollisionDetectionMode.Discrete;
        [SerializeField] private bool kinematic;
        [SerializeField] private bool useGravity = true;
        
        [Header("Solver (Applies on awake)")]
        [SerializeField] private bool applySolverIterations;
        [SerializeField, Min(0)] private int solverIterations = 6;
        [SerializeField, Min(0)] private int solverVelocityIterations = 1;

        [Header("Mass")] 
        [SerializeField] private bool applyMassSettings;
        [SerializeField, Min(0f)] private float totalMass = 60f;
        [SerializeField, Min(0f)] private float massMultiplier = 1f;
        
        [Header("Other")]
        [SerializeField] private bool unfreezeAllRigidbodies;
        
        [Header("Invocation Settings")]
        [SerializeField] private bool refreshOnAwake;
        [SerializeField] private bool applyOnAwake;

        [Header("External Dependencies")]
        [SerializeField] private List<Rigidbody> rigidbodies = new List<Rigidbody>();

        public IReadOnlyList<Rigidbody> Rigidbodies => rigidbodies;

        public const string RefreshComponentsMethodTitle = "Refresh Components";
        public const string ApplyMethodTitle = "Apply";
        
        private void Awake()
        {
            if (refreshOnAwake)
            {
                RefreshComponents();
            }

            if (applySolverIterations)
            {
                SetSolverIterations(solverIterations);
                SetVelocitySolverIterations(solverVelocityIterations);
            }
            
            if (applyOnAwake)
            {
                ApplyAll();
            }
        }
        
        [ContextMenu(RefreshComponentsMethodTitle)]
        [Button(RefreshComponentsMethodTitle)]
        public void RefreshComponents()
        {
            GetComponentsInChildren(rigidbodies);
            
            OnRefresh();
        }

        [ContextMenu(ApplyMethodTitle)]
        [Button(ApplyMethodTitle)]
        public void ApplyAll()
        {
            if (rigidbodies == null)
                throw new Exception("Rigidbodies list is null! Try to refresh components.");

            if (rigidbodies.Count == 0)
                throw new Exception("Rigidbodies list is empty! Try to refresh components.");
            
            SetInterpolation(rigidbodyInterpolation);
            SetCollisionDetectionMode(collisionDetectionMode);
            SetKinematic(kinematic);
            SetGravity(useGravity);

            if (applyMassSettings)
            {
                AffectMassWithMultiplier();
            }

            if (unfreezeAllRigidbodies)
            {
                UnfreezeAll();
            }
            
#if UNITY_EDITOR
            Debug.Log("Parameters applied to rigidbodies.");
#endif
            
            OnApply();
        }
        
        public void AddForce(Vector3 direction, float force, ForceMode forceMode = ForceMode.Impulse)
        {
            rigidbodies.ForEach(rb => rb.AddForce(direction * force, forceMode));
        }
        
        public void AddExplosionForce(float force, Vector3 position, float radius, float upwardsMultiplier, 
            ForceMode forceMode = ForceMode.Impulse)
        {
            rigidbodies.ForEach(rb =>
            {
                rb.AddExplosionForce(force, position, radius, upwardsMultiplier, forceMode);
            });
        }

        public void SetSolverIterations(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            rigidbodies.ForEach(rb => rb.solverIterations = count);
        }

        public void SetVelocitySolverIterations(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            
            rigidbodies.ForEach(rb => rb.solverVelocityIterations = count);
        }

        public void SetKinematic(bool status)
        {
            rigidbodies.ForEach(rb => rb.isKinematic = status);
        }

        public void SetGravity(bool status)
        {
            rigidbodies.ForEach(rb => rb.useGravity = status);
        }

        public void SetInterpolation(RigidbodyInterpolation interpolation)
        {
            rigidbodies.ForEach(rb => rb.interpolation = interpolation);
        }
        
        public void SetCollisionDetectionMode(CollisionDetectionMode detectionMode)
        {
            rigidbodies.ForEach(rb => rb.collisionDetectionMode = detectionMode);
        }
        
        public void AffectMassWithMultiplier()
        {
            var count = rigidbodies.Count;
            
            if (count == 0)
                return;

            var massOnOnePart = totalMass / count * massMultiplier;
            
            rigidbodies.ForEach(rb => rb.mass = massOnOnePart);
        }
        
        public void UnfreezeAll()
        {
            rigidbodies.ForEach(rb => rb.constraints = RigidbodyConstraints.None);
        }
        
        public void EnableKinematic() => SetKinematic(true);
        public void DisableKinematic() => SetKinematic(false);
        public void EnableGravity() => SetGravity(true);
        public void DisableGravity() => SetGravity(false);
        public void Interpolate() => SetInterpolation(RigidbodyInterpolation.Interpolate);
        public void Extrapolate() => SetInterpolation(RigidbodyInterpolation.Extrapolate);
        public void DisableInterpolation() => SetInterpolation(RigidbodyInterpolation.None);
        
        public void SetDiscreteMode() => 
            SetCollisionDetectionMode(CollisionDetectionMode.Discrete);
        public void SetContinuousMode() => 
            SetCollisionDetectionMode(CollisionDetectionMode.Continuous);
        public void SetContinuousDynamicMode() => 
            SetCollisionDetectionMode(CollisionDetectionMode.ContinuousDynamic);
        public void SetContinuousSpeculativeMode() => 
            SetCollisionDetectionMode(CollisionDetectionMode.ContinuousSpeculative);

        protected virtual void OnRefresh() { }
        protected virtual void OnApply() { }
    }
}
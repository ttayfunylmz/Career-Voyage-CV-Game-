using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ArcadeVehicleController
{
    public class Vehicle : MonoBehaviour
    {
        private class SpringData
        {
            public float currentLength;
            public float currentVelocity;
        }

        private static readonly Wheel[] wheels = new Wheel[]
        {
            Wheel.FrontLeft, Wheel.FrontRight, Wheel.BackLeft, Wheel.BackRight
        };

        private static readonly Wheel[] frontWheels = new Wheel[] { Wheel.FrontLeft, Wheel.FrontRight };
        private static readonly Wheel[] backWheels = new Wheel[] { Wheel.BackLeft, Wheel.BackRight };

        public event Action OnVehicleFix;

        [Header("References")]
        [SerializeField] private VehicleSettings vehicleSettings;

        [Header("Settings")]
        [SerializeField] private float animationDuration = 1f;
        [SerializeField] private float upwardMovementDistance = 2f;
        [SerializeField] private Ease animationEase;
        [SerializeField] private bool isFlipping;

        private Transform vehicleTransform;
        private BoxCollider vehicleCollider;
        private Rigidbody vehicleRigidbody;
        private Dictionary<Wheel, SpringData> springDatas;

        private float steerInput;
        private float accelerateInput;


        public VehicleSettings Settings => vehicleSettings;
        public Vector3 Forward => vehicleTransform.forward;
        public Vector3 Velocity => vehicleRigidbody.velocity;

        private void Awake()
        {
            vehicleTransform = transform;
            InitializeCollider();
            InitializeBody();

            springDatas = new Dictionary<Wheel, SpringData>();
            foreach (Wheel wheel in wheels)
            {
                springDatas.Add(wheel, new());
            }
        }

        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                FixTheVehicle();
            }
        }

        private void FixedUpdate()
        {
            UpdateSuspension();
            UpdateSteering();
            UpdateAccelerate();
            UpdateBrakes();
            UpdateAirResistance();
        }

        public void SetSteerInput(float steerInput)
        {
            this.steerInput = Mathf.Clamp(steerInput, -1.0f, 1.0f);
        }

        public void SetAccelerateInput(float accelerateInput)
        {
            this.accelerateInput = Mathf.Clamp(accelerateInput, -1.0f, 1.0f);
        }

        public float GetSpringCurrentLength(Wheel wheel)
        {
            return springDatas[wheel].currentLength;
        }

        private void InitializeCollider()
        {
            if (!TryGetComponent(out vehicleCollider))
            {
                vehicleCollider = gameObject.AddComponent<BoxCollider>();
            }

            vehicleCollider.center = Vector3.zero;
            vehicleCollider.size = new Vector3(vehicleSettings.Width, vehicleSettings.Height, vehicleSettings.Length);
            vehicleCollider.isTrigger = false;
            vehicleCollider.enabled = true;
        }

        private void InitializeBody()
        {
            if (!TryGetComponent(out vehicleRigidbody))
            {
                vehicleRigidbody = gameObject.AddComponent<Rigidbody>();
            }

            const int WHEELS_COUNT = 4;
            vehicleRigidbody.mass = vehicleSettings.ChassiMass + vehicleSettings.TireMass * WHEELS_COUNT;
            vehicleRigidbody.isKinematic = false;
            vehicleRigidbody.useGravity = true;
            vehicleRigidbody.drag = 0.0f;
            vehicleRigidbody.angularDrag = 0.0f;
            vehicleRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            vehicleRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            vehicleRigidbody.constraints = RigidbodyConstraints.None;
        }

        private void CastSpring(Wheel wheel)
        {
            Vector3 position = GetSpringPosition(wheel);

            float previousLength = springDatas[wheel].currentLength;

            float currentLength;

            if (Physics.Raycast(position, -vehicleTransform.up, out var hit, vehicleSettings.SpringRestLength))
            {
                currentLength = hit.distance;
            }
            else
            {
                currentLength = vehicleSettings.SpringRestLength;
            }

            springDatas[wheel].currentVelocity = (currentLength - previousLength) / Time.fixedDeltaTime;
            springDatas[wheel].currentLength = currentLength;
        }

        private Vector3 GetSpringRelativePosition(Wheel wheel)
        {
            Vector3 boxSize = vehicleCollider.size;
            float boxBottom = boxSize.y * -0.5f;

            float paddingX = vehicleSettings.WheelsPaddingX;
            float paddingZ = vehicleSettings.WheelsPaddingZ;

            return wheel switch
            {
                Wheel.FrontLeft => new Vector3(boxSize.x * (paddingX - 0.5f), boxBottom, boxSize.z * (0.5f - paddingZ)),
                Wheel.FrontRight => new Vector3(boxSize.x * (0.5f - paddingX), boxBottom, boxSize.z * (0.5f - paddingZ)),
                Wheel.BackLeft => new Vector3(boxSize.x * (paddingX - 0.5f), boxBottom, boxSize.z * (paddingZ - 0.5f)),
                Wheel.BackRight => new Vector3(boxSize.x * (0.5f - paddingX), boxBottom, boxSize.z * (paddingZ - 0.5f)),
                _ => default,
            };
        }

        private Vector3 GetSpringPosition(Wheel wheel)
        {
            return vehicleTransform.localToWorldMatrix.MultiplyPoint3x4(GetSpringRelativePosition(wheel));
        }

        private Vector3 GetSpringHitPosition(Wheel wheel)
        {
            Vector3 vehicleDown = -vehicleTransform.up;
            return GetSpringPosition(wheel) + springDatas[wheel].currentLength * vehicleDown;
        }

        private Vector3 GetWheelRollDirection(Wheel wheel)
        {
            bool frontWheel = wheel == Wheel.FrontLeft || wheel == Wheel.FrontRight;

            if (frontWheel)
            {
                var steerQuaternion = Quaternion.AngleAxis(steerInput * vehicleSettings.SteerAngle, Vector3.up);
                return steerQuaternion * vehicleTransform.forward;
            }
            else
            {
                return vehicleTransform.forward;
            }
        }

        private Vector3 GetWheelSlideDirection(Wheel wheel)
        {
            Vector3 forward = GetWheelRollDirection(wheel);
            return Vector3.Cross(vehicleTransform.up, forward);
        }

        private Vector3 GetWheelTorqueRelativePosition(Wheel wheel)
        {
            Vector3 boxSize = vehicleCollider.size;

            float paddingX = vehicleSettings.WheelsPaddingX;
            float paddingZ = vehicleSettings.WheelsPaddingZ;

            return wheel switch
            {
                Wheel.FrontLeft => new Vector3(boxSize.x * (paddingX - 0.5f), 0.0f, boxSize.z * (0.5f - paddingZ)),
                Wheel.FrontRight => new Vector3(boxSize.x * (0.5f - paddingX), 0.0f, boxSize.z * (0.5f - paddingZ)),
                Wheel.BackLeft => new Vector3(boxSize.x * (paddingX - 0.5f), 0.0f, boxSize.z * (paddingZ - 0.5f)),
                Wheel.BackRight => new Vector3(boxSize.x * (0.5f - paddingX), 0.0f, boxSize.z * (paddingZ - 0.5f)),
                _ => default,
            };

        }

        private Vector3 GetWheelTorquePosition(Wheel wheel)
        {
            return vehicleTransform.localToWorldMatrix.MultiplyPoint3x4(GetWheelTorqueRelativePosition(wheel));
        }

        private float GetWheelGripFactor(Wheel wheel)
        {
            bool frontWheel = wheel == Wheel.FrontLeft || wheel == Wheel.FrontRight;
            return frontWheel ? vehicleSettings.FrontWheelsGripFactor : vehicleSettings.RearWheelsGripFactor;
        }

        private bool IsGrounded(Wheel wheel)
        {
            return springDatas[wheel].currentLength < vehicleSettings.SpringRestLength;
        }

        private void UpdateSuspension()
        {
            foreach (Wheel id in springDatas.Keys)
            {
                CastSpring(id);
                float currentLength = springDatas[id].currentLength;
                float currentVelocity = springDatas[id].currentVelocity;

                float force = SpringMath.CalculateForceDamped(currentLength, currentVelocity,
                    vehicleSettings.SpringRestLength, vehicleSettings.SpringStrength,
                    vehicleSettings.SpringDamper);

                vehicleRigidbody.AddForceAtPosition(force * vehicleTransform.up, GetSpringPosition(id));
            }
        }

        private void UpdateSteering()
        {
            foreach (Wheel wheel in wheels)
            {
                if (!IsGrounded(wheel))
                {
                    continue;
                }

                Vector3 springPosition = GetSpringPosition(wheel);

                Vector3 slideDirection = GetWheelSlideDirection(wheel);
                float slideVelocity = Vector3.Dot(slideDirection, vehicleRigidbody.GetPointVelocity(springPosition));

                float desiredVelocityChange = -slideVelocity * GetWheelGripFactor(wheel);
                float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;

                Vector3 force = desiredAcceleration * vehicleSettings.TireMass * slideDirection;
                vehicleRigidbody.AddForceAtPosition(force, GetWheelTorquePosition(wheel));
            }
        }

        private void UpdateAccelerate()
        {
            if (Mathf.Approximately(accelerateInput, 0.0f))
            {
                return;
            }

            float forwardSpeed = Vector3.Dot(vehicleTransform.forward, vehicleRigidbody.velocity);
            bool movingForward = forwardSpeed > 0.0f;
            float speed = Mathf.Abs(forwardSpeed);

            if (movingForward && speed > vehicleSettings.MaxSpeed)
            {
                return;
            }
            else if (!movingForward && speed > vehicleSettings.MaxReverseSpeed)
            {
                return;
            }

            foreach (Wheel wheel in wheels)
            {
                if (!IsGrounded(wheel))
                {
                    continue;
                }

                Vector3 position = GetWheelTorquePosition(wheel);
                Vector3 wheelForward = GetWheelRollDirection(wheel);
                vehicleRigidbody.AddForceAtPosition(accelerateInput * vehicleSettings.AcceleratePower * wheelForward, position);
            }
        }

        private void UpdateBrakes()
        {
            float forwardSpeed = Vector3.Dot(vehicleTransform.forward, vehicleRigidbody.velocity);
            float speed = Mathf.Abs(forwardSpeed);

            float brakesRatio;

            const float ALMOST_STOPPING_SPEED = 2.0f;
            bool almostStopping = speed < ALMOST_STOPPING_SPEED;
            if (almostStopping)
            {
                brakesRatio = 1.0f;
            }
            else
            {
                bool accelerateContrary =
                    !Mathf.Approximately(accelerateInput, 0.0f) &&
                    Vector3.Dot(accelerateInput * vehicleTransform.forward, vehicleRigidbody.velocity) < 0.0f;
                if (accelerateContrary)
                {
                    brakesRatio = 1.0f;
                }
                else if (Mathf.Approximately(accelerateInput, 0.0f)) // No accelerate input
                {
                    brakesRatio = 0.1f;
                }
                else
                {
                    return;
                }
            }

            foreach (Wheel wheel in backWheels)
            {
                if (!IsGrounded(wheel))
                {
                    continue;
                }

                Vector3 springPosition = GetSpringPosition(wheel);
                Vector3 rollDirection = GetWheelRollDirection(wheel);
                float rollVelocity = Vector3.Dot(rollDirection, vehicleRigidbody.GetPointVelocity(springPosition));

                float desiredVelocityChange = -rollVelocity * vehicleSettings.BrakesPower * brakesRatio;
                float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;

                Vector3 force = desiredAcceleration * vehicleSettings.TireMass * rollDirection;
                vehicleRigidbody.AddForceAtPosition(force, GetWheelTorquePosition(wheel));
            }
        }

        private void UpdateAirResistance()
        {
            vehicleRigidbody.AddForce(vehicleCollider.size.magnitude * vehicleSettings.AirResistance * -vehicleRigidbody.velocity);
        }

        private void FixTheVehicle()
        {
            if (isFlipping) { return; }

            vehicleCollider.enabled = false;
            vehicleRigidbody.useGravity = false;
            vehicleRigidbody.isKinematic = true;
            isFlipping = true;

            transform.DOMove(new Vector3(transform.position.x, transform.position.y + upwardMovementDistance, transform.position.z), animationDuration)
                .SetEase(animationEase)
                .OnComplete(() =>
                {
                    transform.DORotate(new Vector3(0f, transform.eulerAngles.y, 0f), animationDuration)
                            .SetEase(animationEase).SetUpdate(UpdateType.Fixed, false)
                            .OnComplete(() =>
                            {
                                DOTween.KillAll();
                                vehicleCollider.enabled = true;
                                vehicleRigidbody.useGravity = true;
                                vehicleRigidbody.isKinematic = false;
                                OnVehicleFix?.Invoke();
                            });
                })
                .SetUpdate(UpdateType.Fixed, false);
        }

        public void SetIsFlipping(bool value)
        {
            isFlipping = value;
        }

        public bool GetIsFlipping()
        {
            return isFlipping;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Vector3 vehicleDown = -transform.up;

                foreach (Wheel wheel in springDatas.Keys)
                {
                    // Spring
                    Vector3 position = GetSpringPosition(wheel);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(position, position + vehicleDown * vehicleSettings.SpringRestLength);
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(GetSpringHitPosition(wheel), Vector3.one * 0.08f);

                    // Wheel
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(position, GetWheelRollDirection(wheel));
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(position, GetWheelSlideDirection(wheel));
                }
            }
            else
            {
                if (vehicleSettings != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(transform.position,
                        new Vector3(
                            vehicleSettings.Width,
                            vehicleSettings.Height,
                            vehicleSettings.Length));
                }
            }
        }
#endif
    }
}
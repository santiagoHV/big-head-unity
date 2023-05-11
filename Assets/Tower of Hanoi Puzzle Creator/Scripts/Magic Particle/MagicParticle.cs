using UnityEngine;
using UnityEngine.Events;

namespace TowerOfHanoiPuzzle
{
    using TowerOfHanoiGeometry;

    /// <summary>
    /// This is the script you need to attach to an object
    /// if you want it to be a "Magic Particle", i.e., whatever
    /// you want to fly around the selected rod to indicate that it's selected.
    /// Usage example: create a sphere, attach this script, 
    /// save as a prefab. Then at the TowerOfHanoi 
    /// script inspector, under the "Magic Particles" section,
    /// set the "Particle" field to reference the newly created prefab.
    /// </summary>
    public class MagicParticle: MonoBehaviour
    {
        #region Inspector Fields
        public float radiusOffset = 0.05f;
        public float rodChangeAcceleration = 8;
        public FloatRange verticalSpeedRange = new FloatRange(0.3f, 0.6f);
        public FloatRange angularSpeedRange = new FloatRange(5f, 11f);
        public FloatRange fadeInDurationRange = new FloatRange(0.3f, 1f);
        public FloatRange fadeOutDurationRange = new FloatRange(0.2f, 1f);
        public UnityEventFloat FadingValueChanged;
        public bool fadeScale = true;
        public bool fadeTrailRenderer = false; 
        #endregion
        #region Motion
        #region Accessed by TowerOfHanoi class
        public int CurrentRod
        {
            get
            {
                return currentRod;
            }
            set
            {
                currentRod = value;
            }
        }
        public int TargetRod
        {
            get
            {
                return targetRod;
            }
            set
            {
                targetRod = value;
            }
        }

        public Transform[] RodsTransforms
        {
            get
            {
                return rodsTransforms;
            }
            set
            {
                rodsTransforms = value;
            }
        }
        public AnimationCurve[] StacksOutlines
        {
            get
            {
                return stacksOutlines;
            }
            set
            {
                stacksOutlines = value;
            }
        }

        public FloatRange VerticalPositionRange
        {
            get
            {
                return verticalPositionRange;
            }
            set
            {
                verticalPositionRange = value;
            }
        }

        public float VerticalSpeed
        {
            get
            {
                return verticalSpeed;
            }
            set
            {
                verticalSpeed = value;
            }
        }
        public FloatRange VerticalSpeedRange
        {
            get
            {
                return verticalSpeedRange;
            }
            set
            {
                verticalSpeedRange = value;
            }
        }
        public float TwistAngularSpeed
        {
            get
            {
                return twistAngularSpeed;
            }
            set
            {
                twistAngularSpeed = value;
            }
        }
        public FloatRange TwistAngularSpeedRange
        {
            get
            {
                return angularSpeedRange;
            }
            set
            {
                angularSpeedRange = value;
            }
        }

        public void RandomizeSpeeds()
        {
            verticalSpeed = RandomSign * verticalSpeedRange.Random;
            twistAngularSpeed = RandomSign * angularSpeedRange.Random;
        }
        public void RandomizePosition()
        {
            twistAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            verticalPosition = verticalPositionRange.Random;
            UpdateTransform();
        }
        public bool IsOrbitingTargetRod()
        {
            return !isChangingRod && currentRod == targetRod;
        }
        public float RodChangeAcceleration
        {
            get
            {
                return rodChangeAcceleration;
            }
            set
            {
                rodChangeAcceleration = Mathf.Abs(value);
            }
        }
        #endregion
        #region Other Fields
        int currentRod = 0;
        int targetRod = 0;
        Transform[] rodsTransforms;
        AnimationCurve[] stacksOutlines;
        float verticalPosition = 0;
        float verticalSpeed = 1;
        float twistAngle = 0;
        float twistAngularSpeed = 1;
        FloatRange verticalPositionRange = new FloatRange(0, 2);
        bool isChangingRod;
        float RandomSign
        {
            get
            {
                return UnityEngine.Random.Range(0, 2) == 0 ? 1f : -1f;
            }
        }
        #endregion
        #region Stack Change Movement
        Curve2DComposition changeTrajectory;
        float changeStartTime,
              changeStartSpeed,
              changeTrajectoryLength;

        float GetOnStackRadius(int stack)
        {
            return radiusOffset + stacksOutlines[stack].Evaluate(verticalPosition);
        }
        Vector3 CylindricalToCartesianCoordinates(float verticalPosition, float radius, float angle)
        {
            return new Vector3(radius * Mathf.Cos(angle), verticalPosition, radius * Mathf.Sin(angle));
        }
        Vector3 GetOnStackPosition(int stack)
        {
            return CylindricalToCartesianCoordinates(verticalPosition, GetOnStackRadius(stack), twistAngle);
        }
        Vector3 GetOnStackVelocity(int stack)
        {
            float drdt = (stacksOutlines[stack].Evaluate(verticalPosition + 0.0001f) - stacksOutlines[stack].Evaluate(verticalPosition - 0.0001f)) / 0.0002f * verticalSpeed;
            float radius = GetOnStackRadius(stack);
            float cos = Mathf.Cos(twistAngle),
                  sin = Mathf.Sin(twistAngle);

            return new Vector3(drdt * cos - radius * sin * twistAngularSpeed, verticalSpeed, drdt * sin + radius * cos * twistAngularSpeed);
        }
        Circle2D GetStackCircle(int stack, int referenceStack)
        {
            float x = rodsTransforms[stack].localPosition.x - rodsTransforms[referenceStack].localPosition.x;
            return new Circle2D(new Vector2(x, 0), GetOnStackRadius(stack) - 0.0001f);
        }
        Vector3 GetOnStackRelativePosition(int stack, int referenceStack)
        {
            float deltaX = rodsTransforms[stack].localPosition.x - rodsTransforms[referenceStack].localPosition.x;
            return Vector3.right * deltaX + CylindricalToCartesianCoordinates(verticalPosition, GetOnStackRadius(stack), twistAngle);
        }
        void UpdateCylindricalPositionFromCurrentTransform()
        {
            Vector3 P = transform.localPosition - rodsTransforms[currentRod].localPosition;
            twistAngle = Mathf.Atan2(P.z, P.x);
            verticalPosition = P.y;
        }
        ClampedParametricCurve<Vector2> AvoidCircle(TowerOfHanoiGeometry.Ray2D ray, Circle2D circle)
        {
            Vector2[] intersection = Compute.Intersection(ray, circle);
            if (intersection.Length < 2)
                return null;

            Vector2 R1 = intersection[0] - circle.Center,
                    R2 = intersection[1] - circle.Center;

            float maxAngle = Vector2.Angle(R1, R2) * Mathf.Deg2Rad;
            float startAngle = Vector2.Angle(R1, Vector2.right) * Mathf.Deg2Rad;
            if (R1.y < 0)
                startAngle = -startAngle;

            return new ClampedParametricCurve<Vector2>(new Circle2D(circle.Center, circle.Radius, startAngle, Compute.RotationDirection(R1, R2)), 0, maxAngle * circle.Radius);
        }
        Curve2DComposition BuildTrajectory(Vector2 from, Vector2 to, params Circle2D[] obstacleCircles)
        {
            Curve2DComposition curve = new Curve2DComposition();
            TowerOfHanoiGeometry.Ray2D ray = new TowerOfHanoiGeometry.Ray2D(from, (to - from).normalized);
            bool lastIntersected = false;
            for (int n = 0; n < obstacleCircles.Length; ++n)
            {
                var circle = AvoidCircle(ray, obstacleCircles[n]);
                if (circle != null)
                {
                    if (n == obstacleCircles.Length - 1)
                        lastIntersected = true;

                    float distance = Vector2.Distance(circle.Start, ray.Origin);
                    if (distance <= 0.00001f)
                    {
                        ray.Origin = circle.End;
                    }
                    else
                    {
                        curve.Add(ray, distance);
                        ray = new TowerOfHanoiGeometry.Ray2D(circle.End, ray.Direction);
                    }
                    curve.Add(circle, circle.Max);
                }
            }

            if (!lastIntersected && Vector2.Distance(to, ray.Origin) >= 0.00001f)
            {
                curve.Add(ray, (to - ray.Origin).magnitude);
            }
            return curve;
        }
        bool CanChangeStack()
        {
            const float theta = 0.35f;
            if (targetRod > currentRod)
            {
                if (twistAngularSpeed > 0)
                {
                    if (twistAngle < Mathf.PI)
                        return false;
                }
                else
                {
                    if (twistAngle > Mathf.PI)
                        return false;
                }
            }
            else
            {
                if (twistAngularSpeed < 0)
                {
                    if (twistAngle < Mathf.PI)
                        return false;
                }
                else
                {
                    if (twistAngle > Mathf.PI)
                        return false;
                }

            }
            return twistAngle > theta && twistAngle < Mathf.PI - theta ||
                   twistAngle > Mathf.PI + theta && twistAngle < 2 * Mathf.PI - theta;
        }
        void StartChangeMovement()
        {
            changeStartTime = Time.time;
            changeStartSpeed = GetOnStackVelocity(currentRod).magnitude;
            int stackDelta = Mathf.Abs(targetRod - currentRod);
            Vector3 currentPosition = GetOnStackPosition(currentRod);
            Vector2 A = new Vector2(currentPosition.x, currentPosition.z);
            Vector3 targetPosition = GetOnStackRelativePosition(targetRod, currentRod);
            Vector2 B = new Vector2(targetPosition.x, targetPosition.z);

            if (stackDelta == 1)
            {
                changeTrajectory = BuildTrajectory(A, B,
                                GetStackCircle(currentRod, currentRod),
                                GetStackCircle(targetRod, currentRod));
            }
            else
            {
                changeTrajectory = BuildTrajectory(A, B,
                                GetStackCircle(currentRod, currentRod),
                                GetStackCircle(1, currentRod),
                                GetStackCircle(targetRod, currentRod));
            }

            changeTrajectoryLength = changeTrajectory.Length;
            isChangingRod = true;
        }
        void UpdateChangeMovement()
        {
            float t = Time.time - changeStartTime;
            float s = (changeStartSpeed + rodChangeAcceleration * t / 2) * t;
            if (s > changeTrajectoryLength)
            {
                currentRod = targetRod;
                isChangingRod = false;
                UpdateCylindricalPositionFromCurrentTransform();
                FadeOutAndDestroy();
            }
            else
            {
                Vector2 position = changeTrajectory.GetPosition(s);
                Transform stackTransform = rodsTransforms[currentRod];
                transform.localPosition = stackTransform.localPosition +
                                          new Vector3(position.x, verticalPosition, position.y);
            }
        }
        #endregion
        #region Orbiting Movement
        void UpdateVerticalMovement()
        {
            verticalPosition += verticalSpeed * Time.deltaTime;
            if (verticalPosition < verticalPositionRange.Min)
            {
                verticalPosition = verticalPositionRange.Min;
                verticalSpeed = -verticalSpeed;
            }
            else if (verticalPosition > verticalPositionRange.Max)
            {
                verticalPosition = verticalPositionRange.Max;
                verticalSpeed = -verticalSpeed;
            }
        }
        void UpdateTwistMovement()
        {
            twistAngle += twistAngularSpeed * Time.deltaTime;
            twistAngle %= 2 * Mathf.PI;
            if (twistAngle < 0)
                twistAngle += 2 * Mathf.PI;
        }
        void UpdateTransform()
        {
            Transform stackTransform = rodsTransforms[currentRod];
            transform.localPosition = stackTransform.localPosition + GetOnStackPosition(currentRod);
        }
        void UpdateOrbitingMovement()
        {
            UpdateVerticalMovement();
            UpdateTwistMovement();
            UpdateTransform();
        }
        void UpdateMotion()
        {
            if (isChangingRod)
            {
                UpdateChangeMovement();
            }
            else
            {
                if (currentRod == targetRod)
                    UpdateOrbitingMovement();
                else if (CanChangeStack())
                    StartChangeMovement();
                else
                    UpdateOrbitingMovement();
            }
        }
        #endregion
        #endregion
        #region Fading Control
        bool isFading = false;
        float speed = 1,
              currentFadingValue = 1,
              targetFadingValue = 0;
        
        [System.Serializable]
        public class UnityEventFloat : UnityEvent<float> { }
        
        public float LossyScaleFactor
        {
            get
            {
                var parent = transform.parent;
                if (parent == null)
                    return 1;

                var lossyScale = parent.lossyScale;
                return (lossyScale.x + lossyScale.y + lossyScale.z) / 3f;
            }
        }

        float FadingDuration
        {
            set
            {
                if (value <= 0)
                    speed = float.PositiveInfinity;
                else
                    speed = Mathf.Log(1 / 0.03f) / value;
            }
        }
        float CurrentFadingValue
        {
            get
            {
                return currentFadingValue;
            }
            set
            {
                if (value != currentFadingValue)
                {
                    currentFadingValue = value;
                    FadingValueChanged.Invoke(currentFadingValue);
                }
            }
        }
        float TargetFadingValue
        {
            get
            {
                return targetFadingValue;
            }
            set
            {
                targetFadingValue = value;
                isFading = !isFading && targetFadingValue != currentFadingValue;
            }
        }

        public void FadeIn()
        {
            FadingDuration = fadeInDurationRange.Random;
            CurrentFadingValue = 0;
            TargetFadingValue = 1;
        }
        public void FadeOutAndDestroy()
        {
            FadingDuration = fadeOutDurationRange.Random;
            TargetFadingValue = 0;
            isFading = true;
        }

        void UpdateFading()
        {
            if (isFading)
            {
                float newValue = Mathf.Lerp(currentFadingValue, targetFadingValue, Time.deltaTime * speed);
                if (Mathf.Abs(targetFadingValue - newValue) <= 0.03f)
                {
                    isFading = false;
                    CurrentFadingValue = targetFadingValue;
                    if (targetFadingValue == 0)
                        Destroy(gameObject);
                }
                else
                {
                    CurrentFadingValue = newValue;
                }
            }
        }
        #endregion
        #region Scale Fading
        Vector3 maxScale;

        void ScaleFadingListener(float value)
        {
            transform.localScale = value * maxScale;
        }
        void AwakeScaleFading()
        {
            FadingValueChanged.AddListener(ScaleFadingListener);
            maxScale = transform.localScale;
        }
        #endregion
        #region Trail Renderer Fading
        TrailRenderer trailRenderer;
        float maxTime,
              maxStartWidth,
              maxEndWidth;

        void TrailFadingListener(float value)
        {
            trailRenderer.time = value * maxTime;
            trailRenderer.startWidth = value * maxStartWidth;
            trailRenderer.endWidth = value * maxEndWidth;
        }
        void AwakeTrailFading()
        {
            trailRenderer = GetComponent<TrailRenderer>();
            if (trailRenderer == null)
            {
                fadeTrailRenderer = false;
            }
            else
            {
                FadingValueChanged.AddListener(TrailFadingListener);
                maxTime = trailRenderer.time;
                maxStartWidth = trailRenderer.startWidth;
                maxEndWidth = trailRenderer.endWidth;
            }
        }
        void StartTrailFading()
        {
            float scale = LossyScaleFactor;
            maxStartWidth *= scale;
            maxEndWidth *= scale;
        }
        #endregion
        #region MonoBehaviour
        void Start()
        {
            if (rodsTransforms == null)
            {
                rodsTransforms = new Transform[3];
                for (int n = 0; n < 3; ++n)
                    rodsTransforms[n] = transform;
            }
            if (stacksOutlines == null)
            {
                stacksOutlines = new AnimationCurve[3];
                for (int n = 0; n < 3; ++n)
                    stacksOutlines[n] = new AnimationCurve();
            }

            if (fadeTrailRenderer)
                StartTrailFading();
        }
        void Awake()
        {
            if (fadeScale)
                AwakeScaleFading();

            if (fadeTrailRenderer)
                AwakeTrailFading();
        }
        void Update()
        {
            UpdateMotion();
            UpdateFading();
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerOfHanoiPuzzle
{
    /// <summary>
    /// The job of this class is to compose the disks
    /// animation from 3 pieces: getting out of the rod, 
    /// moving over the rods, and getting in another rod.
    /// </summary>
    class TowerOfHanoiDiskAnimation
    {
        #region Fields
        AnimationClip animationBetweenAdjacentRods,
                      animationBetweenExtremeRods,
                      currentAnimation;
        Vector2 startPosition, endPosition;
        const float dt = 0.001f;
        float onRodSpeed, 
              onRodAcceleration, 
              frameRateMultiplier = 1,
              rodHeight, diskHeight;

        Function upCurve, downCurve;
        MyTransform startTransform, endTransform;
        float upEndSpeed, upDistance, upDuration,
              downStartSpeed, downDistance, downDuration,
              duration;

        Vector3 betweenRodsAnimationScale;
        bool animationIsLeftToRight;
        MyTransform animationStartTransform, animationEndTransform;
        public static GameObject auxGo;
        bool updateTrajectory = true,
             useAnimationBetweenAdjacentRods = true;
        #endregion
        #region Geometry Properties
        public float RodHeight
        {
            get
            {
                return rodHeight;
            }
            set
            {
                if (rodHeight != value)
                {
                    rodHeight = value;
                    updateTrajectory = true;
                }
            }
        }
        public float DiskHeight
        {
            get
            {
                return diskHeight;
            }
            set
            {
                if (diskHeight != value)
                {
                    diskHeight = value;
                    updateTrajectory = true;
                }
            }
        }

        public Vector2 StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                if (startPosition != value)
                {
                    updateTrajectory = true;
                    startPosition = value;
                }
            }
        }
        public Vector2 EndPosition
        {
            get
            {
                return endPosition;
            }
            set
            {
                if (endPosition != value)
                {
                    updateTrajectory = true;
                    endPosition = value;
                }
            }
        }
        #endregion
        #region Animation Properties
        public float OnRodAcceleration
        {
            get
            {
                return onRodAcceleration;
            }
            set
            {
                if (onRodAcceleration != value)
                {
                    onRodAcceleration = value;
                    updateTrajectory = true;
                }
            }
        }
        public float OnRodSpeed
        {
            get
            {
                return onRodSpeed;
            }
            set
            {
                if (onRodSpeed != value)
                {
                    onRodSpeed = value;
                    updateTrajectory = true;
                }
            }
        }
        public bool UseAnimationBetweenAdjacentRods
        {
            get
            {
                return useAnimationBetweenAdjacentRods;
            }
            set
            {
                if (useAnimationBetweenAdjacentRods != value)
                {
                    useAnimationBetweenAdjacentRods = value;
                    updateTrajectory = true;
                }
            }
        }
        public AnimationClip AnimationBetweenAdjacentRods
        {
            get
            {
                return animationBetweenAdjacentRods;
            }
            set
            {
                if (animationBetweenAdjacentRods != value)
                {
                    animationBetweenAdjacentRods = value;
                    if (useAnimationBetweenAdjacentRods)
                        updateTrajectory = true;
                }
            }
        }
        public AnimationClip AnimationBetweenExtremeRods
        {
            get
            {
                return animationBetweenExtremeRods;
            }
            set
            {
                if (animationBetweenExtremeRods != value)
                {
                    animationBetweenExtremeRods = value;
                    if (!useAnimationBetweenAdjacentRods)
                        updateTrajectory = true;
                }
            }
        }
        public float Duration
        {
            get
            {
                return duration;
            }
        }
        public float FrameRateMultiplier
        {
            get
            {
                return frameRateMultiplier;
            }
            set
            {
                frameRateMultiplier = value;
            }
        }
        #endregion
        #region Nested Classes/Structures
        public struct MyTransform
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 PositionFromLocalToWorldSpace(Vector3 position)
            {
                return this.position + Quaternion.Inverse(rotation) * position;
            }
        }
        abstract class Function
        {
            public abstract float Eval(float x);
        }
        class PiecewiseFunction : Function
        {
            class Segment
            {
                public float x, y;
                public Function function;
                public float span;
            }
            List<Segment> segments = new List<Segment>();
            public void Add(Function function, float after)
            {
                Segment newSegment = new Segment();
                newSegment.function = function;
                newSegment.span = float.PositiveInfinity;
                if (segments.Count == 0)
                {
                    newSegment.x = after;
                    newSegment.y = 0;
                }
                else
                {
                    Segment last = segments.Last();
                    last.span = after;
                    newSegment.y = last.y + last.function.Eval(after);
                    newSegment.x = last.x + after;
                }

                segments.Add(newSegment);
            }
            public override float Eval(float x)
            {
                var first = segments.First();
                if (x < first.x)
                    return first.y;
                x -= first.x;

                foreach (var segment in segments)
                {
                    if (x <= segment.span)
                        return segment.y + segment.function.Eval(x);
                    x -= segment.span;
                }

                return 0;
            }
        }
        class Line : Function
        {
            public float a, b;
            public override float Eval(float x)
            {
                return a * x + b;
            }
            public Line(float a = 0, float b = 0)
            {
                this.a = a;
                this.b = b;
            }
        }
        class Parabola : Function
        {
            public float a = 0, b = 0, c = 0;
            public override float Eval(float x)
            {
                return (a * x + b) * x + c;
            }
            public Parabola(float a = 0, float b = 0, float c = 0)
            {
                this.a = a;
                this.b = b;
                this.c = c;
            }
        }
        #endregion
        #region Methods
        PiecewiseFunction GeneratePositionFunction(float startSpeed, float middleSpeed, float endSpeed, float acceleration, float distance, out float endTime)
        {
            PiecewiseFunction function = new PiecewiseFunction();
            float a0, a2;
            if (middleSpeed >= startSpeed)
                a0 = acceleration;
            else
                a0 = -acceleration;

            if (endSpeed >= middleSpeed)
                a2 = acceleration;
            else
                a2 = -acceleration;


            float v0sqr = startSpeed * startSpeed,
                  v1sqr = middleSpeed * middleSpeed,
                  v2sqr = endSpeed * endSpeed,
                  s0 = (v1sqr - v0sqr) / (2 * a0),
                  s2 = (v2sqr - v1sqr) / (2 * a2),
                  s1 = distance - s0 - s2;

            if (s1 > 0)
            {
                float t0 = (middleSpeed - startSpeed) / a0,
                      t1 = s1 / middleSpeed,
                      t2 = (endSpeed - middleSpeed) / a2;
                function.Add(new Parabola(a0 / 2, startSpeed), 0);
                function.Add(new Line(middleSpeed), t0);
                function.Add(new Parabola(a2 / 2, middleSpeed), t1);
                endTime = t0 + t1 + t2;
            }
            else if (Mathf.Sign(a0) == Mathf.Sign(a2))
            {
                acceleration = (v2sqr - v0sqr) / (2 * distance);
                function.Add(new Parabola(acceleration / 2, startSpeed), 0);
                endTime = (endSpeed - startSpeed) / acceleration;
            }
            else
            {
                v1sqr = (distance + v0sqr / (2 * a0) - v2sqr / (2 * a2)) / (1 / (2 * a0) - 1 / (2 * a2));
                if (v1sqr >= 0)
                {
                    middleSpeed = Mathf.Sqrt(v1sqr);
                    float t0 = (middleSpeed - startSpeed) / a0,
                          t2 = (endSpeed - middleSpeed) / a2;
                    function.Add(new Parabola(a0 / 2, startSpeed), 0);
                    function.Add(new Parabola(a2 / 2, middleSpeed), t0);
                    endTime = t0 + t2;
                }
                else
                {
                    acceleration = (v2sqr - v0sqr) / (2 * distance);
                    function.Add(new Parabola(acceleration / 2, startSpeed), 0);
                    endTime = (endSpeed - startSpeed) / acceleration;
                }
            }

            return function;
        }
        void UpdateTrajectory()
        {
            if (updateTrajectory)
            {
                updateTrajectory = false;

                //Start
                startTransform.position = new Vector3(startPosition.x, startPosition.y);
                startTransform.rotation = Quaternion.identity;

                //End
                endTransform.position = new Vector3(endPosition.x, endPosition.y);
                endTransform.rotation = Quaternion.identity;

                if (useAnimationBetweenAdjacentRods)
                    currentAnimation = animationBetweenAdjacentRods;
                else
                    currentAnimation = animationBetweenExtremeRods;
                
                if (currentAnimation == null)
                {
                    upDuration = 0;
                    downDuration = 0;
                    duration = 0;
                    return;
                }

                //Middle Animation
                animationStartTransform.position = new Vector3(startPosition.x, rodHeight + diskHeight / 2);
                animationEndTransform.position = new Vector3(endPosition.x, rodHeight + diskHeight / 2);
                animationStartTransform.rotation = Quaternion.identity;
                animationEndTransform.rotation = animationStartTransform.rotation;
                animationIsLeftToRight = endPosition.x > startPosition.x;
                
                currentAnimation.SampleAnimation(auxGo, currentAnimation.length);
                float scale = Mathf.Abs((endPosition.x - startPosition.x) / auxGo.transform.position.x);
                betweenRodsAnimationScale.Set(scale, scale, scale);

                //Up
                currentAnimation.SampleAnimation(auxGo, dt);
                upEndSpeed = scale * auxGo.transform.position.y / dt;
                upDistance = rodHeight + diskHeight / 2 - startPosition.y;
                upCurve = GeneratePositionFunction(0, onRodSpeed, upEndSpeed, onRodAcceleration, upDistance, out upDuration);
                
                //Down
                currentAnimation.SampleAnimation(auxGo, currentAnimation.length - dt);
                downStartSpeed = scale * auxGo.transform.position.y / dt;
                downDistance = rodHeight + diskHeight / 2 - endPosition.y;
                downCurve = GeneratePositionFunction(downStartSpeed, onRodSpeed, 0, onRodAcceleration, downDistance, out downDuration);
                
                //Duration
                duration = (upDuration + downDuration + currentAnimation.length) / FrameRateMultiplier;
            }
        }
        MyTransform GetUpTransform(float time)
        {
            if (time <= 0)
                return startTransform;

            if (time < upDuration)
            {
                MyTransform result = startTransform;
                result.position.y += upCurve.Eval(time);
                return result;
            }

            return animationStartTransform;
        }
        MyTransform GetAnimationTransform(float time)
        {
            MyTransform myTransform = new MyTransform();
            auxGo.transform.rotation = Quaternion.identity;
            currentAnimation.SampleAnimation(auxGo, time);
            if (animationIsLeftToRight)
            {
                myTransform.position = animationStartTransform.PositionFromLocalToWorldSpace(Vector3.Scale(auxGo.transform.position, betweenRodsAnimationScale));
                myTransform.rotation = animationStartTransform.rotation * auxGo.transform.rotation;
            }
            else
            {
                myTransform.position = animationStartTransform.PositionFromLocalToWorldSpace(Vector3.Scale(new Vector3(-auxGo.transform.position.x, auxGo.transform.position.y, -auxGo.transform.position.z), betweenRodsAnimationScale));
                Vector3 aux = auxGo.transform.rotation.eulerAngles;
                myTransform.rotation = animationStartTransform.rotation * Quaternion.Euler(-aux.x, aux.y, -aux.z);
            }
            return myTransform;
        }
        MyTransform GetDownTransform(float time)
        {
            if (time <= 0)
                return animationEndTransform;

            if (time < downDuration)
            {
                MyTransform result = endTransform;
                result.position.y += (downDistance - downCurve.Eval(time));
                return result;
            }

            return endTransform;
        }
        public MyTransform GetTransform(float time)
        {
            UpdateTrajectory();
            if (time <= 0) return startTransform;
            if (currentAnimation != null)
            {
                time *= frameRateMultiplier;
                if (time <= upDuration) return GetUpTransform(time);
                time -= upDuration;
                if (time <= currentAnimation.length) return GetAnimationTransform(time);
                time -= currentAnimation.length;
                if (time <= downDuration) return GetDownTransform(time);
            }
            return endTransform;
        }
        public TowerOfHanoiDiskAnimation()
        {
            if (auxGo == null)
                auxGo = new GameObject("Hanoi Tower Disks Animation");
        }
        #endregion
    }
}

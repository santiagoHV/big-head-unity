using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TowerOfHanoiPuzzle
{
    /// <summary>
    /// Auxiliary geometric objects and computations.
    /// </summary>
    namespace TowerOfHanoiGeometry
    {
        abstract class ParametricCurve<Out>
        {
            abstract public Out GetPosition(float s);
            abstract public Out GetTangent(float s);
        }

        class Circle2D : ParametricCurve<Vector2>
        {
            public Vector2 Center;
            public float Radius;
            public float StartAngle;
            public float TurnDirection;
            public override Vector2 GetPosition(float s)
            {
                float angle = StartAngle + TurnDirection * s / Radius;
                return Center + new Vector2(Radius * Mathf.Cos(angle), Radius * Mathf.Sin(angle));
            }
            public override Vector2 GetTangent(float s)
            {
                float angle = StartAngle + TurnDirection * s / Radius;
                return new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle));
            }
            public Circle2D(Vector2 center = default(Vector2), float radius = 1, float startAngle = 0, float turnDirection = 1)
            {
                Center = center;
                Radius = radius;
                StartAngle = startAngle;
                TurnDirection = turnDirection;
            }
        }
        class Line2D : ParametricCurve<Vector2>
        {
            public Vector2 Origin, Direction;

            public override Vector2 GetPosition(float s)
            {
                return Origin + Direction * s;
            }
            public override Vector2 GetTangent(float s)
            {
                return Direction;
            }

            public Line2D()
            {
                Origin = Vector2.zero;
                Direction = Vector2.right;
            }
            public Line2D(Vector2 start, Vector2 direction)
            {
                Origin = start;
                Direction = direction;
            }
        }
        class Ray2D : ParametricCurve<Vector2>
        {
            public Vector2 Origin, Direction;

            public override Vector2 GetPosition(float s)
            {
                if (s <= 0)
                    return Origin;
                return Origin + Direction * s;
            }
            public override Vector2 GetTangent(float s)
            {
                return Direction;
            }

            public Ray2D()
            {
                Origin = Vector2.zero;
                Direction = Vector2.right;
            }
            public Ray2D(Vector2 start, Vector2 direction)
            {
                Origin = start;
                Direction = direction;
            }
        }
        class Curve2DComposition : ParametricCurve<Vector2>
        {
            class Piece
            {
                public ParametricCurve<Vector2> Curve;
                public float Length;

                public Piece(ParametricCurve<Vector2> curve, float length)
                {
                    Curve = curve;
                    Length = length;
                }
            }
            List<Piece> Pieces = new List<Piece>();
            public float Length
            {
                get
                {
                    return Pieces.Sum(x => x.Length);
                }
            }
            public Vector2 Start
            {
                get
                {
                    return Pieces.First().Curve.GetPosition(0);
                }
            }
            public Vector2 End
            {
                get
                {
                    var lastPiece = Pieces.Last();
                    return lastPiece.Curve.GetPosition(lastPiece.Length);
                }
            }
            public Vector2 StartTangent
            {
                get
                {
                    return Pieces.First().Curve.GetTangent(0);
                }
            }
            public Vector2 EndTangent
            {
                get
                {
                    var lastPiece = Pieces.Last();
                    return lastPiece.Curve.GetTangent(lastPiece.Length);
                }
            }
            public int CountCurves
            {
                get
                {
                    return Pieces.Count;
                }
            }
            public void Add(ParametricCurve<Vector2> curve, float length)
            {
                Pieces.Add(new Piece(curve, length));
            }
            public override Vector2 GetPosition(float s)
            {
                foreach (var node in Pieces)
                {
                    if (s < node.Length)
                        return node.Curve.GetPosition(s);
                    s -= node.Length;
                }
                var lastPiece = Pieces.Last();
                return lastPiece.Curve.GetPosition(s + lastPiece.Length);
            }
            public override Vector2 GetTangent(float s)
            {
                foreach (var node in Pieces)
                {
                    if (s < node.Length)
                        return node.Curve.GetPosition(s);
                    s -= node.Length;
                }
                var lastPiece = Pieces.Last();
                return lastPiece.Curve.GetTangent(s + lastPiece.Length);
            }
        }
        class ClampedParametricCurve<Out> : ParametricCurve<Out>
        {
            public ParametricCurve<Out> Curve;
            public float Min, Max;
            public Out Start
            {
                get
                {
                    return Curve.GetPosition(Min);
                }
            }
            public Out End
            {
                get
                {
                    return Curve.GetPosition(Max);
                }
            }
            public Out StartTangent
            {
                get
                {
                    return Curve.GetTangent(Min);
                }
            }
            public Out EndTangent
            {
                get
                {
                    return Curve.GetTangent(Max);
                }
            }

            public override Out GetPosition(float s)
            {
                return Curve.GetPosition(Mathf.Clamp(s, Min, Max));
            }
            public override Out GetTangent(float s)
            {
                return Curve.GetTangent(Mathf.Clamp(s, Min, Max));
            }
            public ClampedParametricCurve()
            { }
            public ClampedParametricCurve(ParametricCurve<Out> curve, float min, float max)
            {
                Curve = curve;
                Min = min;
                Max = max;
            }
        }

        static class Compute
        {
            public static float RotationDirection(Vector2 from, Vector2 to)
            {
                from.Set(-from.y, from.x);
                return Vector2.Dot(from, to) >= 0 ? 1 : -1;
            }
            public static Vector2[] Intersection(Ray2D ray, Circle2D circle)
            {
                Vector2 AC = circle.Center - ray.Origin;
                float length = Vector2.Dot(AC, ray.Direction);
                Vector2 D = ray.GetPosition(length);
                float distanceCD = Vector2.Distance(D, circle.Center);

                if (distanceCD > circle.Radius)
                    return new Vector2[0];

                float deltaLength = Mathf.Sqrt(circle.Radius * circle.Radius - distanceCD * distanceCD);
                float l1 = length - deltaLength,
                      l2 = length + deltaLength;

                if (l1 <= 0)
                {
                    if (l2 <= 0)
                        return new Vector2[0];

                    return new Vector2[1] { ray.GetPosition(l2) };
                }
                else
                {
                    if (l2 <= 0)
                        return new Vector2[1] { ray.GetPosition(l1) };

                    return new Vector2[2] { ray.GetPosition(l1), ray.GetPosition(l2) };
                }
            }
        }
    }

    /// <summary>
    /// Float range to use with inspector sliders.
    /// </summary>
    [Serializable]
    public struct FloatRange
    {
        public float Min, Max;
        public float Random
        {
            get
            {
                return UnityEngine.Random.Range(Min, Max);
            }
        }
        public FloatRange(float min = 0, float max = 0)
        {
            Min = min;
            Max = max;
        }
    }

    /// <summary>
    /// Int range to use with inspector sliders.
    /// </summary>
    [Serializable]
    public struct IntRange
    {
        public int Min, Max;
        public int Random
        {
            get
            {
                return UnityEngine.Random.Range(Min, Max);
            }
        }
        public IntRange(int min = 0, int max = 0)
        {
            Min = min;
            Max = max;
        }
    }
}

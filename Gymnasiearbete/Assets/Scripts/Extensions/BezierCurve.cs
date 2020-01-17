using UnityEngine;

namespace ArenaShooter.Extensions
{

    struct BezierCurve
    {

        private Vector2 p0, p1, p2, p3;

        public BezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public Vector2 GetPoint(float t)
        {
            float delta = 1f - t;

            // P = (1−t)^3 * P1 + 3(1−t)^2 * tP2 + 3(1−t) * t^2 * P3 + t^3 * P4
            return delta * delta * delta * p0 + 3f * (delta * delta) * t * p1 + 3f * delta * (t * t) * p2 + t * t * t * p3;
        }

        public static BezierCurve CreateSlope(Vector2 start, Vector2 end)
        {
            float middleX = (start.x + end.x) / 2f;

            return new BezierCurve(start, new Vector2(middleX, start.y), new Vector2(middleX, end.y), end);
        }

        public static BezierCurve CreateCurve(Vector2 start, Vector2 end, float curveTime)
        {
            float curveX = start.x + (end.x - start.x) * curveTime;
            float curveY = start.y + (end.y - start.y) * (1f - curveTime);

            return new BezierCurve(start, new Vector2(curveX, start.y), new Vector2(end.x, curveY), end);
        }

    }

}

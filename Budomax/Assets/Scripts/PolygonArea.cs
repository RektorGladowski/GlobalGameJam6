using UnityEngine;

namespace PolygonArea
{
    public static class Vector2ArrayExtensions
    {
        private static float Determinant(Vector2 start, Vector2 end)
        {
            return start.x * end.y - end.x * start.y;
        }

        public static float Area(this Vector2[] points)
        {
            int count = points.Length;

            if (count < 3) { return 0f; }

            float area = Determinant(points[count - 1], points[0]);
            for (int i = 1; i < count; i++)
            {
                area += Determinant(points[i - 1], points[i]);
            }
            return Mathf.Abs(area / 2f);
        }
    }
}

namespace UnityEngine
{
    public static class Vector2Extensions
    {
        public static void Deconstruct(this Vector2 value, out float x, out float y)
        {
            x = value.x;
            y = value.y;
        }
    }
    public static class Vector2IntExtensions
    {
        public static void Deconstruct(this Vector2Int value, out int x, out int y)
        {
            x = value.x;
            y = value.y;
        }
    }
    public static class Vector3Extensions
    {
        public static void Deconstruct(this Vector3 value, out float x, out float y, out float z)
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
    }

    // and more other structrs...
}
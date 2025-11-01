using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods.Settings
{
    public class Movement
    {
        public static int flySpeedIndex = 2;
        public static float flySpeed = 15f;

        public static void ChangeFlySpeed()
        {
            string[] speedNames = new string[] { "Very Slow", "Slow", "Normal", "Fast", "Very Fast", "Extreme" };
            float[] speedValues = new float[] { 5f, 10f, 15f, 20f, 30f, 50f };

            flySpeedIndex++;
            flySpeedIndex %= speedNames.Length;

            GetIndex("Change Fly Speed").overlapText = $"Change Fly Speed [{speedNames[flySpeedIndex]}]";
        }
    }
}

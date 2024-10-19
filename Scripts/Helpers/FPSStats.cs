using CorePublic.Interfaces;

namespace CorePublic.Helpers
{
    
    public class FPSStats: FPSCounter, IStats
    {
        public string GetStats()
        {
            var msec = fpsDeltaTime * 1000.0f;
            string colorCode = fps < 25 ? "<color=red>" : fps < 50 ? "<color=orange>" : "<color=green>";
            var fpsStats = string.Format("{0:0.0} ms." + colorCode + "  ({1:0.} fps)</color>", msec, fps);
            return fpsStats;
        }   
    }
}
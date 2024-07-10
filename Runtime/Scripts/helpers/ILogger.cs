using UnityEngine;

namespace Utilities.SoundService.helpers
{
    public interface ILogger
    {
        public void Log(object message, Object context = null);
        public void LogWarning(object message, Object context = null);
        public void LogError(object message, Object context = null);
    }

    public class SoundServiceLogger : ILogger
    {
        public void Log(object message, Object context)
        {
#if DEBUG_SOUND_SERVICE
            if (Debug.isDebugBuild || Application.isEditor)
            {
                Debug.Log($"<b>SOUND_SERVICE:</b> {message}", context);
            }
#endif
        }

        public void LogWarning(object message, Object context = null)
        {
            if (Debug.isDebugBuild || Application.isEditor)
            {
                Debug.LogWarning($"<b>SOUND_SERVICE:</b> {message}", context);
            }
        }

        public void LogError(object message, Object context = null)
        {
            if (Debug.isDebugBuild || Application.isEditor)
            {
                Debug.LogError($"<b>SOUND_SERVICE:</b> {message}", context);
            }
        }
    }
}
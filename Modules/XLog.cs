﻿using System.Drawing;

namespace MapEditor.Modules
{
    public class XLog
    {
        public static Levels Level = Levels.Debug;

        public static void WriteLine(Levels level, string l, params object[] args)
        {
            if (Level <= level)
            {
                var color = new Color();
                var text = string.Format("{0}: {1}", new object[] { level.ToString(), string.Format(l, args) });

                switch (level)
                {
                    case Levels.Debug:
                        color = Color.LightGray;
                        break;
                    case Levels.Info:
                        color = Color.White;
                        break;
                    case Levels.Trace:
                        color = Color.White;
                        break;                        
                    case Levels.Warning:
                        color = Color.Yellow;
                        break;
                    case Levels.Fatal:
                        color = Color.Red;
                        break;
                    case Levels.Error:
                        color = Color.DarkRed;
                        break;
                }

                Editor.Instance.InsertLog(level, text, color);
            }
        }
    }

    public enum Levels
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal,
    }
}

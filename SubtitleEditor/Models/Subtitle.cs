using System;
using System.Collections.Generic;
using System.Text;

namespace SubtitleEditor.Models
{
    public class Subtitle
    {
        public TimeSpan StartTime;
        public TimeSpan EndTime;
        public List<String> Lines;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(StartTime);
            sb.Append(" : ");
            for (int i = 0; i < Lines.Count; i++)
            {
                sb.Append(Lines[i]);
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NAudio.Wave;
using SubtitleEditor.Models;

namespace SubtitleEditor.Subtitles
{
    public class SubtitleEdit
    {
        public void SetStartTime(List<Subtitle> subtitles, TimeSpan time)
        {
            var delta = time - subtitles[0].StartTime;
            foreach (var sub in subtitles)
            {
                sub.StartTime += delta;
                sub.EndTime += delta;
            }
            SaveTempSubs(subtitles);
        }

        public void Save(string path, List<Subtitle> subtitles)
        {
            var sb = new StringBuilder();
            var size = subtitles.Count;
            for (int i = 0; i < size; i++)
            {
                sb.Append(i+1);
                sb.Append(Environment.NewLine);
                var start = subtitles[i].StartTime.ToString().Replace(".", ",");
                sb.Append(start.Remove(start.Length - 4));
                sb.Append(" --> ");
                var end = subtitles[i].EndTime.ToString().Replace(".", ",");
                sb.Append(end.Remove(end.Length - 4));
                sb.Append(Environment.NewLine);
                for (int j = 0; j < subtitles[i].Lines.Count; j++)
                {
                    sb.Append(subtitles[i].Lines[j]);
                    sb.Append(Environment.NewLine);
                }

                sb.Append(Environment.NewLine);
            }
            File.WriteAllText(path, sb.ToString());
        }
        
        public void SaveTempSubs(List<Subtitle> subtitles)
        {
            Save("tmp.srt", subtitles);
        }
    }
}
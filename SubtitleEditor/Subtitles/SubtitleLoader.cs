using System;
using System.Collections.Generic;
using System.IO;
using SubtitleEditor.Models;

namespace SubtitleEditor.Subtitles
{
    public class SubtitleLoader
    {
        public List<Subtitle> LoadSubtitles(String path)
        {
            var data = File.ReadAllLines(path);
            var dataCount = data.Length;
            var subtitles = new List<Subtitle>();
            var iterator = 0;

            while (iterator < dataCount)
            {
                try
                {
                    var number = Convert.ToInt32(data[iterator++]);
                    var times = data[iterator++].Replace(",", ".").Split(" --> ");
                    var lines = new List<String>();
                    while (!String.IsNullOrEmpty(data[iterator]))
                    {
                        lines.Add(data[iterator++]);
                    }

                    var subtitle = new Subtitle()
                    {
                        StartTime = TimeSpan.Parse(times[0]),
                        EndTime = TimeSpan.Parse(times[1]),
                        Lines = lines
                    };
                    subtitles.Add(subtitle);
                    iterator++;
                }
                catch (FormatException e)
                {
                    continue;
                }
            }

            return subtitles;
        }
    }
}
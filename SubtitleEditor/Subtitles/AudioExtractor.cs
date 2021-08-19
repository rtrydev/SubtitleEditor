using System.Diagnostics;
using System.IO;

namespace SubtitleEditor.Subtitles
{
    public class AudioExtractor
    {
        public string Extract(string path)
        {
            Clean();
            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/usr/bin/ffmpeg", Arguments = $"-i {path} -q:a 0 -map a tmp.mp3", }; 
            Process proc = new Process() { StartInfo = startInfo, };
            proc.Start();
            return "tmp.mp3";
        }

        private void Clean()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/usr/bin/rm", Arguments = "tmp.mp3", }; 
            Process proc = new Process() { StartInfo = startInfo, };
            proc.Start();
        }
    }
}
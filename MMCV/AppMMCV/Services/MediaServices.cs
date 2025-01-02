using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace AppMMCV.Services
{
    public class MediaServices
    {
        public static string SoundOK = AppDomain.CurrentDomain.BaseDirectory + @"\Sound\sound-ok.wav";
        public static string SoundNG = AppDomain.CurrentDomain.BaseDirectory + @"\Sound\sound-ng.wav";

        public static void PlaySound(string path)
        {
            try
            {
                SoundPlayer player = new SoundPlayer(path);
                player.Play(); // Sử dụng PlaySync() nếu bạn muốn đợi âm thanh phát xong
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

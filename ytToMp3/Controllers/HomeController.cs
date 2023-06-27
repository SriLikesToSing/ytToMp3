using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using ytToMp3.Models;
using System.Net.Http;
using System.IO;
using System.Net;

namespace ytToMp3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string ytLink)
        {
            //save sessionID and path for file on db
            
            var ytdl = new YoutubeDL();
            ytdl.YoutubeDLPath = "C:\\Users\\madhu\\Desktop\\Megafile\\Software\\yt-dlp.exe";
            ytdl.FFmpegPath = "C:\\PATH_Programs\\ffmpeg.exe";
            ytdl.OutputFolder = "C:\\Users\\madhu\\Desktop\\Megafile\\programming\\projects\\ytToMp3\\wwwroot\\lib\\links";
            //  download the file to from server to user
            string fileName = "here's a 10 second joke because you're busy.mp3";

            var res = await ytdl.RunAudioDownload(ytLink, AudioConversionFormat.Mp3);
            string path = res.Data;
            downloadFile(fileName, ytdl.OutputFolder);

            return Content ($"Hello {path}");
        }

        private async void downloadFile(string fileName, string filePath)
        {
            /*
            Response.Headers["Content-Disposition"] = "attachment;filename=" + fileName;
            Response.Headers["Transfer-Encoding"] = "identity";
            Response.SendFileAsync(filePath+"\\"+fileName);
            Response.CompleteAsync(); 
            */

            string fi = filePath + "\\" + fileName;

            using (WebClient Client = new WebClient())
            {
                Client.DownloadFile(fi);
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
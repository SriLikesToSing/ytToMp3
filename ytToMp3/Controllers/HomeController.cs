using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using ytToMp3.Models;
using System.Net.Http;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting.Server;
using System.Security.Cryptography.Xml;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Linq;
using System.Reflection.Metadata;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Http.Extensions;

namespace ytToMp3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PhysicalFileProvider _fileProvider;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            string rootPath = Directory.GetCurrentDirectory();
            _fileProvider = new PhysicalFileProvider(rootPath);

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string ytLink)
        {
            string selectedOption = Request.Form["dropdownName"];
                
            if (ytLink == null || selectedOption == null)
                {
                    return RedirectToAction("Index");
                }

            string filepath2 = Path.GetTempPath();
            string filepath3 = "C:\\Users\\madhu\\Desktop\\Megafile\\programming\\projects\\ytToMp3\\wwwroot\\lib\\links";
            string filepath4 = "C:\\home\\site\\wwwroot\\wwwroot\\lib\\links";
            var tempDirectoryPath = Environment.GetEnvironmentVariable("TEMP");

            string debugPath = "C:\\Users\\madhu\\Desktop\\Megafile\\programming\\projects\\ytToMp3\\wwwroot\\lib\\links";


            //save sessionID and path for file on db
            var ytdl = new YoutubeDL();

            //the problem is with the paths
            //ytdl.YoutubeDLPath = "C:\\home\\yt-dlp.exe";
            //ytdl.FFmpegPath = "C:\\home\\ffmpeg.exe";
            ytdl.YoutubeDLPath = "C:\\Users\\madhu\\Desktop\\Megafile\\programming\\projects\\ytToMp3\\wwwroot\\lib\\yt-dlp.exe";
            ytdl.FFmpegPath = "C:\\PATH_Programs\\ffmpeg.exe";
            ytdl.OutputFolder = debugPath;



            //  download the file to from server to user
            if (selectedOption == "mp3")
            {
                var res = await ytdl.RunAudioDownload(ytLink, AudioConversionFormat.Mp3);
                string path = res.Data;
                string fileName = path.Split('\\').Last();
                //checkDelete(ytdl.OutputFolder);
                return downloadFile(path, fileName);

                //return Content($"Hello {path}");
            }
            else if(selectedOption  == "mp4")
            {
                var res = await ytdl.RunVideoDownload(ytLink);
                string path = res.Data;
                string fileName = path.Split('\\').Last();
                //checkDelete(ytdl.OutputFolder);
                return downloadFile(path, fileName);
                //return Content($"Hello {selectedOption}");
            }
            return new EmptyResult();


        }

        protected void buttonClickEvent(object sender, EventArgs e)
        {
           
           
        }
        public void checkDelete(string directoryPath)
        {
                //clear storage every few days or so
                (from f in new DirectoryInfo(directoryPath).GetFiles()
                 where f.CreationTime < DateTime.Now.Subtract(TimeSpan.FromDays(3))
                 select f
                 ).ToList()
                    .ForEach(f => f.Delete());
        }

        [HttpGet]
        public ActionResult downloadFile(string fileLocation, string fileName)
        {
            var bytes = new byte[0];

            using (var fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
            {
                var br = new BinaryReader(fs);
                long numBytes = new FileInfo(fileLocation).Length;
                var buff = br.ReadBytes((int)numBytes);
                return File(buff, "audio/mpeg", fileName);
            }
            return null;
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
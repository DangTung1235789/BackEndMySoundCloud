using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebserviceSoundcloud.Models;

namespace WebserviceSoundcloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private static IWebHostEnvironment _webHostEnvironment;
        public SongsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("GetSong")]
        public async Task<string> GetSongByName(string songName)
        {
            string songUrl = GetSongUrlByName(songName);
            return songUrl;
        }

        [HttpPost("UploadSong")]
        public async Task<string> UploadSong([FromForm]UploadSong obj)
        {
            if(obj.files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Uploads\\Songs"))
                    {
                        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Uploads\\Songs");
                    }
                    string songName = obj.files.FileName.Trim();
                    string filePath = GetFilePath(songName);

                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        obj.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return "\\Uploads\\Songs" + songName;
                    }
                }   
                catch(Exception ex)
                {
                    return ex.ToString();
                }
            }
           else
           {
                return "Upload Failed";
           }
        }

        private string GetFilePath(string SongName)
        {
            return _webHostEnvironment.WebRootPath + "\\Uploads\\Songs\\" + SongName;
        }

        private string GetSongUrlByName(string SongName)
        {
            string SongUrl = string.Empty;
            string HostUrl = "http://192.168.1.7:7121";
            string songPath = GetFilePath(SongName);
            if(!System.IO.File.Exists(songPath))
            {
                SongUrl = "Not Exists The Song!!!";
            } 
            else
            {
                SongUrl = HostUrl + "/Uploads/Songs/" + SongName;
            }
            return SongUrl;
        }
    }
}

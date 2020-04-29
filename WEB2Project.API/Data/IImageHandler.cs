using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WEB2Project.Data
{
    public interface IImageHandler
    {
        Task<IActionResult> UploadImage(IFormFile file);
    }
}

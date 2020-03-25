using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WEB2Project.Data
{
    public interface IImageWriter
    {
        Task<string> UploadImage(IFormFile file);
    }
}

using Domain.DTOs;
using Microsoft.AspNetCore.Http;

namespace Service.Interface
{
    public interface IFileService
    {
        Task<StatusMessageDTO> ProcessFileAsync(IFormFile file);
    }
}

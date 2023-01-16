using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace Domain
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IFileService _fileService;

        public FileController(ILogger logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpPost("/file/read"), DisableRequestSizeLimit]
        public async Task<IActionResult> ProcessFileAsync(IFormFile file)
        {
            try
            {
                var result = await _fileService.ProcessFileAsync(file);

                if (file != null)
                    _logger.Information($"Resultado do processamento do arquivo '{file.FileName}': {JsonSerializer.Serialize(result)}");

                if (result.Sucess)
                    return StatusCode(StatusCodes.Status200OK, result.NormalizePath());
                else
                    return StatusCode(StatusCodes.Status409Conflict, result.Error);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, nameof(ProcessFileAsync));
                return StatusCode(StatusCodes.Status409Conflict, $"{ex.Message} Para mais detalhes, veja o log da aplicação.");
            }
        }
    }
}
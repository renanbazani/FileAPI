using Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Service.Interface;
using ILogger = Serilog.ILogger;

namespace Service.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger _logger;

        public FileService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<StatusMessageDTO> ProcessFileAsync(IFormFile? file)
        {
            try
            {
                if (file == null)
                {
                    _logger.Warning($"O arquivo é obrigatório!");
                    return new StatusMessageDTO() { Sucess = false, Error = $"O arquivo é obrigatório! Para mais detalhes, veja o log da aplicação." };
                }

                if (file.Length == 0)
                {
                    _logger.Warning($"O arquivo '{file.FileName}' está sem conteúdo!");
                    return new StatusMessageDTO() { Sucess = false, Error = $"O arquivo está vazio! Para mais detalhes, veja o log da aplicação." };
                }

                if (!Path.GetExtension(file.FileName).ToLower().Equals(".txt"))
                {
                    _logger.Warning($"A extenção do arquivo '{file.FileName}' está inválida! É necessário que seja no formato '.txt'.");
                    return new StatusMessageDTO() { Sucess = false, Error = $"Formato do arquivo inválido! Para mais detalhes, veja o log da aplicação." };
                }

                var outPath = Path.Combine(Directory.GetCurrentDirectory(), $"{Path.GetFileNameWithoutExtension(file.FileName)}_base64.txt");

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);

                    using (var sw = new StreamWriter(outPath))
                    {
                        await sw.WriteLineAsync(Convert.ToBase64String(ms.ToArray()));
                    }
                }

                return new StatusMessageDTO() { Sucess = true, OutputPath = outPath };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, nameof(ProcessFileAsync));
                return new StatusMessageDTO() { Sucess = false, Error = $"{ex.Message} Para mais detalhes, veja o log da aplicação." };
            }
        }
    }
}

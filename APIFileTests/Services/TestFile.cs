using Microsoft.AspNetCore.Http;
using Moq;
using Serilog;
using Service.Services;

namespace APIFileTests.Services
{
    public class TestFile : IClassFixture<CustomClassFixture>, IDisposable
    {
        private FileService? _fileService;

        #region Initialize and Dispose

        public TestFile(CustomClassFixture fixt)
        {
            Fixture = fixt;
        }

        private CustomClassFixture Fixture { get; }

        public void Dispose() { }

        #endregion

        #region ProcessFile

        [Fact]
        [Trait("", "")]
        public void ProcessFile_1_Sucess()
        {
            var logger = new Mock<ILogger>();

            _fileService = new FileService(logger.Object);

            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Conteúdo do Arquivo...");
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns("file.txt");
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var result = _fileService.ProcessFileAsync(fileMock.Object);

            Assert.True(result.Result.Sucess);
            Assert.Null(result.Result.Error);
        }

        [Fact]
        [Trait("", "")]
        public void ProcessFile_2_FileIsNull()
        {
            var logger = new Mock<ILogger>();

            _fileService = new FileService(logger.Object);

            var result = _fileService.ProcessFileAsync(null);

            Assert.False(result.Result.Sucess);
            Assert.Equal("O arquivo é obrigatório! Para mais detalhes, veja o log da aplicação.", result.Result.Error);
        }

        [Fact]
        [Trait("", "")]
        public void ProcessFile_3_FileIsEmpty()
        {
            var logger = new Mock<ILogger>();

            _fileService = new FileService(logger.Object);

            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("");
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns("file.txt");
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var result = _fileService.ProcessFileAsync(fileMock.Object);

            Assert.False(result.Result.Sucess);
            Assert.Equal("O arquivo está vazio! Para mais detalhes, veja o log da aplicação.", result.Result.Error);
        }

        [Fact]
        [Trait("", "")]
        public void ProcessFile_4_FileFormatIsInvalid()
        {
            var logger = new Mock<ILogger>();

            _fileService = new FileService(logger.Object);

            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Conteúdo do Arquivo...");
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns("file.pdf");
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var result = _fileService.ProcessFileAsync(fileMock.Object);

            Assert.False(result.Result.Sucess);
            Assert.Equal("Formato do arquivo inválido! Para mais detalhes, veja o log da aplicação.", result.Result.Error);
        }

        #endregion
    }
}
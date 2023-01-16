namespace Domain.DTOs
{
    public class StatusMessageDTO
    {
        public bool Sucess { get; set; }
        public string? Error { get; set; }
        public string? OutputPath { get; set; }

        public string? NormalizePath()
        {
            return OutputPath?.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
    }
}

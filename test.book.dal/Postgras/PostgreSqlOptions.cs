using System.ComponentModel.DataAnnotations;

namespace test.book.DAL.Postgras
{
    public class PostgreSqlOptions
    {
        [Required] public string Server { get; set; } = string.Empty;
        [Required] public string Database { get; set; } = string.Empty;
        [Required] public string User { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;
    }
}

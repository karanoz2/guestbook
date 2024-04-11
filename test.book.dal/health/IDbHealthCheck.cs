namespace test.book.dal.health
{
    public interface IDbHealthCheck
    {
        Task<(bool state, Exception? ex)> IsHealthyAsync();
    }
}

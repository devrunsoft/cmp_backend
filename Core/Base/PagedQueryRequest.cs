namespace ScoutDirect.Core.Base
{
    /// <summary>
    /// sample is ?page=1&size=10
    /// </summary>
    public class PagedQueryRequest
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}

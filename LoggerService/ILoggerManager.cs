namespace LoggerService
{
    /// <summary>
    /// Interface định nghĩa các phương thức logging cho ứng dụng
    /// </summary>
    public interface ILoggerManager
    {
        /// <summary>
        /// Log thông tin chi tiết cho debugging
        /// </summary>
        void LogDebug(string message);

        /// <summary>
        /// Log thông tin hoạt động bình thường
        /// </summary>
        void LogInfo(string message);

        /// <summary>
        /// Log cảnh báo về các tình huống có thể gây vấn đề
        /// </summary>
        void LogWarn(string message);

        /// <summary>
        /// Log lỗi xảy ra trong ứng dụng
        /// </summary>
        void LogError(string message);
    }
}

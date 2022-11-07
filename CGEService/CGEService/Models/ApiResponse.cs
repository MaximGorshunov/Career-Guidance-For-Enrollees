namespace CGEService.Models
{
    public class ApiResponse<T>
    {
        /// <summary>
        /// Result data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// HTTP status code
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// A message that specifies the nature of occurred error in the case when Success is <see langword="false"/>
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Indicate that request has been handled successfully 
        /// </summary>
        public bool Success => Data != null;
    }
}

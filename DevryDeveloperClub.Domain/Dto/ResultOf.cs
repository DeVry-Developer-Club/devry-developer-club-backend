namespace DevryDeveloperClub.Domain.Dto
{
    public class ResultOf<T>
    {
        /// <summary>
        /// Value that shall be returned
        /// </summary>
        public T Value;
        
        /// <summary>
        /// Message (if applicable)
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// HTTP Status Code for what happened
        /// </summary>
        public int StatusCode { get; set; } = 200;
        
        public bool Success => ErrorMessage == string.Empty || StatusCode is >= 200 and <= 299;

        public static ResultOf<T> Failure(string errorMessage, int statusCode = 404)
            => new()
            {
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
    }
}
namespace NewsTrack.WebApi.Dtos
{
    public class Error
    {
        internal enum Codes : uint
        {
            NotFound = 404
        }

        public string Message { get; set; }
        public uint Code { get; set; }
    }
}

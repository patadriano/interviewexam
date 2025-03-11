namespace MoviesAPI.Models
{
    public class User
    {
        public int userID { get; set; }
        public  string fullname { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
    }
}

namespace ProLink.Application.Authentication
{
    public class VerifyOTPRequest
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}

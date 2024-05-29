using Microsoft.AspNetCore.Identity;
using ProLink.Application.Authentication;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterUser registerUser);
    Task<LoginResult> LoginAsync(LoginUser loginUser);
    Task<bool> ForgetPasswordAsync(string email);
    Task<IdentityResult> ResetPasswordAsync(ResetPassword resetPassword);
    Task<IdentityResult> ChangePasswordAsync(ChangePassword changePassword);
    Task<IdentityResult> VerifyOTPAsync(VerifyOTPRequest request);
    Task<IdentityResult> SendOTPAsync(string email);
}

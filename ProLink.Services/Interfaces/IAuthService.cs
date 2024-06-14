using Microsoft.AspNetCore.Identity;
using ProLink.Application.Authentication;
using ProLink.Application.DTOs;

public interface IAuthService
{
    public Task<IdentityResult> RegisterAsync(RegisterUser registerUser);
    public Task<AuthDTO> LoginAsync(LoginUser loginUser);
    public Task<string> LogoutAsync();
    public Task<bool> ForgetPasswordAsync(string email);
    public Task<IdentityResult> ResetPasswordAsync(ResetPassword resetPassword);
    public Task<IdentityResult> ChangePasswordAsync(ChangePassword changePassword);
    public Task<IdentityResult> VerifyOTPAsync(VerifyOTPRequest request);
    public Task<IdentityResult> SendOTPAsync(string email);
    public Task<AuthDTO> RefreshTokenAsync(string Token);
    public Task<bool> RevokeTokenAsync(string Token);
}

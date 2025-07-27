using System.Threading.Tasks;



namespace PETScanner
{
    public class LoginViewModel
    {
        private readonly AuthService _authService;

        public LoginViewModel()
        {
            _authService = new AuthService();
        }

        public async Task<AuthResponse> LoginAsync(string mobileId, string password)
        {
            var mobile = new Mobile
            {
                MobileId = mobileId,
                Password = password
            };

            return await _authService.ValidateMobileAsync(mobile);
        }
    }
}

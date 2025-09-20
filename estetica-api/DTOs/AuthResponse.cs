namespace dentist_panel_api.DTOs
{
    public class AuthResponse
    {
        public TokenDTO AuthToken { get; set; } = new TokenDTO();
        public TokenDTO RefreshToken { get; set; } = new TokenDTO();
        public string TokenType { get; set; }
        public ApplicationUserDTO AuthState { get; set; }
        public AuthError Error { get; set; }
    }

    public class TokenDTO
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }

    public class AuthError
    {
        public string Description { get; set; }
        public string Code { get; set; }
    }
}

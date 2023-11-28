using Grpc.Core;
using Identity.Application.Abstraction;
using Identity.Application.Abstraction.Permissions;
using Identity.Application.Abstraction.Users;

namespace Identity.GrpcServer.Services
{
    public class UserGrpcService : Authenticator.AuthenticatorBase
    {
        public IUserService UserService { get; }
        public ITokenService? TokenService { get; }

        public UserGrpcService(IUserService userService, ITokenService? tokenService)
        {
            UserService = userService;
            TokenService = tokenService;
        }

        public override async Task<GetUserIdResponse> GetUserId(GetUserIdRequest request, ServerCallContext context)
        {
            var user = await UserService.GetUser(request.Username, request.Password);
            if (user is null)
                return new GetUserIdResponse { UserId = "" };

            return new GetUserIdResponse { UserId = user.Id.ToString() };

        }
        public override async Task<GetUserIdResponse> GetMobileUserId(GetMobileUserIdRequest request, ServerCallContext context)
        {
            var user = await UserService.GetMobileUser(request.Mobile, request.VerificationKey);
            if (user is null)
                return new GetUserIdResponse { UserId = "" };

            return new GetUserIdResponse { UserId = user.Id.ToString() };

        }
        public override async Task<GetUserIdResponse> GetEmailUserId(GetEmailUserIdRequest request, ServerCallContext context)
        {
            var user = await UserService.GetEmailUser(request.Email, request.VerificationKey);
            if (user is null)
                return new GetUserIdResponse { UserId = "" };

            return new GetUserIdResponse { UserId = user.Id.ToString() };

        }

        public override async Task<GetTokenResponse> GetToken(GetUserIdRequest request, ServerCallContext context)
        {
            var user = await UserService.GetUser(request.Username, request.Password);
            if (user is null)
                return new GetTokenResponse { Token = "", Expiresat = "" };

            if (TokenService is not null)
            {
                var token = await TokenService.GenerateToken(user.Id);

                return new GetTokenResponse { Token = token.Value, Expiresat = token.ExpiresAt.ToString() };
            }

            return new GetTokenResponse { Token = "", Expiresat = "" };
        }
        public override async Task<GetTokenResponse> GetMobileToken(GetMobileUserIdRequest request, ServerCallContext context)
        {
            var user = await UserService.GetMobileUser(request.Mobile, request.VerificationKey);
            if (user is null)
                return new GetTokenResponse { Token = "", Expiresat = "" };

            if (TokenService is not null)
            {
                var token = await TokenService.GenerateToken(user.Id);

                return new GetTokenResponse { Token = token.Value, Expiresat = token.ExpiresAt.ToString() };
            }

            return new GetTokenResponse { Token = "", Expiresat = "" };
        }
        public override async Task<GetTokenResponse> GetEmailToken(GetEmailUserIdRequest request, ServerCallContext context)
        {
            var user = await UserService.GetEmailUser(request.Email, request.VerificationKey);
            if (user is null)
                return new GetTokenResponse { Token = "", Expiresat = "" };

            if (TokenService is not null)
            {
                var token = await TokenService.GenerateToken(user.Id);

                return new GetTokenResponse { Token = token.Value, Expiresat = token.ExpiresAt.ToString() };
            }

            return new GetTokenResponse { Token = "", Expiresat = "" };
        }

        public override async Task<OperationResult> NewMobileVerificationKey(NewMobileUserKeyRequest request, ServerCallContext context)
        {
            await UserService.NewMobileUserVerificationKey(request.Mobile);

            return new OperationResult() { Done = true };
        }
        public override async Task<OperationResult> NewEmailVerificationKey(NewEmailUserKeyRequest request, ServerCallContext context)
        {
            await UserService.NewEmailUserVerificationKey(request.Email);

            return new OperationResult() { Done = true };
        }

        public override async Task<ValidateTokenResponse> ValidateToken(ValidateTokenRequest request, ServerCallContext context)
        {
            if (TokenService is not null)
            {
                var userId = await TokenService.ValidateToken(request.Token);

                return new ValidateTokenResponse
                {
                    Isvalid = !string.IsNullOrEmpty(userId),
                    Userid = userId
                };
            }

            return new ValidateTokenResponse { Isvalid = false, Userid = "" };
        }
    }
}

using FluentResults;
using Grpc.Net.Client;
using Identity.Application.Abstraction;
using Identity.Application.Features.Authorizations.Commands.ViewModels;
using Identity.Application.Models;
using Identity.GrpcServer;
using Identity.Resources;
using Identity.Shared.Api.ViewModels;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NP.Resources;
using NP.Shared.Api.Controllers;

namespace Identity.Shared.Api.Controllers
{

    [Route("api/v1/{appname}/[Controller]")]
    public class AuthController : BaseController
    {
        public ITokenService TokenService { get; }

        //private const string APPNAME = "appname";
        public IConfiguration Configuration { get; }

        public AuthController(IMediator mediator, ITokenService tokenService, IConfiguration configuration) : base(mediator)
        {
            TokenService = tokenService;
            Configuration = configuration;
        }


        [HttpPost("token")]
        public async Task<IActionResult> Post([FromBody] GetTokenViewModel model)
        {

            if (model is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            Token? generatedToken = null;

            var tokenGeneration = Configuration["Identity:TokenGeneration"];
            switch (tokenGeneration)
            {
                case "internal":
                    var response = await GetUserId(model.Username!, model.Password!);
                    if (response is null)
                        return BadResult(string.Format(Errors.CallingServiceError, "Identity.GrpcServer.Authenticator.GetUserIdAsync"));

                    if (string.IsNullOrEmpty(response.UserId))
                        return BadResult(IdentityValidations.IncorrectUsernameOrPasseword);

                    generatedToken = await TokenService.GenerateToken(Guid.Parse(response.UserId));
                    break;

                default:
                    var responseToken = await GetToken(model.Username!, model.Password!);
                    if (responseToken is null)
                        return BadResult(string.Format(Errors.CallingServiceError, "Identity.GrpcServer.Authenticator.GetTokenAsync"));

                    if (string.IsNullOrEmpty(responseToken.Token))
                        return BadResult(IdentityValidations.IncorrectUsernameOrPasseword);

                    generatedToken = new Token
                    {
                        Value = responseToken.Token,
                        ExpiresAt = DateTime.Parse(responseToken.Expiresat)
                    };
                    break;
            }

            var token = generatedToken.Adapt<TokenViewModel>();

            var result = new Result<TokenViewModel>();
            result.WithValue(token);
            result.WithSuccess(Messages.OperationSucceeded);

            return Result(result);
        }

        [HttpPost("mobile-token")]
        public async Task<IActionResult> MobileToken([FromBody] GetMobileTokenViewModel model)
        {

            if (model is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            Token? generatedToken = null;

            var tokenGeneration = Configuration["Identity:TokenGeneration"];
            switch (tokenGeneration)
            {
                case "internal":
                    var response = await GetMobileUserId(model.Mobile!, model.VerificationKey!);
                    if (response is null)
                        return BadResult(string.Format(Errors.CallingServiceError, "Identity.GrpcServer.Authenticator.GetMobileUserIdAsync"));

                    if (string.IsNullOrEmpty(response.UserId))
                        return BadResult(IdentityValidations.InvalidMobileOrVerificationKey);

                    generatedToken = await TokenService.GenerateToken(Guid.Parse(response.UserId));
                    break;

                default:
                    var responseToken = await GetMobileToken(model.Mobile!, model.VerificationKey!);
                    if (responseToken is null)
                        return BadResult(string.Format(Errors.CallingServiceError, "Identity.GrpcServer.Authenticator.GetMobileTokenAsync"));

                    if (string.IsNullOrEmpty(responseToken.Token))
                        return BadResult(IdentityValidations.InvalidMobileOrVerificationKey);

                    generatedToken = new Token
                    {
                        Value = responseToken.Token,
                        ExpiresAt = DateTime.Parse(responseToken.Expiresat)
                    };
                    break;
            }

            var token = generatedToken.Adapt<TokenViewModel>();

            var result = new Result<TokenViewModel>();
            result.WithValue(token);
            result.WithSuccess(Messages.OperationSucceeded);

            return Result(result);
        }
        [HttpPost("email-token")]
        public async Task<IActionResult> EmailToken([FromBody] GetEmailTokenViewModel model)
        {

            if (model is null)
                return BadResult(Validations.InvalidInputData);

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            Token? generatedToken = null;

            var tokenGeneration = Configuration["Identity:TokenGeneration"];
            switch (tokenGeneration)
            {
                case "internal":
                    var response = await GetEmailUserId(model.Email!, model.VerificationKey!);
                    if (response is null)
                        return BadResult(string.Format(Errors.CallingServiceError, "Identity.GrpcServer.Authenticator.GetEmailUserIdAsync"));

                    if (string.IsNullOrEmpty(response.UserId))
                        return BadResult(IdentityValidations.InvalidEmailOrVerificationKey);

                    generatedToken = await TokenService.GenerateToken(Guid.Parse(response.UserId));
                    break;

                default:
                    var responseToken = await GetEmailToken(model.Email!, model.VerificationKey!);
                    if (responseToken is null)
                        return BadResult(string.Format(Errors.CallingServiceError, "Identity.GrpcServer.Authenticator.GetEmailTokenAsync"));

                    if (string.IsNullOrEmpty(responseToken.Token))
                        return BadResult(IdentityValidations.InvalidEmailOrVerificationKey);

                    generatedToken = new Token
                    {
                        Value = responseToken.Token,
                        ExpiresAt = DateTime.Parse(responseToken.Expiresat)
                    };
                    break;
            }

            var token = generatedToken.Adapt<TokenViewModel>();

            var result = new Result<TokenViewModel>();
            result.WithValue(token);
            result.WithSuccess(Messages.OperationSucceeded);

            return Result(result);
        }

        [HttpPost("mobile-tokens")]
        public async Task<IActionResult> Post([FromBody] NewMobileKeyViewModel model)
        {
            if (model is null)
                return BadResult(Validations.InvalidInputData);

            if (model.Mobile is null)
                return BadResult(string.Format(Validations.RequiredField, SharedDataDictionary.Mobile));

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            await NewMobileKey(model.Mobile);

            var result = new Result();
            result.WithSuccess(Messages.OperationSucceeded);

            return Result(result);
        }

        [HttpPost("email-tokens")]
        public async Task<IActionResult> Post([FromBody] NewEmailKeyViewModel model)
        {
            if (model is null)
                return BadResult(Validations.InvalidInputData);

            if (model.Email is null)
                return BadResult(string.Format(Validations.RequiredField, SharedDataDictionary.Mobile));

            if (!ModelState.IsValid)
                return BadResult(ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());


            await NewEmailKey(model.Email);

            var result = new Result();
            result.WithSuccess(Messages.OperationSucceeded);

            return Result(result);
        }


        private async Task<GetUserIdResponse?> GetUserId(string username, string password)

        {
            var grpcAddress = Configuration["Identity:GrpcAddress"];
            var grpcChannel = GrpcChannel.ForAddress(grpcAddress);

            var client = new Authenticator.AuthenticatorClient(grpcChannel);
            var request = new GetUserIdRequest { Username = username, Password = password };

            return await client.GetUserIdAsync(request);
        }
        private async Task<GetUserIdResponse?> GetMobileUserId(string mobile, string key)

        {
            var grpcAddress = Configuration["Identity:GrpcAddress"];
            var grpcChannel = GrpcChannel.ForAddress(grpcAddress);

            var client = new Authenticator.AuthenticatorClient(grpcChannel);
            var request = new GetMobileUserIdRequest { Mobile = mobile, VerificationKey = key };

            return await client.GetMobileUserIdAsync(request);
        }
        private async Task<GetUserIdResponse?> GetEmailUserId(string email, string key)

        {
            var grpcAddress = Configuration["Identity:GrpcAddress"];
            var grpcChannel = GrpcChannel.ForAddress(grpcAddress);

            var client = new Authenticator.AuthenticatorClient(grpcChannel);
            var request = new GetEmailUserIdRequest { Email = email, VerificationKey = key };

            return await client.GetEmailUserIdAsync(request);
        }

        private async Task<GetTokenResponse?> GetToken(string username, string password)
        {
            var grpcAddress = Configuration["Identity:GrpcAddress"];
            var grpcChannel = GrpcChannel.ForAddress(grpcAddress);

            var client = new Authenticator.AuthenticatorClient(grpcChannel);
            var request = new GetUserIdRequest { Username = username, Password = password };

            return await client.GetTokenAsync(request);
        }
        private async Task<GetTokenResponse?> GetMobileToken(string mobile, string key)
        {
            var grpcAddress = Configuration["Identity:GrpcAddress"];
            var grpcChannel = GrpcChannel.ForAddress(grpcAddress);

            var client = new Authenticator.AuthenticatorClient(grpcChannel);
            var request = new GetMobileUserIdRequest { Mobile = mobile, VerificationKey = key };

            return await client.GetMobileTokenAsync(request);
        }
        private async Task<GetTokenResponse?> GetEmailToken(string email, string key)
        {
            var grpcAddress = Configuration["Identity:GrpcAddress"];
            var grpcChannel = GrpcChannel.ForAddress(grpcAddress);

            var client = new Authenticator.AuthenticatorClient(grpcChannel);
            var request = new GetEmailUserIdRequest { Email = email, VerificationKey = key };

            return await client.GetEmailTokenAsync(request);
        }

        private async Task NewEmailKey(string email)
        {
            var grpcAddress = Configuration["Identity:GrpcAddress"];
            var grpcChannel = GrpcChannel.ForAddress(grpcAddress);

            var client = new Authenticator.AuthenticatorClient(grpcChannel);
            var request = new NewEmailUserKeyRequest { Email = email };

            await client.NewEmailVerificationKeyAsync(request);
        }
        private async Task NewMobileKey(string mobile)
        {
            var grpcAddress = Configuration["Identity:GrpcAddress"];
            var grpcChannel = GrpcChannel.ForAddress(grpcAddress);

            var client = new Authenticator.AuthenticatorClient(grpcChannel);
            var request = new NewMobileUserKeyRequest { Mobile = mobile };

            await client.NewMobileVerificationKeyAsync(request);
        }
    }
}

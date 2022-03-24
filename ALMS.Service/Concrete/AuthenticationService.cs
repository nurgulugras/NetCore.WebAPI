using System;
using System.Threading.Tasks;
using ALMS.Model;
using AutoMapper;

namespace ALMS.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IAppService _appService;
        private readonly IUserCredentialService _userCredentialService;
        private readonly IJwtService _jwtService;

        public AuthenticationService(IMapper mapper, IUserService userService, IAppService appService, IUserCredentialService userCredentialService, IJwtService jwtService)
        {
            _mapper = mapper;
            _userService = userService;
            _appService = appService;
            _userCredentialService = userCredentialService;
            _jwtService = jwtService;
        }

        public async Task<OperationResult<string>> LoginForUIAsync(UserLoginRequestParameter loginRequestModel)
        {
            var operationResult = new OperationResult<string>();

            var existsUser = await _userService.GetActiveUserByUsernameAsync(loginRequestModel.Username);
            if (existsUser == null)
            {
                operationResult.Message = "Kullanıcı adı yada şifresi hatalı!";
                operationResult.HttpStatusCode = System.Net.HttpStatusCode.Unauthorized;
                return operationResult;
            }

            var isValidUserOperationResult = _userCredentialService.CheckUserIsValid(existsUser, loginRequestModel.Password);
            if (!isValidUserOperationResult.IsSuccess)
            {
                operationResult.Message = isValidUserOperationResult.Message;
                return operationResult;
            }

            var jwtUser = _mapper.Map<JWTUser>(existsUser);
            var jwtClient = new JWTClient(jwtUser);

            var token = _jwtService.GenerateToken(jwtClient, existsUser.Role);

            operationResult.IsSuccess = true;
            operationResult.DataModel = token;

            return operationResult;
        }

        public async Task<OperationResult<string>> LoginForAPIAsync(APILoginRequestParameter loginRequestModel)
        {
            var operationResult = new OperationResult<string>();

            var existsApp = await _appService.GetActiveAppByKeyAndSecretAsync(loginRequestModel.ApiKey, loginRequestModel.ApiSecretKey);
            if (existsApp == null)
            {
                operationResult.Message = "Geçersiz uygulama!";
                operationResult.HttpStatusCode = System.Net.HttpStatusCode.Unauthorized;
                return operationResult;
            }

            var jwtClient = new JWTClient(new JWTApp(existsApp.Id, existsApp.PasswordHashCode));

            var token = _jwtService.GenerateToken(jwtClient, RoleType.API);

            operationResult.IsSuccess = true;
            operationResult.DataModel = token;

            return operationResult;
        }
        public void Dispose() { }
    }
}
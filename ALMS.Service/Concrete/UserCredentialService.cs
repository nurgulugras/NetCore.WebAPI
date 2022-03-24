using System;
using System.Threading.Tasks;
using ALMS.Core;
using ALMS.Model;

namespace ALMS.Service
{
    public class UserCredentialService : IUserCredentialService
    {
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IContextUserIdentity _contextUserIdentity;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public UserCredentialService(IPasswordHasherService passwordHasherService, IContextUserIdentity contextUserIdentity, IUserService userService, IMailService mailService)
        {
            _passwordHasherService = passwordHasherService;
            _contextUserIdentity = contextUserIdentity;
            _userService = userService;
            _mailService = mailService;
        }
        public OperationResult<User> CheckUserIsValid(User user, string password)
        {
            var operationResult = new OperationResult<User>();
            var isValidUserPass = VerifyPassword(password, user.Password);
            if (!isValidUserPass)
            {
                operationResult.Message = "Kullanıcı adı yada şifresi hatalı!";
                operationResult.NoContent = true;
                return operationResult;
            }
            operationResult.IsSuccess = true;
            operationResult.DataModel = user;
            return operationResult;
        }
        private bool VerifyPassword(string enteredPassword, string userHashedPassword)
        {
            return enteredPassword.Equals(CryptographyHelper.SymmetricDecrypt(userHashedPassword));
        }
        public string GenerateRandomPassword(int lenght)
        {
            return PasswordGeneration.GeneratePassword(lenght, lenght / 4, lenght / 4, 1, lenght / 4, PasswordCombinationType.Any);
        }

        public void Dispose() { }

        public async Task<OperationResult<bool>> ResetUserPasswordAsync(int userId)
        {
            var result = new OperationResult<bool>();
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("Kullanıcı bulunamadı!");
                }

                var generatedPassword = GenerateRandomPassword(10);
                var hashed = GenerateRandomPassword(6);

                user.Password = CryptographyHelper.SymmetricEncrypt(generatedPassword);
                user.PasswordHashCode = hashed;

                try
                {
                    await _userService.UpdateUserAsync(user);
                }
                catch (System.Exception ex)
                {
                    throw new Exception("Kullanıcı güncellenirken hata oluştu. Mesaj: " + ex.TryResolveExceptionMessage());
                }
                try
                {
                    await SendChangingInfoMailAsync(user, generatedPassword);
                }
                catch (System.Exception ex)
                {
                    throw new Exception("Kullanıcı mail bilgilendirmesi yapılırken hata oluştu. Mesaj: " + ex.TryResolveExceptionMessage());
                }

                result.IsSuccess = true;
                result.DataModel = true;
            }
            catch (System.Exception ex)
            {
                result.Message = ex.TryResolveExceptionMessage();
            }
            return result;
        }
        public async Task<OperationResult<bool>> ChangeCurrentUserPasswordAsync(string oldPassword, string newPassword)
        {
            var result = new OperationResult<bool>();
            var currentUserId = _contextUserIdentity.GetContextUser().User.Id;
            var existsUser = await _userService.GetUserByIdAsync(currentUserId);
            if (existsUser == null)
            {
                return new OperationResult<bool>($"{currentUserId} numaralı kayıt mevcut değil!", true);
            }

            var existsUserOldPassword = CryptographyHelper.SymmetricDecrypt(existsUser.Password);
            if (!existsUserOldPassword.Equals(oldPassword))
            {
                return new OperationResult<bool>($"Şifreler uyumlu değil!", true);
            }

            existsUser.Password = CryptographyHelper.SymmetricEncrypt(newPassword);
            existsUser.PasswordHashCode = GenerateRandomPassword(12);

            var updateOperationResult = await _userService.UpdateUserAsync(existsUser);
            if (updateOperationResult.IsSuccess)
            {
                result.IsSuccess = true;
                result.DataModel = true;
            }
            else
            {
                result.IsSuccess = false;
                result.DataModel = false;
                result.Message = updateOperationResult.Message;
            }

            return result;
        }

        private async Task SendChangingInfoMailAsync(User user, string password)
        {
            var mailBody = MessageTemplates.ChangePasswordTemplate(user, password);
            var mailInfo = new MailInfo(user.Email, MessageTemplates.UserPasswordChangeSubject, mailBody);
            await _mailService.SendMailAsync(mailInfo);
        }

    }
}
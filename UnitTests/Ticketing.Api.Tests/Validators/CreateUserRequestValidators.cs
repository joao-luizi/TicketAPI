using Ticketing.Api.Validators;
using Ticketing.Api.Contracts.Request;

namespace Ticketing.Api.Tests.Validators
{
    public class CreateUserRequestValidators
    {
        private readonly CreateUserRequestValidator _validator = new();

        [Fact]
        public void ValidateCreateUserRequest_ShouldPass_WhenRequestIsValid()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "user@example.com",
                fullName = "Valid User",
                password = "Aa1!abcd"
            };

            var result = _validator.Validate(request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_ShouldFail_WhenUsernameIsEmpty()
        {
            var request = new CreateUserRequest
            {
                username = string.Empty,
                email = "user@example.com",
                fullName = "Valid User",
                password = "Aa1!abcd"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.username)
                                              && e.ErrorMessage == "Username is required."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenUsernameTooShort()
        {
            var request = new CreateUserRequest
            {
                username = "ab",
                email = "user@example.com",
                fullName = "Valid User",
                password = "Aa1!abcd"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.username)
                                              && e.ErrorMessage == "Username must be between 3 and 80 characters."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenUsernameTooLong()
        {
            var request = new CreateUserRequest
            {
                username = new string('u', 81),
                email = "user@example.com",
                fullName = "Valid User",
                password = "Aa1!abcd"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.username)
                                              && e.ErrorMessage == "Username must be between 3 and 80 characters."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenEmailIsEmpty()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = string.Empty,
                fullName = "Valid User",
                password = "Aa1!abcd"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.email)
                                              && e.ErrorMessage == "Email is required."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenEmailIsInvalid()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "invalid-email",
                fullName = "Valid User",
                password = "Aa1!abcd"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.email)
                                              && e.ErrorMessage == "Email must be a valid email address."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenFullNameIsEmpty()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "user@example.com",
                fullName = string.Empty,
                password = "Aa1!abcd"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.fullName)
                                              && e.ErrorMessage == "Full name is required."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenFullNameTooShort()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "user@example.com",
                fullName = "Al",
                password = "Aa1!abcd"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.fullName)
                                              && e.ErrorMessage == "Full name must be between 3 and 160 characters."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenFullNameTooLong()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "user@example.com",
                fullName = new string('f', 161),
                password = "Aa1!abcd"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.fullName)
                                              && e.ErrorMessage == "Full name must be between 3 and 160 characters."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenPasswordIsEmpty()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "user@example.com",
                fullName = "Valid User",
                password = string.Empty
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.password)
                                              && e.ErrorMessage == "Password é obrigatória."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenPasswordTooShort()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "user@example.com",
                fullName = "Valid User",
                password = "Aa1!a" // shorter than 8
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.password)
                                              && e.ErrorMessage == "Password deve ter pelo menos 8 caracteres."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenPasswordMissingUppercase()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "user@example.com",
                fullName = "Valid User",
                password = "aa1!aaaa"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.password)
                                              && e.ErrorMessage == "Password deve conter pelo menos uma letra maiúscula."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenPasswordMissingLowercase()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "user@example.com",
                fullName = "Valid User",
                password = "AA1!AAAA"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.password)
                                              && e.ErrorMessage == "Password deve conter pelo menos uma letra minúscula."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenPasswordMissingDigit()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "user@example.com",
                fullName = "Valid User",
                password = "Aa!aaaaa"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.password)
                                              && e.ErrorMessage == "Password deve conter pelo menos um número."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenPasswordMissingSpecialCharacter()
        {
            var request = new CreateUserRequest
            {
                username = "validuser",
                email = "user@example.com",
                fullName = "Valid User",
                password = "Aa1aaaaa"
            };

            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateUserRequest.password)
                                              && e.ErrorMessage == "Password deve conter pelo menos um caractere especial."));
        }
    }
}
using System.Linq;
using Xunit;
using Ticketing.Api.Validators;
using Ticketing.Api.Contracts.Request;

namespace Ticketing.Api.Tests.Validators
{
    public class CreateTicketRequestValidators
    {
        private readonly CreateTicketRequestValidator _validator = new();

        [Fact]
        public void ValidateCreateTicketRequest_ShouldPass_WhenRequestIsValid()
        {
            var request = new CreateTicketRequest
            {
                Title = "Valid Title",
                Description = "Valid Description",
                UserEmail = "admin@admin.com"
            };
            var result = _validator.Validate(request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_ShouldFail_WhenTitleIsEmpty()
        {
            var request = new CreateTicketRequest
            {
                Title = string.Empty,
                Description = "Valid Description",
                UserEmail = "admin@admin.com"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateTicketRequest.Title)
                                              && e.ErrorMessage == "Title is required."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenTitleExceedsMaxLength()
        {
            var request = new CreateTicketRequest
            {
                Title = new string('A', 101),
                Description = "Valid Description",
                UserEmail = "admin@admin.com"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateTicketRequest.Title)
                                              && e.ErrorMessage == "Title cannot exceed 100 characters."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenDescriptionIsEmpty()
        {
            var request = new CreateTicketRequest
            {
                Title = "Valid Title",
                Description = string.Empty,
                UserEmail = "admin@admin.com"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateTicketRequest.Description)
                                              && e.ErrorMessage == "Description is required."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenDescriptionExceedsMaxLength()
        {
            var request = new CreateTicketRequest
            {
                Title = "Valid Title",
                Description = new string('D', 501),
                UserEmail = "admin@admin.com"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateTicketRequest.Description)
                                              && e.ErrorMessage == "Description cannot exceed 500 characters."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenUserEmailIsEmpty()
        {
            var request = new CreateTicketRequest
            {
                Title = "Valid Title",
                Description = "Valid Description",
                UserEmail = string.Empty
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateTicketRequest.UserEmail)
                                              && e.ErrorMessage == "User email is required."));
        }

        [Fact]
        public void Validate_ShouldFail_WhenUserEmailIsInvalid()
        {
            var request = new CreateTicketRequest
            {
                Title = "Valid Title",
                Description = "Valid Description",
                UserEmail = "not-an-email"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.True(result?.Errors.Any(e => e.PropertyName == nameof(CreateTicketRequest.UserEmail)
                                              && e.ErrorMessage == "Invalid email address."));
        }
    }
}

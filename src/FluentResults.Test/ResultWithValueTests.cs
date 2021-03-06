using FluentAssertions;
using System;
using Xunit;

namespace FluentResults.Test
{
    public class ResultWithValueTests
    {
        [Fact]
        public void Ok_WithNoParams_ShouldReturnSuccessResult()
        {
            // Act
            var okResult = Result.Ok<int>(default);

            // Assert
            okResult.IsFailed.Should().BeFalse();
            okResult.IsSuccess.Should().BeTrue();

            okResult.Reasons.Should().BeEmpty();
            okResult.Errors.Should().BeEmpty();
            okResult.Successes.Should().BeEmpty();
            okResult.Value.Should().Be(0);
            okResult.ValueOrDefault.Should().Be(0);
        }

        [Fact]
        public void Ok_WithValidValue_ShouldReturnSuccessResult()
        {
            // Act
            var okResult = Result.Ok(5);

            // Assert
            okResult.IsSuccess.Should().BeTrue();
            okResult.Value.Should().Be(5);
            okResult.ValueOrDefault.Should().Be(5);
        }

        [Fact]
        public void WithValue_WithValidParam_ShouldReturnSuccessResult()
        {
            var okResult = Result.Ok<int>(default);

            // Act
            okResult.WithValue(5);

            // Assert
            okResult.Value.Should().Be(5);
            okResult.ValueOrDefault.Should().Be(5);
        }

        [Fact]
        public void Fail_WithValidErrorMessage_ShouldReturnFailedResult()
        {
            // Act
            var result = Result.Fail<int>("Error message");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.ValueOrDefault.Should().Be(0);
        }

        [Fact]
        public void ValueOrDefault_WithDateTime_ShouldReturnFailedResult()
        {
            var result = Result.Fail<DateTime>("Error message");

            // Act
            var valueOrDefault = result.ValueOrDefault;

            // Assert
            var defaultDateTime = default(DateTime);
            valueOrDefault.Should().Be(defaultDateTime);
        }

        class TestValue
        {

        }

        [Fact]
        public void ValueOrDefault_WithObject_ShouldReturnFailedResult()
        {
            var result = Result.Fail<TestValue>("Error message");

            // Act
            var valueOrDefault = result.ValueOrDefault;

            // Assert
            valueOrDefault.Should().Be(null);
        }

        [Fact]
        public void Value_WithResultInFailedState_ShouldThrowException()
        {
            // Act
            var result = Result.Fail<int>("Error message");

            // Assert

            Action action = () => { var v = result.Value; };

            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Result is in status failed. Value is not set.");
        }

        [Fact]
        public void WithValue_WithResultInFailedState_ShouldThrowException()
        {
            var failedResult = Result.Fail<int>("Error message");

            // Act
            Action action = () => { failedResult.WithValue(5); };

            // Assert
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Result is in status failed. Value is not set.");
        }

        [Fact]
        public void ToResult_ReturnFailedResult()
        {
            var valueResult = Result.Fail<int>("First error message");

            // Act
            var result = valueResult.ToResult();

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void ToResult_ToAntotherValueType_ReturnFailedResult()
        {
            var valueResult = Result.Fail<int>("First error message");

            // Act
            var result = valueResult.ToResult<float>();

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void ToResult_ToAntotherValueTypeWithOkResultAndNoConverter_ReturnFailedResult()
        {
            var valueResult = Result.Fail<int>("Failed");

            // Act
            var result = valueResult.ToResult<float>();

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void ToResult_ToAntotherValueTypeWithOkResultAndConverter_ReturnFailedResult()
        {
            var valueResult = Result.Ok(4);

            // Act
            var result = valueResult.ToResult<float>(v => v);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(4);
        }

        [Fact]
        public void ImplicitCastOperator_ReturnFailedResult()
        {
            var valueResult = Result.Fail<int>("First error message");

            // Act
            Result result = valueResult;

            // Assert
            result.IsFailed.Should().BeTrue();
        }
    }
}
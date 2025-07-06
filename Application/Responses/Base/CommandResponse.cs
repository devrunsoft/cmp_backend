using CMPNatural.Application.Responses;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace ScoutDirect.Application.Responses
{
    public class CommandResponse<T>
    {
        public CommandResponse()
        {

        }

        //public object Id { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; } = false;
        public string StatusCode { get; set; }
        public string Message { get; set; }

        public List<ValidationFailure> Errors { get; set; }
    }

    public class Success<T> : CommandResponse<T>
    {
        public Success()
        {
            Success = true;
            Message = "Operation completed successfully.";
            StatusCode = "200";
        }
    }

    public class HasError : CommandResponse<object>
    { 
        public HasError(ValidationResult validationResult)
        {
            Success = validationResult.IsValid;
            Errors = validationResult.Errors.Where(validationFailure => validationFailure != null).ToList();
            Message = validationResult.IsValid ? null : "Error";
            StatusCode = "400";
        } 
    }

    public class HasError<T> : CommandResponse<T>
    {

        public HasError()
        {
            Success = false;
            StatusCode = "400";
        }
        public HasError(ValidationResult validationResult)
        {
            Success = validationResult.IsValid;
            Errors = validationResult.Errors.Where(validationFailure => validationFailure != null).ToList();
            Message = validationResult.IsValid ? null : "Error";
            StatusCode = "400";
        }
    }


    public class NoAcess<T>: CommandResponse<T>
    {

        public NoAcess()
        {
            Success = false;
            StatusCode = "400";
        }
        public NoAcess(ValidationResult validationResult)
        {
            Success = validationResult.IsValid;
            Errors = validationResult.Errors.Where(validationFailure => validationFailure != null).ToList();
            Message = validationResult.IsValid ? null : "Error";
            StatusCode = "400";
        }
    }

    public class NoAcess : CommandResponse<object>
    {

        public NoAcess()
        {
            Success = false;
            StatusCode = "400";
        }
        public NoAcess(ValidationResult validationResult)
        {
            Success = validationResult.IsValid;
            Errors = validationResult.Errors.Where(validationFailure => validationFailure != null).ToList();
            Message = validationResult.IsValid ? null : "Error";
            StatusCode = "400";
        }
    }

    public class BadRequest : CommandResponse<object>
    {
        public BadRequest()
        {
            Success = false;
            StatusCode = "400";
        }

        public BadRequest(ValidationResult validationResult)
        {
            Success = validationResult.IsValid;
            Errors = validationResult.Errors.Where(validationFailure => validationFailure != null).ToList();
            Message = validationResult.IsValid ? null : "Error";
            StatusCode = "400";
        } 
    }

    public class ResponseNotFound : CommandResponse<object>
    {
        public ResponseNotFound()
        {
            Success = false;
            Message = "Not Found";
            StatusCode = "404";
        }
 
    }

    public static class CommandResponsetExtensions
    {
        public static bool IsSucces<T>(this CommandResponse<T> i)
        {
            return i is Success<T>;
        }
        public static bool IsNoAcess<T>(this CommandResponse<T> i)
        {
            return i is NoAcess;
        }
        public static bool IsBadRequest<T>(this CommandResponse<T> i)
        {
            return i is BadRequest;
        }
    }
}



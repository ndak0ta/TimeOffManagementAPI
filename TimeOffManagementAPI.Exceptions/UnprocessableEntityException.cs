namespace TimeOffManagementAPI.Exceptions;

public class UnprocessableEntityException : Exception
{
    public UnprocessableEntityException() : base("Unprocessable request.")
    {
    }

    public UnprocessableEntityException(string message) : base(message)
    {
    }
}
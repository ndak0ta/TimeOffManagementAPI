namespace TimeOffManagementAPI.Exceptions;

public class DuplicateRecordException : Exception
{
    public DuplicateRecordException() : base("Dubplicate record found.")
    {
    }

    public DuplicateRecordException(string message) : base(message)
    {
    }
}



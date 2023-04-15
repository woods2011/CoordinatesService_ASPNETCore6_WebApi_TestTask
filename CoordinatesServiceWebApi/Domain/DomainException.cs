namespace CoordinatesServiceWebApi.Domain;

public class DomainException : Exception
{
    public DomainException() { }

    public DomainException(string message) : base(message) { }

    public DomainException(string message, Exception inner) : base(message, inner) { }
}

public class DomainValidationException : DomainException
{
    public DomainValidationException(string message) : base(message) { }
}
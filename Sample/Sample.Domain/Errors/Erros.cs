using Neptunee.OperationResponse;

namespace Sample.Domain;

public static class Errors
{
    public static class Events
    {
        public static Error AvailableTickets(int minimum) => new($"AvailableTickets cannot be less than {minimum}");
        public static readonly Error StartDateCannotBeInPast = new("StartDate cannot be in past");
        public static readonly Error EndDateCannotBeBeforeStatDate = new("EndDate cannot be before statDate");
        public static readonly Error ChangesHappened = new("Event data are changed by another user after you got the original value");
    }

    public static class Tickets
    {
        public static readonly Error AlreadyBooked = new("Already Booked");
        public static readonly Error FullBooking = new("Full Booking");
    }

    public static class Users
    {
        public static readonly Error EmailNotFound = new("Email not fount");
        public static readonly Error EmailAlreadyUsed = new("Email already used");
        public static readonly Error WrongEmailOrPassword = new("Wrong email or password ");
    }
}
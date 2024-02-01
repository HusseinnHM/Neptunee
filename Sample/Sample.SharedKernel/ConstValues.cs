namespace Sample.SharedKernel;

public abstract class ConstValues
{
    public abstract class HeaderKeys
    {
        public const string Language = "x-language";
    }

    public abstract class ClaimTypes
    {
        public const string UserType = "userType";
    }

    public abstract class UserTypes
    {
        public const string EventManager = nameof(EventManager);
        public const string ParticipationUser = nameof(ParticipationUser);
    }
}
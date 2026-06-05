namespace Test.Web.Classes
{
    public enum PaymentSystemEnum : int
    {
        bKash = 1,
        Rocket = 2,
        Nagad = 3
    }

    public enum PaymentStatusEnum : int
    {
        Pending = 1,
        Verified = 2,
        Rejected = 3
    }

    public enum RoleEnum : int
    {
        User = 1,
        Admin = 2,
        SuperAdmin = 3,
        Viewer = 4
    }
}

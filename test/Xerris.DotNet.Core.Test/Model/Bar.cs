namespace Xerris.DotNet.Core.Test.Model
{
    public class Bar
    {
        public Bar(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int SocialSecurityNumber { get; set; }
    }
}
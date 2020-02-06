namespace Xerris.DotNet.Core.Test.Model
{
    public class Bar
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int SocialSecurityNumber { get; set; }

        public Bar(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
    
}
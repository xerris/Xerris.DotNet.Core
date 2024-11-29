namespace Xerris.DotNet.Core.Test.Model
{
    public class Foo
    {
        public Foo()
        {
        }

        public Foo(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
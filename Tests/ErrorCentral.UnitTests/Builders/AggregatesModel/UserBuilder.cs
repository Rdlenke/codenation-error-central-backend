using ErrorCentral.Domain.AggregatesModel.UserAggregate;

namespace ErrorCentral.UnitTests.Builders.AggregatesModel
{
    public class UserBuilder : IBuilder<User>
    {
        private User _user;
        public string FirstName => "Otavio";
        public string LastName => "Silva";
        public string Email => "otavio@email.com";
        public string Password => "senha123!";
        public UserBuilder()
        {
            _user = WithDefaultValues();
        }

        public User WithDefaultValues()
        {
            _user = new User(FirstName, LastName, Email, Password);
            return _user;
        }

        public User Build()
        {
            return _user;
        }
    }
}

using ErrorCentral.Application.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.UnitTests.Builders.Settings
{
    public class JwtBuilder : IBuilder<Jwt>
    {
        private Jwt _jwt;
        public string Secret => "js6%\\$(tzp8E(Lu!s4DhM.$ABNtj4Cqmf.:U";
        public int Expiration => 2;
        public string Issuer => "MyEnvironment";
        public string Audience => "https://localhost";

        public JwtBuilder()
        {
            _jwt = WithDefaultValues();
        }

        public Jwt WithDefaultValues()
        {
            _jwt = new Jwt()
            {
                Secret = Secret,
                Expiration = Expiration,
                Issuer = Issuer,
                Audience = Audience,
            };
            return _jwt;
        }

        public Jwt Build()
        {
            return _jwt;
        }
    }
}

using System;

namespace Zaabee.Dapper.Lambda.TestProject.Entities
{
    class FakeGuid
    {
        public FakeGuid()
        {
            Id = Guid.NewGuid();
        }

        public Guid? Id { get; set; }
    }
}

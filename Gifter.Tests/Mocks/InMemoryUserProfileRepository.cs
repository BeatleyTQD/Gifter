using System;
using System.Collections.Generic;
using System.Linq;
using Gifter.Models;
using Gifter.Repositories;

namespace Gifter.Tests.Mocks
{
    class InMemoryUserProfileRepository : IUserProfileRepository
    {
        private readonly List<Post> _data;
        public void Add(UserProfile user)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserProfile> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            throw new NotImplementedException();
        }

        public UserProfile GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserProfile user)
        {
            throw new NotImplementedException();
        }
    }
}

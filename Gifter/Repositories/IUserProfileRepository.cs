using Gifter.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gifter.Repositories
{
    public interface IUserProfileRepository
    {
        List<UserProfile> GetAll();
        UserProfile GetById(int id);
        void Add(UserProfile user);
        void Update(UserProfile user);
        void Delete(int id);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Gifter.Models;
using Gifter.Repositories;
using Microsoft.AspNetCore.Localization;

namespace Gifter.Tests.Mocks
{
    class InMemoryUserProfileRepository : IUserProfileRepository
    {
        private readonly List<UserProfile> _data;

        public List<UserProfile> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryUserProfileRepository(List<UserProfile> startingData)
        {
            _data = startingData;
        }

        public void Add(UserProfile user)
        {
            var lastUserProfile = _data.Last();
            user.Id = lastUserProfile.Id + 1;
            _data.Add(user);
        }

        public void Delete(int id)
        {
            var userProfileToDelete = _data.FirstOrDefault(p => p.Id == id);
            if (userProfileToDelete == null)
            {
                return;
            }

            _data.Remove(userProfileToDelete);
        }

        public List<UserProfile> GetAll()
        {
            return _data;
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            return _data.FirstOrDefault(p => p.FirebaseUserId == firebaseUserId);
        }

        public UserProfile GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public void Update(UserProfile user)
        {
            var currentUserProfile = _data.FirstOrDefault(p => p.Id == user.Id);
            if (currentUserProfile == null)
            {
                return;
            }
            currentUserProfile.Name = user.Name;
            currentUserProfile.Bio = user.Bio;
            currentUserProfile.Email = user.Email;
            currentUserProfile.DateCreated = user.DateCreated;
            currentUserProfile.FirebaseUserId = user.FirebaseUserId;
            currentUserProfile.ImageUrl = user.ImageUrl;
        }
    }
}

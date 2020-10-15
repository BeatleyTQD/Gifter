using Gifter.Controllers;
using Gifter.Models;
using Gifter.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Gifter.Tests
{
    public class UserProfileControllerTests
    {
        [Fact]
        public void Get_Returns_All_User_Profiles()
        {
            // Arrange
            var userCount = 10;
            var userProfiles = CreateTestUsers(userCount);

            var repo = new InMemoryUserProfileRepository(userProfiles);
            var controller = new UserProfileController(repo);

            //Act
            var result = controller.Get();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualUsers = Assert.IsType<List<UserProfile>>(okResult.Value);

            Assert.Equal(userCount, actualUsers.Count);
            Assert.Equal(userProfiles, actualUsers);
        }

        [Fact]
        public void Get_By_FirebaseId_Returns_A_User_With_Id()
        {
            //Arrange
            var testUserid = "64abc";
            var users = CreateTestUsers(13);
            users[2].FirebaseUserId = testUserid;

            var repo = new InMemoryUserProfileRepository(users);
            var controller = new UserProfileController(repo);

            //Act
            var result = controller.GetByFirebaseUserId(testUserid);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualUser = Assert.IsType<UserProfile>(okResult.Value);

            Assert.Equal(testUserid, actualUser.FirebaseUserId);
        }

        [Fact]
        public void Post_Method_Adds_A_New_User()
        {
            var userCount = 50;
            var users = CreateTestUsers(userCount);

            var repo = new InMemoryUserProfileRepository(users);
            var controller = new UserProfileController(repo);

            var newUser = new UserProfile()
            {
                Name = "Name",
                Bio = "Bio",
                Email = "Email",
                DateCreated = DateTime.Today,
                FirebaseUserId = "FirebaseUserId"
            };

            controller.Post(newUser);

            Assert.Equal(userCount + 1, repo.InternalData.Count);
        }
       
        [Fact]
        public void Put_Method_Updates_A_User()
        {
            var testUserId = 64;
            var users = CreateTestUsers(4);
            users[1].Id = testUserId;

            var repo = new InMemoryUserProfileRepository(users);
            var controller = new UserProfileController(repo);

            var userToUpdate = new UserProfile()
            {
                Id = testUserId,
                Name = "Updated",
                Bio = "Updated",
                Email = "Updated",
                DateCreated = DateTime.Today,
                FirebaseUserId = "Updated"
            };

            controller.Put(testUserId, userToUpdate);

            var userFromDB = repo.InternalData.FirstOrDefault(p => p.Id == testUserId);
            Assert.NotNull(userFromDB);

            Assert.Equal(userToUpdate.Name, userFromDB.Name);
            Assert.Equal(userToUpdate.Bio, userFromDB.Bio);
            Assert.Equal(userToUpdate.Email, userFromDB.Email);
            Assert.Equal(userToUpdate.DateCreated, userFromDB.DateCreated);
            Assert.Equal(userToUpdate.FirebaseUserId, userFromDB.FirebaseUserId);
        }

        [Fact]
        public void Delete_Method_Removes_A_Post()
        {
            var testUserId = 34;
            var users = CreateTestUsers(8);
            users[4].Id = testUserId;

            var repo = new InMemoryUserProfileRepository(users);
            var controller = new UserProfileController(repo);

            controller.Delete(testUserId);

            var userFromDB = repo.InternalData.FirstOrDefault(p => p.Id == testUserId);
            Assert.Null(userFromDB);
        }

        private List<UserProfile> CreateTestUsers(int count)
        {
            var users = new List<UserProfile>();
            for (var i = 1; i <= count; i++)
            {
                users.Add(new UserProfile()
                {
                    Id = i,
                    Name = $"Name {i}",
                    Bio = $"Bio {i}",
                    Email = $"{i}@test.com",
                    DateCreated = DateTime.Today.AddDays(-i),
                    FirebaseUserId = $"{i}abc"
                }) ;
            }
            return users;
        }
    }
}

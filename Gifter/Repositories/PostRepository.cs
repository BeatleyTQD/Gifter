using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Gifter.Models;
using Gifter.Utils;

namespace Gifter.Repositories
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        public PostRepository(IConfiguration configuration) : base(configuration) { }

        public List<Post> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                { 
                    cmd.CommandText = PostQuery(" ORDER BY p.DateCreated");
                    var reader = cmd.ExecuteReader();
                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        posts.Add(DbUtils.NewPost(reader));
                    }
                    reader.Close();
                    return posts;
                }
            }
        }

        public List<Post> GetAllWithComments()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = PostWithCommentsQuery(" ORDER BY p.DateCreated");
                    var reader = cmd.ExecuteReader();
                    var posts = new List<Post>();

                    while (reader.Read())
                    {
                        var postId = DbUtils.GetInt(reader, "PostId");
                        var existingPost = posts.FirstOrDefault(p => p.Id == postId);

                        if (existingPost == null)
                        {
                            existingPost = DbUtils.NewPostWithComments(reader);
                            posts.Add(existingPost);
                        }

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            existingPost.Comments.Add(new Comment()
                            {
                                Id = DbUtils.GetInt(reader, "CommentId"),
                                Message = DbUtils.GetString(reader, "Message"),
                                PostId = postId,
                                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                            });
                        }
                    }

                    reader.Close();
                    return posts;
                }
            }
        }

        public Post GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = PostQuery(" WHERE p.Id = @Id");
                    DbUtils.AddParameter(cmd, "@Id", id);
                    var reader = cmd.ExecuteReader();
                    Post post = null;

                    if (reader.Read())
                    {
                        post = DbUtils.NewPost(reader);
                    }

                    reader.Close();
                    return post;
                }
            }
        }

        public Post GetPostByIdWithComments(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = PostWithCommentsQuery(" WHERE p.Id = @Id");
                    DbUtils.AddParameter(cmd, "@Id", id);
                    var reader = cmd.ExecuteReader();
                    Post post = null;

                    while (reader.Read())
                    {
                        if (post == null)
                        {
                            post = DbUtils.NewPostWithComments(reader);
                        }

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            post.Comments.Add(new Comment()
                            {
                                Id = DbUtils.GetInt(reader, "CommentId"),
                                Message = DbUtils.GetString(reader, "Message"),
                                PostId = post.Id,
                                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                            });
                        }
                    }

                    reader.Close();
                    return post;
                }
            }
        }

        public List<Post> PostsFromUser(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = PostWithCommentsQuery(" WHERE p.UserProfileId = @id");
                    DbUtils.AddParameter(cmd, "@Id", id);
                    var reader = cmd.ExecuteReader();
                    var posts = new List<Post>();


                    while (reader.Read())
                    {
                        var postId = DbUtils.GetInt(reader, "PostId");
                        var existingPost = posts.FirstOrDefault(p => p.Id == postId);

                        if (existingPost == null)
                        {
                            existingPost = DbUtils.NewPostWithComments(reader);
                            posts.Add(existingPost);
                        }

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            existingPost.Comments.Add(new Comment()
                            {
                                Id = DbUtils.GetInt(reader, "CommentId"),
                                Message = DbUtils.GetString(reader, "Message"),
                                PostId = postId,
                                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                            });
                        }
                    }

                    reader.Close();
                    return posts;
                }
            }
        }

        public void Add(Post post)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Post (Title, Caption, DateCreated, ImageUrl, UserProfileId)
                        OUTPUT INSERTED.ID
                        VALUES (@Title, @Caption, @DateCreated, @ImageUrl, @UserProfileId)";

                    DbUtils.AddParameter(cmd, "@Title", post.Title);
                    DbUtils.AddParameter(cmd, "@Caption", post.Caption);
                    DbUtils.AddParameter(cmd, "@DateCreated", post.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", post.ImageUrl);
                    DbUtils.AddParameter(cmd, "@UserProfileId", post.UserProfileId);

                    post.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Post post)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Post
                           SET Title = @Title,
                               Caption = @Caption,
                               DateCreated = @DateCreated,
                               ImageUrl = @ImageUrl,
                               UserProfileId = @UserProfileId
                         WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Title", post.Title);
                    DbUtils.AddParameter(cmd, "@Caption", post.Caption);
                    DbUtils.AddParameter(cmd, "@DateCreated", post.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", post.ImageUrl);
                    DbUtils.AddParameter(cmd, "@UserProfileId", post.UserProfileId);
                    DbUtils.AddParameter(cmd, "@Id", post.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Post WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Post> Search(string criterion, bool sortDescending)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var sql = PostWithCommentsQuery(" WHERE p.Title LIKE @Criterion OR p.Caption LIKE @Criterion");

                    if (sortDescending)
                    {
                        sql += " ORDER BY p.DateCreated DESC";
                    }
                    else
                    {
                        sql += " ORDER BY p.DateCreated";
                    }

                    cmd.CommandText = sql;
                    DbUtils.AddParameter(cmd, "@Criterion", $"%{criterion}%");
                    var reader = cmd.ExecuteReader();
                    var posts = new List<Post>();

                    while (reader.Read())
                    {
                        var postId = DbUtils.GetInt(reader, "PostId");
                        var existingPost = posts.FirstOrDefault(p => p.Id == postId);

                        if (existingPost == null)
                        {
                            existingPost = DbUtils.NewPostWithComments(reader);
                            posts.Add(existingPost);
                        }

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            existingPost.Comments.Add(new Comment()
                            {
                                Id = DbUtils.GetInt(reader, "CommentId"),
                                Message = DbUtils.GetString(reader, "Message"),
                                PostId = postId,
                                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                            });
                        }
                    }

                    reader.Close();
                    return posts;
                }
            }
        }

        public List<Post> HottestPosts(DateTime criterion, bool sortDescending)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var sql = PostQuery(" WHERE p.DateCreated > Convert(datetime, @Criterion)");
                    if (sortDescending)
                    {
                        sql += " ORDER BY p.DateCreated DESC";
                    }
                    else
                    {
                        sql += " ORDER BY p.DateCreated";
                    }

                    cmd.CommandText = sql;
                    DbUtils.AddParameter(cmd, "@Criterion", $"{criterion}");
                    var reader = cmd.ExecuteReader();
                    var posts = new List<Post>();

                    while (reader.Read())
                    {
                        posts.Add(DbUtils.NewPost(reader));
                    }

                    reader.Close();
                    return posts;
                }
            }
        }

        /// <summary>
        /// Selects Posts with the associated UserProfile and Comments.
        /// </summary>
        /// <param name="qualifier">Allows you to adjust call to your needs; WHERE, ORDER BY, etc</param>
        /// <returns></returns>
        private string PostWithCommentsQuery(string qualifier)
        {
            var sql = @"SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated,
                               p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,

                               up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated,
                               up.ImageUrl AS UserProfileImageUrl,

                               c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
                          FROM Post p
                          LEFT JOIN UserProfile up ON p.UserProfileId = up.id
                          LEFT JOIN Comment c on c.PostId = p.id";
            sql += qualifier;
            return sql;
        }
        /// <summary>
        /// Selects Posts and the associated UserProfile.
        /// </summary>
        /// <param name="qualifier">Allows you to adjust call to your needs; WHERE, ORDER BY, etc</param>
        /// <returns></returns>
        private string PostQuery(string qualifier)
        {
            var sql = @"SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated, 
                               p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,

                               up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
                               up.ImageUrl AS UserProfileImageUrl
                          FROM Post p 
                               LEFT JOIN UserProfile up ON p.UserProfileId = up.id";
            sql += qualifier;
            return sql;
        }
    }
}
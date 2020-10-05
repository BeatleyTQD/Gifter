import React, { useContext, useState, useEffect } from 'react';
import { PostContext } from '../providers/PostProvider';
import Post from './Post';
import { useParams } from "react-router-dom";


const UserPosts = () => {
    const { posts, getUserPosts } = useContext(PostContext);
    const { id } = useParams();

    useEffect(() => {
        getUserPosts(id)
    }, []);

    return (
        <div className="container">
            <div className="row justify-content-center">
                <div className="cards-column">
                    {posts.map((post) => (
                        <Post key={post.id} post={post} />
                    ))}
                </div>
            </div>
        </div>
    );
};

export default UserPosts;
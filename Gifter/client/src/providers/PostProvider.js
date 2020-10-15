import React, { useState, useContext } from "react";
import { UserProfileContext } from "./UserProfileProvider";


export const PostContext = React.createContext();

export const PostProvider = (props) => {
    const [posts, setPosts] = useState([]);
    const { getToken } = useContext(UserProfileContext);

    const getAllPosts = () => {
        getToken().then((token) =>
            fetch("/api/post/getwithcomments", {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }).then((res) => res.json())
                .then(setPosts));
    };

    const getPost = (id) => {
        return fetch(`/api/post/getwithcomments/${id}`).then((res) => res.json());
    };

    const getUserPosts = (id) => {
        return fetch(`/api/post/users/${id}`).then((res) => res.json()).then(setPosts);
    }

    const addPost = (post) => {
        getToken().then((token) =>
            fetch("/api/post", {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(post),
            }).then(res => {
                if (res.ok) {
                    return res.json();
                }
                throw new Error("Unauthorized");
            }))
    };

    const searchPosts = (search) => {
        return fetch(`api/post/search?q=${search}&sortDesc=true`)
            .then((res) => res.json())
            .then(setPosts);
    }

    return (
        <PostContext.Provider value={{ posts, getAllPosts, getPost, addPost, searchPosts, getUserPosts }}>
            {props.children}
        </PostContext.Provider>
    );
};
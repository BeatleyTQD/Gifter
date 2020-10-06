import React from "react";
import { Switch, Route } from "react-router-dom";
import PostList from "./PostList";
import PostForm from "./PostForm";
import PostDetails from "./PostDetails";
import UserPosts from "./UserPosts";
import Post from "./Post";
import SearchPosts from "./SearchPosts";

const ApplicationViews = () => {
    return (
        <Switch>
            <Route path="/" exact>
                <SearchPosts />
                <PostList />
            </Route>

            <Route path="/post/add">
                <PostForm />
            </Route>

            <Route path="/post/:id">
                <PostDetails />
            </Route>

            {/* <Route path="post/users/:id">
                <UserPosts />
            </Route> */}
        </Switch>
    );
};

export default ApplicationViews;
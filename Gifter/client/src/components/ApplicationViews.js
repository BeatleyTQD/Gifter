import React, { useContext } from "react";
import { Switch, Route, Redirect } from "react-router-dom";
import PostList from "./PostList";
import PostForm from "./PostForm";
import PostDetails from "./PostDetails";
import { UserProfileContext } from "../providers/UserProfileProvider";
import UserPosts from "./UserPosts";
import Post from "./Post";
import Login from './Login';
import SearchPosts from "./SearchPosts";


const ApplicationViews = () => {
    const { isLoggedIn } = useContext(UserProfileContext);

    return (
        <Switch>
            <Route path="/" exact>
                {isLoggedIn ? <>
                    <SearchPosts />
                    <PostList /> </>
                    : <Redirect to="/login" />}
                <PostList />
            </Route>

            <Route path="/post/add">
                <PostForm />
            </Route>

            <Route path="/post/:id">
                <PostDetails />
            </Route>

            <Route path="/login">
                <Login />
            </Route>
            {/* <Route path="post/users/:id">
                <UserPosts />
            </Route> */}
        </Switch>
    );
};

export default ApplicationViews;
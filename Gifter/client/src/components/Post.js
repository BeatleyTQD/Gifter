import React from "react";
import { NavLink } from "react-router-dom";
import { Card, CardImg, CardBody } from "reactstrap";

const Post = ({ post }) => {

    return (
        <Card className="m-4">
            <NavLink to={`post/users/${post.userProfileId}`} className="text-left px-2">Posted by: {post.userProfile.name}</NavLink>
            <CardImg top src={post.imageUrl} alt={post.title} />
            <CardBody>
                <NavLink to={`/post/${post.id}`}>
                    <strong>{post.title}</strong>
                </NavLink>
                <p>{post.caption}</p>
                <p>Comments</p>
                {post.comments.map((comment) => {
                    return <p key={comment.id}>{comment.userProfileId}: {comment.message}</p>
                })}
            </CardBody>
        </Card>
    );
};

export default Post;
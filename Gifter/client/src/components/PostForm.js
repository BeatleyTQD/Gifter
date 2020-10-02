import React, { useContext, useState } from "react";
import { PostContext } from "../providers/PostProvider";
import { Form, FormGroup, Label, Input, Button } from 'reactstrap';

const PostForm = () => {
    const [post, setPost] = useState({ UserProfileId: 1, Title: "", ImageURL: "", Caption: "", DateCreated: "" })
    const { addPost, getAllPosts } = useContext(PostContext);

    const handleFieldChange = evt => {
        const stateToChange = post;
        stateToChange[evt.target.id] = evt.target.value;
        setPost(stateToChange);
    };

    const constructNewPost = evt => {
        addPost(post)
            .then(() => { getAllPosts() });
        
    }

    return (
        <>
            <div className="container">
                <h2>What GIFts do you have for us today?</h2>
                <Form>
                    <FormGroup>
                        <Label for="Title">Title</Label>
                        <Input type="text" name="Title" id="Title" placeholder="Title" onChange={handleFieldChange} />

                        <Label for="ImageURL">Image URL</Label>
                        <Input type="text" name="ImageURL" id="ImageURL" placeholder="Image URL" onChange={handleFieldChange} />

                        <Label for="Caption">Caption</Label>
                        <Input type="text" name="Caption" id="Caption" placeholder="Caption" onChange={handleFieldChange} />

                        <Label for="DateCreated">Date Created</Label>
                        <Input type="datetime-local" name="DateCreated" id="DateCreated" placeholder="DateCreated" onChange={handleFieldChange} />
                    </FormGroup>
                    <Button color="primary" onClick={constructNewPost}>Submit!</Button>{' '}
                </Form>
            </div>
        </>
    )

};

export default PostForm;
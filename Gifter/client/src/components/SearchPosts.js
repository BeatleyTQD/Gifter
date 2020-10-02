import React, { useContext, useState } from "react";
import { PostContext } from "../providers/PostProvider";
import { Form, FormGroup, Label, Input, Button } from 'reactstrap';
import Post from "./Post";

const SearchPosts = () => {
    const [search, setSearch] = useState("");
    const { searchPosts } = useContext(PostContext);


    const handleFieldChange = evt => {
        const stateToChange = evt.target.value;
        // stateToChange[evt.target.id] = evt.target.value;
        setSearch(stateToChange);
    };

    const newSearch = evt => {
        searchPosts(search);
    }

    return (
        <>
            <div className="container">
                <h2>Search</h2>
                <Form>
                    <FormGroup>
                        <Input type="text" name="Search" id="Search" placeholder="What would you like to search for?" onChange={handleFieldChange} />
                    </FormGroup>
                    <Button color="success" onClick={newSearch}>Search!</Button>{' '}
                </Form>
            </div>
        </>
    )

};

export default SearchPosts;

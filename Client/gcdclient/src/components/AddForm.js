import React from "react"
import { useState } from "react";

function AddForm({ onAddTodo }) {
    const [title, setTitle] = useState('');

    function handleChange(e) {
        setTitle(e.target.value);
    }

    return (
        <form className="row row-cols-lg-auto g-3 align-items-center">
            <div className="form-floating col-11">
                <input type="text" className="form-control" value={title} id="newTodo" onChange={handleChange} required/>
                <label for="floatingInput">Add Todo</label>
            </div>
            <div className="col-1">
                <button className="btn btn-primary btn-sm" style={{ marginLeft:10 }} onClick={() => { onAddTodo(title); setTitle('') }} type="button"><i className="fa fa-plus"></i></button>
            </div>
        </form>
    )
}


export default AddForm
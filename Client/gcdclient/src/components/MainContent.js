import React from "react"

function MainContent({ todos, onUpdateTodo, onDeleteTodo }) {
    return (
        <ul className="list-group">
            {todos.map(todo => 
            <li key={todo.todoItemId} className="list-group-item d-flex justify-content-between align-items-left">
                <input type="checkbox" 
                    checked={todo.checked}
                    onChange={() => onUpdateTodo(todo)} />
                {todo.title}
                <button className="btn btn-danger"><i className="fa fa-trash" onClick={() => onDeleteTodo(todo)}></i></button>
            </li>
            )}
        </ul>
    )
}

export default MainContent
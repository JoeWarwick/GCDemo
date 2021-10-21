import './App.css'
import MainContent from './components/MainContent'
import Loading from './components/Loading'
import { useEffect, useState } from 'react'
import axios from 'axios'

function App() {
  const [todos, setTodos] = useState(null)
  
  useEffect(() => {
    //https://jsonplaceholder.typicode.com/todos
    axios.get("https://gcdapiapi.azure-api.net/todo/todos").then(result => {
      console.table(result.data)
      setTodos(result.data)
    })
  }, [])

  function onUpdateTodo(todo){
    const todoItemIndex = todos.findIndex((x) => x.todoItemId === todo.todoItemId)
    const newTodos = [...todos]

    const newTodo = newTodos[todoItemIndex]
    newTodo.checked = !newTodo.checked
    newTodos[todoItemIndex] = newTodo
    setTodos(newTodos)
  }

  return (
    <div className="App">
      {todos ? (
        <MainContent todos={todos}  onUpdateTodo={onUpdateTodo}/> 
      ) : (
       <Loading />
      )}
    </div>
  )
}

export default App

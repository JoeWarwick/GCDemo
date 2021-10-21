import './App.css'
import MainContent from './components/MainContent'
import Loading from './components/Loading'
import { useEffect, useState } from 'react'
import ToDoService from './service/ToDoService'
import AddForm from './components/AddForm'

function App() {
  const [todos, setTodos] = useState(null)
  
  useEffect(() => {
    ToDoService.getTodos().then(result => {
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
    ToDoService.updateTodo(newTodo)
  }

  function onAddTodo(title){
    if(!title) return;
    const newTodos = [...todos]
    const newTodo = { todoItemId:0, title: title, checked: false, scheduledy: new Date(), added: new Date() }
    newTodos.push(newTodo)
    setTodos(newTodos)
    ToDoService.addTodo(newTodo)
  }

  function onDeleteTodo(todo){
    const todoItemIndex = todos.findIndex((x) => x.todoItemId === todo.todoItemId)
    const newTodos = [...todos]
    newTodos.splice(todoItemIndex, 1);
    setTodos(newTodos)
    ToDoService.delTodo(todo.todoItemId);
  }

  return (
    <div className="App">
      <AddForm onAddTodo={onAddTodo} />
      {todos ? (
        <MainContent todos={todos}  onUpdateTodo={onUpdateTodo} onDeleteTodo={onDeleteTodo} /> 
      ) : (
       <Loading />
      )}
    </div>
  )
}

export default App

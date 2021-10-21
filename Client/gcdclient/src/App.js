import './App.css'
import MainContent from './components/MainContent'
import Loading from './components/Loading'
import { useEffect, useState } from 'react'
import ToDoService from './service/ToDoService'
import AddForm from './components/AddForm'
import Footer from './components/Footer'

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
    ToDoService.updateTodo(newTodo).catch(msg => console.log(msg));
  }

  function onAddTodo(title){
    if(!title) return;
    const newTodos = [...todos]
    const newTodo = { todoItemId:0, title: title, checked: false, scheduledy: new Date(), added: new Date() }
    newTodos.push(newTodo)
    setTodos(newTodos)
    ToDoService.addTodo(newTodo).catch(msg => console.log(msg));
  }

  function onDeleteTodo(todo){
    const todoItemIndex = todos.findIndex((x) => x.todoItemId === todo.todoItemId)
    const newTodos = [...todos]
    newTodos.splice(todoItemIndex, 1);
    setTodos(newTodos)
    ToDoService.delTodo(todo.todoItemId).catch(msg => console.log(msg));
  }

  return (
    <div className="App">
      <h2>ToDo List</h2>
      <AddForm onAddTodo={onAddTodo} />
      {todos ? (
        <MainContent todos={todos}  onUpdateTodo={onUpdateTodo} onDeleteTodo={onDeleteTodo} /> 
      ) : (
       <Loading />
      )}
      <Footer />
    </div>
  )
}

export default App

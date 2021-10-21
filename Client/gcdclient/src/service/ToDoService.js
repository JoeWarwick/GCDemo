import axios from "axios"

const instance = axios.create({
    baseURL: 'https://gcdapiapi.azure-api.net/todo',
    headers: {
        'content-type':'application/json'
    },
});

// eslint-disable-next-line import/no-anonymous-default-export
export default {
    getTodos: () => instance({
        'method':'GET',
        'url': '/todos',
    }),
    addTodo: (todo) => instance({
        'method': 'POST',
        'url':'/todo',
        'data': todo
    }),
    delTodo: (id) => instance({
        'method': 'DELETE',
        'url':'/todo/'+id
    }),
    updateTodo: (todo) => instance({
        'method': 'PUT',
        'url':'/todo',
        'data': todo
    })
}
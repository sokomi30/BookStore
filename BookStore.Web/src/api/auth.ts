import axios from 'axios'

const API = axios.create({ baseURL: 'http://localhost:5000/api' })

export const login = async (username: string, password: string) => {
  const { data } = await API.post('/auth/login', { username, password })
  return data
}

export const register = async (username: string, password: string) => {
  try {
    const { data } = await API.post('/auth/register', { username, password })
    return data
  } catch (error: any) {
    const message = error.response?.data?.message || error.response?.data?.title || 'Registration failed'
    throw new Error(message)
  }
}
import axios from 'axios'

const API = axios.create({ baseURL: 'http://localhost:5000/api' })

API.interceptors.request.use(config => {
  const saved = localStorage.getItem('user')
  if (saved) {
    const user = JSON.parse(saved)
    config.headers.Authorization = `Bearer ${user.token}`
  }
  return config
})

export interface BookDto {
  id: number
  isbn: string
  title: string
  price: number
  authorId: number
  authorFullName: string
}
export interface CreateBookDto {
  isbn: string
  title: string
  price: number
  authorId: number
}

export interface AuthorDto {
  id: number
  fullName: string
  birthDate: string
}

export const createBook = async (dto: CreateBookDto) => {
  const { data } = await API.post('/books', dto)
  return data
}

export const updateBook = async (id: number, dto: CreateBookDto) => {
  const { data } = await API.put(`/books/${id}`, dto)
  return data
}

export const deleteBook = async (id: number) => {
  await API.delete(`/books/${id}`)
}

export const getAuthors = async () => {
  const { data } = await API.get<AuthorDto[]>('/authors')
  return data
}

export const createAuthor = async (dto: { fullName: string; birthDate: string }) => {
  const { data } = await API.post('/authors', dto)
  return data
}

export const deleteAuthor = async (id: number) => {
  await API.delete(`/authors/${id}`)
}

export const getBooks = async () => {
  const { data } = await API.get<BookDto[]>('/books')
  return data
}
export const getCoverUrl = (coverPath: string | null | undefined): string | null => {
  if (!coverPath) return null
  return `http://localhost:5000${coverPath}`
}
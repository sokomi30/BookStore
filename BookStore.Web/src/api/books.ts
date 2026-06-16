import axios from 'axios'

const API = axios.create({ baseURL: '/api' })

API.interceptors.request.use(config => {
  const saved = localStorage.getItem('user')
  if (saved) {
    const user = JSON.parse(saved)
    config.headers.Authorization = `Bearer ${user.token}`
  }
  return config
})

export const getCoverUrl = (coverPath: string | null | undefined): string | undefined => {
  if (!coverPath) return undefined
  return coverPath
}
export interface BookDto {
  id: number
  isbn: string
  title: string
  price: number
  authorId: number
  authorFullName: string
  coverImagePath?: string | null
}

export interface PaginatedResult {
  items: BookDto[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasNextPage: boolean
  hasPreviousPage: boolean
}

export const getBooks = async () => {
  const { data } = await API.get<BookDto[]>('/books')
  return data
}

export const searchBooks = async (title?: string, author?: string) => {
  const { data } = await API.get<BookDto[]>('/books/search', { params: { title, author } })
  return data
}

export const getPaginatedBooks = async (page: number, pageSize: number) => {
  const { data } = await API.get<PaginatedResult>('/books/paginated', { params: { page, pageSize } })
  return data
}

export const getBookById = async (id: number) => {
  const { data } = await API.get<BookDto>(`/books/${id}`)
  return data
}
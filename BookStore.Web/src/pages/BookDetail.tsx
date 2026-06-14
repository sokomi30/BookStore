import { useState, useEffect } from 'react'
import { useParams, Link, useLocation } from 'react-router-dom'
import { getBookById, type BookDto } from '../api/books'

function BookDetail() {
  const { id } = useParams()
  const location = useLocation()
  const [book, setBook] = useState<BookDto | null>(null)

  // Берём search-параметры из предыдущей страницы
  const searchParams = new URLSearchParams(location.state?.fromSearch || '').toString()
  const backUrl = `/${searchParams ? `?${searchParams}` : ''}`

  useEffect(() => { if (id) getBookById(Number(id)).then(setBook) }, [id])

  if (!book) return <p className="text-center mt-20">Loading...</p>

  return (
    <div className="max-w-lg mx-auto mt-20 p-6 bg-white dark:bg-gray-800 rounded shadow">
      <h1 className="text-2xl font-bold mb-4">{book.title}</h1>
      <p><strong>Author:</strong> {book.authorFullName}</p>
      <p><strong>ISBN:</strong> {book.isbn}</p>
      <p><strong>Price:</strong> ${book.price}</p>
      <Link to={backUrl} className="text-blue-500 mt-4 inline-block">← Back to books</Link>
    </div>
  )
}

export default BookDetail
import { useState, useEffect } from 'react'
import { useParams, Link } from 'react-router-dom'
import { getBookById, type BookDto } from '../api/books'

function BookDetail() {
  const { id } = useParams()
  const [book, setBook] = useState<BookDto | null>(null)

  useEffect(() => {
    if (id) {
      getBookById(Number(id)).then(setBook)
    }
  }, [id])

  if (!book) return <p>Loading...</p>

  return (
    <div>
      <h1>{book.title}</h1>
      <p><strong>Author:</strong> {book.authorFullName}</p>
      <p><strong>ISBN:</strong> {book.isbn}</p>
      <p><strong>Price:</strong> ${book.price}</p>
      <Link to="/">← Back to books</Link>
    </div>
  )
}

export default BookDetail
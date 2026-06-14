import { useState, useEffect } from 'react'
import { getBooks, createBook, updateBook, deleteBook, getAuthors, type BookDto, type AuthorDto } from '../api/admin'

function AdminBooks() {
  const [books, setBooks] = useState<BookDto[]>([])
  const [authors, setAuthors] = useState<AuthorDto[]>([])
  const [title, setTitle] = useState('')
  const [isbn, setIsbn] = useState('')
  const [price, setPrice] = useState(0)
  const [authorId, setAuthorId] = useState(0)
  const [editId, setEditId] = useState<number | null>(null)

  useEffect(() => {
    loadBooks()
    getAuthors().then(setAuthors)
  }, [])

  const loadBooks = async () => {
    const data = await getBooks()
    setBooks(data)
  }

  const resetForm = () => {
    setTitle('')
    setIsbn('')
    setPrice(0)
    setAuthorId(0)
    setEditId(null)
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    const dto = { isbn, title, price, authorId }

    if (editId) {
      await updateBook(editId, dto)
    } else {
      await createBook(dto)
    }

    resetForm()
    loadBooks()
  }

  const handleEdit = (book: BookDto) => {
    setEditId(book.id)
    setTitle(book.title)
    setIsbn(book.isbn)
    setPrice(book.price)
    setAuthorId(book.authorId)  // ← замени 0 на book.authorId
}

  const handleDelete = async (id: number) => {
  if (window.confirm('Are you sure you want to delete this book?')) {
    await deleteBook(id)
    loadBooks()
  }
}

  return (
    <div>
      <h1>Manage Books</h1>

      <form onSubmit={handleSubmit}>
        <input placeholder="ISBN" value={isbn} onChange={e => setIsbn(e.target.value)} />
        <input placeholder="Title" value={title} onChange={e => setTitle(e.target.value)} />
        <input type="number" placeholder="Price" value={price} onChange={e => setPrice(Number(e.target.value))} />
        <select value={authorId} onChange={e => setAuthorId(Number(e.target.value))}>
          <option value={0}>Select author</option>
          {authors.map(a => (
            <option key={a.id} value={a.id}>{a.fullName}</option>
          ))}
        </select>
        <button type="submit">{editId ? 'Update' : 'Create'}</button>
        {editId && <button onClick={resetForm}>Cancel</button>}
      </form>

      <ul>
        {books.map(book => (
          <li key={book.id}>
            {book.title} — {book.authorFullName} — ${book.price}
            <button onClick={() => handleEdit(book)}>Edit</button>
            <button onClick={() => handleDelete(book.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  )
}

export default AdminBooks
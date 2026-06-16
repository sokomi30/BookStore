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
  const [adminSearch, setAdminSearch] = useState('')
  useEffect(() => { loadBooks(); getAuthors().then(setAuthors) }, [])
  useEffect(() => { loadBooks() }, [adminSearch])
  const loadBooks = async () => {
    const allBooks = await getBooks()
      if (adminSearch) {
        setBooks(allBooks.filter(b => 
          b.title.toLowerCase().includes(adminSearch.toLowerCase()) ||
          b.authorFullName.toLowerCase().includes(adminSearch.toLowerCase())
        ))
      } else {
        setBooks(allBooks)
    }
  }
  // Функция загрузки
  const handleUploadCover = async (bookId: number, file: File) => {
  const formData = new FormData()
  formData.append('file', file)

  const saved = localStorage.getItem('user')
  const token = saved ? JSON.parse(saved).token : ''

  await fetch(`http://localhost:5000/api/books/${bookId}/cover`, {
    method: 'POST',
    headers: { 'Authorization': `Bearer ${token}` },
    body: formData
  })

  loadBooks()
}
  const resetForm = () => { setTitle(''); setIsbn(''); setPrice(0); setAuthorId(0); setEditId(null) }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    editId ? await updateBook(editId, { isbn, title, price, authorId }) : await createBook({ isbn, title, price, authorId })
    resetForm(); loadBooks()
  }

  const handleDelete = async (id: number) => {
    if (window.confirm('Delete this book?')) { await deleteBook(id); loadBooks() }
  }

  return (
    <div className="max-w-4xl mx-auto p-6">
      <h1 className="text-2xl font-bold mb-4">Manage Books</h1>

      <form onSubmit={handleSubmit} className="bg-white dark:bg-gray-800 p-4 rounded shadow mb-6 flex flex-wrap gap-3">
        <input className="border rounded px-3 py-2 flex-1" placeholder="ISBN" value={isbn} onChange={e => setIsbn(e.target.value)} />
        <input className="border rounded px-3 py-2 flex-1" placeholder="Title" value={title} onChange={e => setTitle(e.target.value)} />
        <input className="border rounded px-3 py-2 w-24" type="number" placeholder="Price" value={price} onChange={e => setPrice(Number(e.target.value))} />
        <select className="border rounded px-3 py-2" value={authorId} onChange={e => setAuthorId(Number(e.target.value))}>
          <option value={0}>Select author</option>
          {authors.map(a => <option key={a.id} value={a.id}>{a.fullName}</option>)}
        </select>
        <input
          className="border dark:border-gray-600 dark:bg-gray-700 rounded px-3 py-2 flex-1"
          placeholder="Search books..."
          value={adminSearch}
          onChange={e => setAdminSearch(e.target.value)}
        />
        <button className="bg-blue-500 text-white px-4 py-2 rounded" type="submit">{editId ? 'Update' : 'Create'}</button>
        {editId && <button className="bg-gray-300 px-4 py-2 rounded" onClick={resetForm}>Cancel</button>}
        
      </form>

      <div className="space-y-2">
        {books.map(book => (
          <div key={book.id} className="bg-white dark:bg-gray-800 rounded shadow p-3 flex justify-between items-center">
            <span>{book.title} — {book.authorFullName} — ${book.price}</span>
            <div className="flex gap-2">
                <input
                  type="file"
                  accept="image/*"
                  onChange={e => {
                    const file = e.target.files?.[0]
                    if (file) handleUploadCover(book.id, file)
                  }}
                  className="text-sm"
                />
              <button onClick={() => { setEditId(book.id); setTitle(book.title); setIsbn(book.isbn); setPrice(book.price); setAuthorId(book.authorId) }}
                className="bg-yellow-400 px-3 py-1 rounded">Edit</button>
              <button onClick={() => handleDelete(book.id)} className="bg-red-500 text-white px-3 py-1 rounded">Delete</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default AdminBooks
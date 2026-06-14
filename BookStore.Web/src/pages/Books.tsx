import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { getPaginatedBooks, searchBooks, type BookDto, type PaginatedResult } from '../api/books'

function Books() {
  const [data, setData] = useState<PaginatedResult | null>(null)
  const [books, setBooks] = useState<BookDto[]>([])
  const [searchTitle, setSearchTitle] = useState('')
  const [searchAuthor, setSearchAuthor] = useState('')
  const [page, setPage] = useState(1)
  const [isSearching, setIsSearching] = useState(false)

  useEffect(() => {
    loadBooks()
  }, [page])

  const loadBooks = async () => {
    const result = await getPaginatedBooks(page, 10)
    setData(result)
    setBooks(result.items)
    setIsSearching(false)
  }

  const handleSearch = async () => {
    if (!searchTitle && !searchAuthor) {
      loadBooks()
      return
    }
    const result = await searchBooks(searchTitle, searchAuthor)
    setBooks(result)
    setIsSearching(true)
  }

  return (
    <div>
      <h1>Books</h1>

      <div>
        <input
          type="text"
          placeholder="Search by title"
          value={searchTitle}
          onChange={e => setSearchTitle(e.target.value)}
        />
        <input
          type="text"
          placeholder="Search by author"
          value={searchAuthor}
          onChange={e => setSearchAuthor(e.target.value)}
        />
        <button onClick={handleSearch}>Search</button>
        <button onClick={loadBooks}>Clear</button>
      </div>

      <ul>
        {books.map(book => (
          <li key={book.id}>
            <Link to={`/books/${book.id}`}>
              {book.title} — {book.authorFullName} — ${book.price}
            </Link>
          </li>
        ))}
      </ul>

      {!isSearching && data && (
        <div>
          <button disabled={!data.hasPreviousPage} onClick={() => setPage(page - 1)}>
            Previous
          </button>
          <span> Page {data.page} of {data.totalPages} </span>
          <button disabled={!data.hasNextPage} onClick={() => setPage(page + 1)}>
            Next
          </button>
        </div>
      )}
    </div>
  )
}

export default Books
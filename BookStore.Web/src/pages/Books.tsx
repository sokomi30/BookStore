import { useState, useEffect, useCallback } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { getPaginatedBooks, searchBooks, type BookDto, type PaginatedResult } from '../api/books'

function Books() {
  const [searchParams, setSearchParams] = useSearchParams()
  const [data, setData] = useState<PaginatedResult | null>(null)
  const [books, setBooks] = useState<BookDto[]>([])
  const [searchTitle, setSearchTitle] = useState(searchParams.get('title') || '')
  const [searchAuthor, setSearchAuthor] = useState(searchParams.get('author') || '')
  const [page, setPage] = useState(Number(searchParams.get('page')) || 1)
  const [isSearching, setIsSearching] = useState(false)

  const loadBooks = useCallback(async () => {
    const result = await getPaginatedBooks(page, 10)
    setData(result)
    setBooks(result.items)
    setIsSearching(false)
    setSearchParams({})
  }, [page, setSearchParams])

  useEffect(() => {
    const titleParam = searchParams.get('title')
    const authorParam = searchParams.get('author')

    if (titleParam || authorParam) {
      handleSearch()
    } else {
      loadBooks()
    }
  }, [page])

  const handleSearch = async () => {
    if (!searchTitle && !searchAuthor) {
      loadBooks()
      return
    }

    if ((searchTitle && searchTitle.length < 3) || (searchAuthor && searchAuthor.length < 3)) return

    const params: Record<string, string> = {}
    if (searchTitle) params.title = searchTitle
    if (searchAuthor) params.author = searchAuthor
    setSearchParams(params)

    const result = await searchBooks(searchTitle || undefined, searchAuthor || undefined)
    setBooks(result)
    setIsSearching(true)
  }

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') handleSearch()
  }

  return (
    <div className="max-w-4xl mx-auto p-6">
      <h1 className="text-2xl font-bold mb-4">Books</h1>

      <div className="flex gap-2 mb-6">
        <input
          className="border dark:border-gray-600 dark:bg-gray-700 rounded px-3 py-2 flex-1"
          placeholder="Search by title"
          value={searchTitle}
          onChange={e => setSearchTitle(e.target.value)}
          onKeyDown={handleKeyDown}
        />
        <input
          className="border dark:border-gray-600 dark:bg-gray-700 rounded px-3 py-2 flex-1"
          placeholder="Search by author"
          value={searchAuthor}
          onChange={e => setSearchAuthor(e.target.value)}
          onKeyDown={handleKeyDown}
        />
        <button onClick={handleSearch} className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition">
          Search
        </button>
        <button
          onClick={() => { setSearchTitle(''); setSearchAuthor(''); loadBooks() }}
          className="bg-gray-300 dark:bg-gray-600 px-4 py-2 rounded hover:bg-gray-400 transition"
        >
          Clear
        </button>
      </div>

      {((searchTitle.length > 0 && searchTitle.length < 3) || (searchAuthor.length > 0 && searchAuthor.length < 3)) ? (
        <p className="text-yellow-500 text-sm -mt-4 mb-4">Minimum 3 characters required to search</p>
      ) : (
        <p className="text-gray-400 dark:text-gray-500 text-sm -mt-4 mb-4">Enter at least 3 characters to search</p>
      )}

      {books.length === 0 && (
        <p className="text-center text-gray-400 dark:text-gray-500 mt-10 text-lg">
          No books found. Try a different search term.
        </p>
      )}

      <div className="grid gap-3">
        {books.map(book => (
          <Link
            key={book.id}
            to={`/books/${book.id}`}
            state={{ fromSearch: window.location.search }}
            className="bg-white dark:bg-gray-800 rounded shadow p-4 hover:shadow-md transition"
          >
            <h2 className="font-semibold text-lg">{book.title}</h2>
            <p className="text-gray-500 dark:text-gray-400">{book.authorFullName} — ${book.price}</p>
          </Link>
        ))}
      </div>

      {!isSearching && data && (
        <div className="flex items-center justify-center gap-4 mt-6">
          <button
            disabled={!data.hasPreviousPage}
            onClick={() => setPage(page - 1)}
            className="bg-gray-200 dark:bg-gray-700 px-4 py-2 rounded disabled:opacity-50"
          >
            ← Previous
          </button>
          <span>Page {data.page} of {data.totalPages}</span>
          <button
            disabled={!data.hasNextPage}
            onClick={() => setPage(page + 1)}
            className="bg-gray-200 dark:bg-gray-700 px-4 py-2 rounded disabled:opacity-50"
          >
            Next →
          </button>
        </div>
      )}
    </div>
  )
}

export default Books
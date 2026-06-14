import { useState, useEffect } from 'react'
import { getAuthors, createAuthor, deleteAuthor, type AuthorDto } from '../api/admin'

function AdminAuthors() {
  const [authors, setAuthors] = useState<AuthorDto[]>([])
  const [fullName, setFullName] = useState('')
  const [birthDate, setBirthDate] = useState('')

  useEffect(() => {
    loadAuthors()
  }, [])

  const loadAuthors = async () => {
    const data = await getAuthors()
    setAuthors(data)
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    await createAuthor({ fullName, birthDate })
    setFullName('')
    setBirthDate('')
    loadAuthors()
  }

  return (
    <div>
      <h1>Manage Authors</h1>

      <form onSubmit={handleSubmit}>
        <input placeholder="Full Name" value={fullName} onChange={e => setFullName(e.target.value)} />
        <input type="date" value={birthDate} onChange={e => setBirthDate(e.target.value)} />
        <button type="submit">Create</button>
      </form>

      <ul>
        {authors.map(author => (
          <li key={author.id}>
            {author.fullName} — {new Date(author.birthDate).toLocaleDateString()}
            <button onClick={() => {
                 if (window.confirm('Are you sure you want to delete this author?')) {
                     deleteAuthor(author.id).then(loadAuthors)
                    }
            }}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  )
}

export default AdminAuthors
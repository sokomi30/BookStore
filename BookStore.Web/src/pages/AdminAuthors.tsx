import { useState, useEffect } from 'react'
import { getAuthors, createAuthor, deleteAuthor, type AuthorDto } from '../api/admin'

function AdminAuthors() {
  const [authors, setAuthors] = useState<AuthorDto[]>([])
  const [fullName, setFullName] = useState('')
  const [birthDate, setBirthDate] = useState('')

  useEffect(() => { loadAuthors() }, [])

  const loadAuthors = async () => setAuthors(await getAuthors())

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    await createAuthor({ fullName, birthDate })
    setFullName(''); setBirthDate(''); loadAuthors()
  }

  return (
    <div className="max-w-4xl mx-auto p-6">
      <h1 className="text-2xl font-bold mb-4">Manage Authors</h1>

      <form onSubmit={handleSubmit} className="bg-white dark:bg-gray-800 p-4 rounded shadow mb-6 flex gap-3">
        <input className="border rounded px-3 py-2 flex-1" placeholder="Full Name" value={fullName} onChange={e => setFullName(e.target.value)} />
        <input className="border rounded px-3 py-2" type="date" value={birthDate} onChange={e => setBirthDate(e.target.value)} />
        <button className="bg-blue-500 text-white px-4 py-2 rounded" type="submit">Create</button>
      </form>

      <div className="space-y-2">
        {authors.map(author => (
          <div key={author.id} className="bg-white dark:bg-gray-800 rounded shadow p-3 flex justify-between items-center">
            <span>{author.fullName} — {new Date(author.birthDate).toLocaleDateString()}</span>
            <button onClick={() => { if (window.confirm('Delete?')) deleteAuthor(author.id).then(loadAuthors) }}
              className="bg-red-500 text-white px-3 py-1 rounded">Delete</button>
          </div>
        ))}
      </div>
    </div>
  )
}

export default AdminAuthors
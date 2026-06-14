import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { register as registerApi } from '../api/auth'

function Register() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const { login } = useAuth()
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      const data = await registerApi(username, password)
      login(data)
      navigate('/')
    } catch (err: any) { setError(err.message || 'Registration failed') }
  }

  return (
    <div className="max-w-sm mx-auto mt-20 p-6 bg-white dark:bg-gray-800 rounded shadow">
      <h1 className="text-2xl font-bold mb-4">Register</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-3">
        <input className="border rounded px-3 py-2" placeholder="Username"
          value={username} onChange={e => setUsername(e.target.value)} />
        <input className="border rounded px-3 py-2" type="password" placeholder="Password"
          value={password} onChange={e => setPassword(e.target.value)} />
        <button className="bg-green-500 text-white py-2 rounded" type="submit">Register</button>
      </form>
      {error && <p className="text-red-500 mt-2">{error}</p>}
      <p className="mt-4 text-sm">Already have an account? <Link to="/login" className="text-blue-500">Login</Link></p>
    </div>
  )
}

export default Register
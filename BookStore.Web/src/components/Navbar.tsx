import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { useTheme } from '../context/ThemeContext'

function Navbar() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const { theme, toggle } = useTheme()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <nav className="bg-white dark:bg-gray-800 shadow p-4 flex items-center justify-between">
      <div className="flex items-center gap-6">
        <Link to="/" className="text-xl font-bold text-blue-600">📚 BookStore</Link>
        <Link to="/" className="hover:text-blue-600">Books</Link>
        {user?.role === 'Admin' && (
          <>
            <Link to="/admin/books" className="hover:text-blue-600">Manage Books</Link>
            <Link to="/admin/authors" className="hover:text-blue-600">Manage Authors</Link>
          </>
        )}
      </div>
      <div className="flex items-center gap-4">
        {user ? (
          <>
            <span className="text-sm text-gray-500">{user.username} ({user.role})</span>
            <button onClick={toggle} className="bg-gray-200 dark:bg-gray-700 px-3 py-1 rounded text-sm">
                {theme === 'light' ? '🌙' : '☀️'}
            </button>
            <button onClick={handleLogout} className="bg-red-500 text-white px-3 py-1 rounded">Logout</button>
          </>
        ) : (
          <>
            <Link to="/login" className="bg-blue-500 text-white px-3 py-1 rounded">Login</Link>
            <Link to="/register" className="bg-green-500 text-white px-3 py-1 rounded">Register</Link>
          </>
        )}
      </div>
    </nav>
  )
}

export default Navbar
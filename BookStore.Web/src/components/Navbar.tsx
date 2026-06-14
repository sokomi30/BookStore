import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

function Navbar() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <nav>
      <Link to="/">Books</Link>
      {user ? (
        <>
          <span>Hello, {user.username} ({user.role})</span>
          {user.role === 'Admin' && (
            <>
              <Link to="/admin/books">Manage Books</Link>
              <Link to="/admin/authors">Manage Authors</Link>
            </>
          )}
          <button onClick={handleLogout}>Logout</button>
        </>
      ) : (
        <>
          <Link to="/login">Login</Link>
          <Link to="/register">Register</Link>
        </>
      )}
    </nav>
  )
}

export default Navbar
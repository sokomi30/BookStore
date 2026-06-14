import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { AuthProvider } from './context/AuthContext'
import { ThemeProvider } from './context/ThemeContext'
import Navbar from './components/Navbar'
import ProtectedRoute from './components/ProtectedRoute'
import Login from './pages/Login'
import Register from './pages/Register'
import Books from './pages/Books'
import BookDetail from './pages/BookDetail'
import AdminBooks from './pages/AdminBooks'
import AdminAuthors from './pages/AdminAuthors'

function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
        <BrowserRouter>
          <div className="min-h-screen bg-white dark:bg-gray-900 text-gray-900 dark:text-gray-100 transition-colors">
            <Navbar />
            <Routes>
              <Route path="/" element={<Books />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/books/:id" element={<BookDetail />} />
              <Route path="/admin/books" element={<ProtectedRoute adminOnly><AdminBooks /></ProtectedRoute>} />
              <Route path="/admin/authors" element={<ProtectedRoute adminOnly><AdminAuthors /></ProtectedRoute>} />
            </Routes>
          </div>
        </BrowserRouter>
      </AuthProvider>
    </ThemeProvider>
  )
}

export default App
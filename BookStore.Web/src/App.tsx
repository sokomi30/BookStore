import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { AuthProvider } from './context/AuthContext'
import Login from './pages/Login'
import Register from './pages/Register'
import Books from './pages/Books'
import BookDetail from './pages/BookDetail'
import AdminBooks from './pages/AdminBooks'
import AdminAuthors from './pages/AdminAuthors'
import Navbar from './components/Navbar'
import ProtectedRoute from './components/ProtectedRoute'

// Внутри BrowserRouter, перед Routes:

function App() {
  return (
    <AuthProvider>
      <BrowserRouter><Navbar />
        <Routes>
            ...
        </Routes>
        <Routes>
            <Route path="/" element={<Books />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/books/:id" element={<BookDetail />} />
            <Route path="/admin/books" element={
                <ProtectedRoute adminOnly>
                    <AdminBooks />
                </ProtectedRoute>
            } />
            <Route path="/admin/authors" element={
                <ProtectedRoute adminOnly>
                    <AdminAuthors />
                </ProtectedRoute>
            } />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App